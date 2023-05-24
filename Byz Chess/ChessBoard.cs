using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Byz_Chess.Pieces;
using Byz_Chess.Resources;
using Helpers;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Byz_Chess
{
    public class ChessBoard
    {

        public List<CircularList<Position>> Positions;

        public Position SelectedPosition;
        private readonly List<Position> _shownMovesPositions = new();

        public int PlayerToPlay;

        public readonly Brush Color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF502D16"));
        public readonly Brush Color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD38D5F"));
        private readonly Brush[] _colors;
        public Position[] KingPieces = new Position[3];
        public List<Position> CheckingPieces = new();

        public ChessBoard(IEnumerable<Resources.PositionUC> positions, int height)
        {
            _colors = new Brush[]
            {
                Color1, Color2
            };
            SelectedPosition = Position.Empty;
            Positions = new List<CircularList<Position>>();
            InitChessBoard(positions, height);
        }

        private void InitChessBoard(IEnumerable<Resources.PositionUC> positions, int height)
        {
            var paths = positions as Resources.PositionUC[] ?? positions.ToArray();
            if (paths.Length % height != 0)
            {
                MessageBox.Show("Board size is invalid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var width = paths.Length / height;
            var i = 0;
            var tempList = new CircularList<Position>();
            foreach (var position in paths)
            {

                tempList.Add(new Position()
                {
                    Drawing = position,
                    Color = _colors[(i + Positions.Count) % 2]
                });
                i++;
                if (i % width != 0) continue;
                Positions.Add(tempList);
                tempList = new CircularList<Position>();
                i = 0;
            }
        }

        public void PlacePiece(IPiece piece, Position position)
        {
            position.Piece = piece;
        }

        public void ShowPossibleMoves()
        {
            var pieceMoves = SelectedPosition.GetMoves().ToList();
            if (SelectedPosition?.Piece == null) return;
            if (SelectedPosition?.Piece is Pawn)
            {
                var mirror = Convert.ToInt32((SelectedPosition?.Piece as Pawn).Team == 2) + Convert.ToInt32((SelectedPosition.Column > Positions.First().Count / 2 - 1));
                if (SelectedPosition.Piece.SideConscious && mirror % 2 == 1)
                {
                    pieceMoves = MirrorMoves();
                }
            }
            foreach (var move in pieceMoves)
            {
                if (ValidMove(move))
                {
                    var position = Positions[SelectedPosition.Row + move.row][SelectedPosition.Column + move.col];
                    _shownMovesPositions.Add(position);
                    position.MoveShadow = true;
                }

            }

            List<Offset> MirrorMoves()
            {
                var mirroredMoves = new List<Offset>(); //I really dislike this but don't want to send the piece its position
                for (int i = 0; i < pieceMoves.Count(); i++)
                {
                    mirroredMoves.Add(new Offset(pieceMoves[i].row, -pieceMoves[i].col, pieceMoves[i].taking));
                }
                return mirroredMoves;
            }
        }

        public void ClearShownMoves()
        {
            foreach (var move in _shownMovesPositions)
            {
                move.MoveShadow = false;
            }
            _shownMovesPositions.Clear();
        }

        //TODO check this
        private bool ValidMove(Offset move, Position startingPosition = null)
        {
            if (startingPosition == null)
                startingPosition = SelectedPosition;
            if (!IsOnBoard(move, startingPosition)) return false;
            var position = Positions[startingPosition.Row + move.row][startingPosition.Column + move.col];
            if (startingPosition.Piece == null) return false; ;
            if (startingPosition.Piece.Grounded)
            {
                if (!ValidMoveForGrouded(move, position,startingPosition))
                    return false;
            }
            if (position.Piece == null)
            {
                return !move.taking;
            }
            var alliedPiece = position.Piece?.Team == startingPosition.Piece?.Team;
            if (alliedPiece) return false;
            if (startingPosition.Piece is Pawn)
            {
                return move.taking == true;
            }
            return true;

        }

        private bool IsOnBoard(Offset move, Position startingPosition)
        {
            var positionRow = startingPosition.Row + move.row;
            var onBoard = positionRow < Positions.Count && positionRow >= 0;
            if (!onBoard) return false;
            return true;
        }

        private bool ValidMoveForGrouded(Offset move, Position position, Position startingPosition)  //This will only work for rook but it's ok since he is the only grounded piece
        {
            var positionsToCheck = new CircularList<Position>();
            var blocked = false;
            if (move.row == 0)
            {
                var start = startingPosition.Column;
                var end = position.Column;
                positionsToCheck = Positions[startingPosition.Row];
                blocked = CheckPath(start, end, positionsToCheck.Count) && CheckPath(end, start, positionsToCheck.Count);

            }
            if (move.col == 0)
            {
                var start = startingPosition.Row;
                var end = position.Row;
                positionsToCheck = new CircularList<Position>()
                {
                    Positions[0][startingPosition.Column],
                    Positions[1][startingPosition.Column],
                    Positions[2][startingPosition.Column],
                    Positions[3][startingPosition.Column]

            };
                if (start < end)
                {
                    blocked = CheckPath(start, end, positionsToCheck.Count);
                }
                else
                {
                    blocked = CheckPath(end, start, positionsToCheck.Count);

                }
            }

            bool CheckPath(int start, int end, int count)
            {
                bool hasPiece = false;

                for (int i = (start + 1) % count; i != end; i = (i + 1) % count)
                {
                    if (positionsToCheck[i].Piece != null)
                    {
                        hasPiece = true;
                        break;
                    }
                }

                return hasPiece;
            }

            return !blocked;
        }

        public void DoOnlineMove(int selectedRow, int selectedColumn, int newRow, int newColumn)
        {
            SelectedPosition = Positions[selectedRow][selectedColumn];
            ShowPossibleMoves();
            TryMovePiece(Positions[newRow][newColumn]);
            ClearShownMoves();
        }

        public bool TryMovePiece(Position position)
        {
            if (!_shownMovesPositions.Contains(position)) return false;
            var tempSelectedPosition = SelectedPosition.Piece;
            var tempNextPosition = position.Piece;
            var tempKingPosition = new Position();
            position.Piece = SelectedPosition.Piece;
            SelectedPosition.Piece = null;
            if (position.Piece is King)
            {
                tempKingPosition = KingPieces[PlayerToPlay];
                KingPieces[PlayerToPlay] = position;
            }
            if (IsInCheck(KingPieces[PlayerToPlay]))
            {
                SelectedPosition.Piece = tempSelectedPosition;
                position.Piece = tempNextPosition;
                if (SelectedPosition.Piece is King)
                {
                    KingPieces[PlayerToPlay] = tempKingPosition;
                }
                FlashKing();
                return false;
            }

            return true;
        }

        private bool IsInCheck(Position kingPiece)
        {
            CheckingPieces.Clear();
            foreach (var moveSet in Globals.AllPossibleMoves)
            {
                foreach (var move in moveSet.MovesList)
                {
                    var position = new Position();
                    var newMove = move;
                    if (moveSet.ClassType == typeof(Pawn))
                    {
                        if (kingPiece.Row - move.row > 0)
                        {
                            position = Positions[kingPiece.Row - move.row][kingPiece.Column - move.col];
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!IsOnBoard(move, kingPiece)) continue;
                        position = Positions[kingPiece.Row + move.row][kingPiece.Column + move.col];
                        newMove = new Offset(-move.row, -move.col);
                    }
                    if (ValidMove(newMove, position) && moveSet.ClassType == position.Piece?.GetType() && position.Piece.Team != PlayerToPlay)
                        CheckingPieces.Add(position);
                }
            }

            return CheckingPieces.Count > 0;
        }
        public bool IsCheckMate()
        {
            if (!IsInCheck(KingPieces[PlayerToPlay]))
                return false;
            foreach (var column in Positions)
            {
                foreach (var position in column)
                {
                    if (position.Piece?.Team == PlayerToPlay)
                    {
                        foreach (var move in position.GetMoves())
                        {
                            var trueMove = move;
                            if (position?.Piece is Pawn)
                            {
                                var mirror = Convert.ToInt32((position?.Piece as Pawn).Team == 2) +
                                             Convert.ToInt32((position.Column > Positions.First().Count / 2 - 1));
                                if (position.Piece.SideConscious && mirror % 2 == 1)
                                {
                                    trueMove = new Offset(move.row, -move.col, move.taking);
                                }
                            }

                            if (ValidMove(trueMove, position))
                            {
                                SelectedPosition = position;
                                var tempPositionPiece = position.Piece;
                                var pos = Positions[position.Row + trueMove.row][position.Column + trueMove.col];
                                var tempNewPositionPiece = pos.Piece;
                                var tempKingPosition = new Position();
                                if (position.Piece is King)
                                {
                                    tempKingPosition = KingPieces[PlayerToPlay];
                                }
                                var moved = TryMovePiece(pos);

                                if (!IsInCheck(KingPieces[PlayerToPlay]))
                                {
                                    if (moved)
                                    {
                                        SelectedPosition.Piece = tempPositionPiece;
                                        pos.Piece = tempNewPositionPiece;
                                        if (position.Piece is King)
                                        {
                                            KingPieces[PlayerToPlay] = tempKingPosition;
                                        }

                                    }

                                    return false;
                                }

                                if (moved)
                                {
                                    SelectedPosition.Piece = tempPositionPiece;
                                    pos.Piece = tempNewPositionPiece;
                                    if (position.Piece is King)
                                    {
                                        KingPieces[PlayerToPlay] = tempKingPosition;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        //Does not work
        public void FlashKing()
        {
            KingPieces[PlayerToPlay].Drawing.Fill = Brushes.Red;
            Task.Delay(1000);
            KingPieces[PlayerToPlay].Drawing.Fill = KingPieces[PlayerToPlay].Color;
            Task.Delay(1000);
            KingPieces[PlayerToPlay].Drawing.Fill = Brushes.Red;
            Task.Delay(1000);
            KingPieces[PlayerToPlay].Drawing.Fill = KingPieces[PlayerToPlay].Color;
        }

        public void ClearPositions()
        {
            foreach (var row in Positions)
            {
                foreach (var position in row)
                {
                    position.Piece = null;
                }
            }
        }
    }
}

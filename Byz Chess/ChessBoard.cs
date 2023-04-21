using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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

        public readonly Brush Color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF502D16"));
        public readonly Brush Color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD38D5F"));
        private readonly Brush[] _colors;
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
            ClearShownMoves();
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

        private void ClearShownMoves()
        {
            foreach (var move in _shownMovesPositions)
            {
                move.MoveShadow = false;
            }
            _shownMovesPositions.Clear();
        }

        //TODO check this
        private bool ValidMove(Offset move)
        {
            var positionRow = SelectedPosition.Row + move.row;
            var onBoard = positionRow < Positions.Count && positionRow >= 0;
            if (!onBoard) return false;
            var position = Positions[SelectedPosition.Row + move.row][SelectedPosition.Column + move.col];
            if (SelectedPosition.Piece.Grounded)
            {
                var blocked = 0;
                if (move.col == 0)
                {
                    for (int i = SelectedPosition.Row + 1; i < position.Row; i++)
                        if (Positions[i][SelectedPosition.Column].Piece != null)
                        {
                            blocked++;
                            break;
                        }
                    if (blocked == 1)
                        for (int i = position.Row - 1; i < SelectedPosition.Row; i--)
                            if (Positions[i][SelectedPosition.Column].Piece != null)
                            {
                                return false;
                            }
                }
                if (move.row == 0)
                {
                    for (int i = SelectedPosition.Column + 1; i < position.Column; i++)
                        if (Positions[SelectedPosition.Row][i].Piece != null)
                        {
                            blocked++;
                            break;
                        }
                    if (blocked == 1)
                        for (int i = position.Column + 1; i < SelectedPosition.Column; i++)
                            if (Positions[SelectedPosition.Row][i].Piece != null)
                            {
                                return false;
                            }
                }
            }
            if (position.Piece == null)
            {
                return !move.taking;
            }
            var alliedPiece = position.Piece?.Team == SelectedPosition.Piece?.Team;
            if (alliedPiece) return false;
            if (SelectedPosition.Piece is Pawn)
            {
                return move.taking == true;
            }
            return true;
        }

        public void TryMovePiece(Position position)
        {
            if (!_shownMovesPositions.Contains(position)) return;

            if (SelectedPosition.Piece is Pawn pwn)
                pwn.Moved();
            position.Piece = SelectedPosition.Piece;
            SelectedPosition.Piece = null;

        }
    }
}

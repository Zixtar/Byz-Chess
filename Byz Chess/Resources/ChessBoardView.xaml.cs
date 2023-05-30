using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Byz_Chess.Pieces;

namespace Byz_Chess.Resources
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoardView : UserControl
    {
        public ChessBoard Board;
        public bool GameStarted = false;
        private const int TeamsToggle = 1 ^ 2;
        public int PlayerNr;
        public bool Online = false;
        public bool VsAI = false;
        public int AIdepth = 3;
        AI Ai;
        private int _firstThreeMoves = 0;

        private Position SelectedPosition
        {
            get => Board.SelectedPosition;
            set
            {
                if (Board.TryMovePiece(value))
                {
                    if (VsAI)
                    {
                        Board.PlayerToPlay ^= TeamsToggle;
                        if (this._firstThreeMoves < 2)
                        {
                            Ai.EvaluateRandom(Board);
                            this._firstThreeMoves += 1;
                        }
                        else
                        {
                            Ai.EvaluateAI(Board);
                        }
                    }
                    else if (Online)
                    {
                        Globals.ScriereServer.WriteLine($"M{SelectedPosition.Row}{SelectedPosition.Column}|{value.Row}{value.Column}");
                    }
                    if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = SelectedPosition.Color;
                    Board.SelectedPosition = value;
                    if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = Brushes.GreenYellow;
                    Board.PlayerToPlay ^= TeamsToggle;
                    if (Board.IsCheckMate())
                    {
                        if (Online)
                        {
                            Globals.ScriereServer.WriteLine("F");
                        }
                        MessageBox.Show("CheckMate");
                        ResetGame();
                    }
                }

                Board.ClearShownMoves();

                if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = SelectedPosition.Color;
                Board.SelectedPosition = value;
                if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = Brushes.GreenYellow;

                if (SelectedPosition?.Piece?.Team == Board.PlayerToPlay)
                {
                    if (Online)
                    {
                        if (PlayerNr == Board.PlayerToPlay)
                        {
                            Board.ShowPossibleMoves();
                        }
                    }
                    else
                    {
                        Board.ShowPossibleMoves();
                    }

                }
            }
        }

        public void ResetGame()
        {
            Board.PlayerToPlay = 1;
            Board.ClearPositions();
            ArrangeStandardBoard();
        }

        public void DoOnlineMove(int selectedRow, int selectedColumn, int newRow, int newColumn)
        {
            if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = SelectedPosition.Color;
            Board.DoOnlineMove(selectedRow, selectedColumn, newRow, newColumn);
            Board.PlayerToPlay ^= TeamsToggle;
        }


        public ChessBoardView()
        {
            InitializeComponent();
            var positionPaths = new SortedSet<PositionUC>(BoardCanvas.Children.OfType<PositionUC>(), new PathComparer<PositionUC>());
            Board = new ChessBoard(positionPaths, 4);
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (PositionUC)sender;
            var row = cell.Name.First() - 'A';
            var col = Convert.ToInt32(cell.Name[1..]) - 1;
            if (SelectedPosition == Board.Positions[row][col])
            {
                SelectedPosition = Position.Empty;
                return;
            }

            SelectedPosition = Board.Positions[row][col];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Board.PlacePiece(new Pawn(1), SelectedPosition);
        }

        private class PathComparer<T> : IComparer<PositionUC>
        {
            public int Compare(PositionUC? x, PositionUC? y) //horrible horrible comparer
            {
                if (x == null || y == null) return 0;

                var name1 = x.Name;
                var name2 = y.Name;
                if (name1.Length != y.Name.Length && name1.First() == name2.First()) return Math.Sign((name1.Length - name2.Length));
                return string.Compare(name1, name2);
            }
        }

        public void ArrangeStandardBoard()
        {
            Board.PlacePiece(new InversePawn(1), Board.Positions[0][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[1][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[2][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[3][-2]);
            Board.PlacePiece(new King(1), Board.Positions[0][-1]);
            Board.PlacePiece(new Elephant(1), Board.Positions[1][-1]);
            Board.PlacePiece(new Horse(1), Board.Positions[2][-1]);
            Board.PlacePiece(new Rook(1), Board.Positions[3][-1]);
            Board.PlacePiece(new Minister(1), Board.Positions[0][0]);
            Board.PlacePiece(new Elephant(1), Board.Positions[1][0]);
            Board.PlacePiece(new Horse(1), Board.Positions[2][0]);
            Board.PlacePiece(new Rook(1), Board.Positions[3][0]);
            Board.PlacePiece(new Pawn(1), Board.Positions[0][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[1][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[2][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[3][1]);

            Board.PlacePiece(new Pawn(2), Board.Positions[0][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[1][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[2][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[3][9]);
            Board.PlacePiece(new King(2), Board.Positions[0][8]);
            Board.PlacePiece(new Elephant(2), Board.Positions[1][8]);
            Board.PlacePiece(new Horse(2), Board.Positions[2][8]);
            Board.PlacePiece(new Rook(2), Board.Positions[3][8]);
            Board.PlacePiece(new Minister(2), Board.Positions[0][7]);
            Board.PlacePiece(new Elephant(2), Board.Positions[1][7]);
            Board.PlacePiece(new Horse(2), Board.Positions[2][7]);
            Board.PlacePiece(new Rook(2), Board.Positions[3][7]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[0][6]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[1][6]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[2][6]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[3][6]);

            Board.KingPieces[1] = Board.Positions[0][-1];
            Board.KingPieces[2] = Board.Positions[0][8];

            if (VsAI)
                Ai = new AI(AIdepth, Board.PlayerToPlay);
            if (Board.PlayerToPlay == 2)
            {
                Ai.EvaluateRandom(Board);
                this._firstThreeMoves += 1;
            }
        }

        public void TestingBoard()
        {
            Board.PlacePiece(new InversePawn(1), Board.Positions[0][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[1][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[2][-2]);
            Board.PlacePiece(new InversePawn(1), Board.Positions[3][-2]);
            Board.PlacePiece(new King(1), Board.Positions[0][-1]);
            Board.PlacePiece(new Elephant(1), Board.Positions[1][-1]);
            Board.PlacePiece(new Horse(1), Board.Positions[2][-1]);
            Board.PlacePiece(new Rook(1), Board.Positions[3][-1]);
            Board.PlacePiece(new Minister(1), Board.Positions[0][0]);
            Board.PlacePiece(new Elephant(2), Board.Positions[3][5]);
            Board.PlacePiece(new Horse(1), Board.Positions[2][5]);
            Board.PlacePiece(new Rook(1), Board.Positions[3][0]);
            Board.PlacePiece(new Pawn(1), Board.Positions[0][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[1][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[2][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[3][1]);

            Board.PlacePiece(new Pawn(2), Board.Positions[0][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[1][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[2][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[3][9]);
            Board.PlacePiece(new King(2), Board.Positions[0][8]);
            Board.PlacePiece(new Elephant(2), Board.Positions[1][8]);
            Board.PlacePiece(new Horse(2), Board.Positions[2][8]);
            Board.PlacePiece(new Rook(2), Board.Positions[3][8]);
            Board.PlacePiece(new Minister(2), Board.Positions[0][7]);
            Board.PlacePiece(new Horse(2), Board.Positions[2][7]);
            Board.PlacePiece(new Rook(2), Board.Positions[3][7]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[0][6]);
            Board.PlacePiece(new Pawn(1), Board.Positions[1][6]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[2][6]);
            Board.PlacePiece(new InversePawn(2), Board.Positions[3][6]);

            Board.KingPieces[1] = Board.Positions[0][-1];
            Board.KingPieces[2] = Board.Positions[0][8];
        }
    }
}

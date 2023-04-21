using System;
using System.Collections.Generic;
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
        private Position SelectedPosition
        {
            get => Board.SelectedPosition;
            set
            {
                Board.TryMovePiece(value);
                if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = SelectedPosition.Color;
                Board.SelectedPosition = value;
                if (SelectedPosition.Drawing != null) SelectedPosition.Drawing.Fill = Brushes.GreenYellow;

                Board.ShowPossibleMoves();
            }
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
            Board.PlacePiece(new Pawn(1), Board.Positions[0][-2]);
            Board.PlacePiece(new Pawn(1), Board.Positions[1][-2]);
            Board.PlacePiece(new Pawn(1), Board.Positions[2][-2]);
            Board.PlacePiece(new Pawn(1), Board.Positions[3][-2]);
            //Board.PlacePiece(new King(), Board.Positions[-1][0]);
            //Board.PlacePiece(new Elephant(), Board.Positions[-1][1]);
            //Board.PlacePiece(new Horse(), Board.Positions[-1][2]);
            //Board.PlacePiece(new Rook(), Board.Positions[-1][3]);
            //Board.PlacePiece(new Minister(), Board.Positions[0][0]);
            //Board.PlacePiece(new Elephant(), Board.Positions[0][1]);
            //Board.PlacePiece(new Horse(), Board.Positions[0][2]);
            //Board.PlacePiece(new Rook(), Board.Positions[0][3]);
            Board.PlacePiece(new Pawn(1), Board.Positions[0][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[1][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[2][1]);
            Board.PlacePiece(new Pawn(1), Board.Positions[3][1]);


            Board.PlacePiece(new Pawn(2), Board.Positions[0][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[1][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[2][9]);
            Board.PlacePiece(new Pawn(2), Board.Positions[3][9]);
            //Board.PlacePiece(new King(), Board.Positions[-1][0]);
            //Board.PlacePiece(new Elephant(), Board.Positions[-1][1]);
            //Board.PlacePiece(new Horse(), Board.Positions[-1][2]);
            //Board.PlacePiece(new Rook(), Board.Positions[-1][3]);
            //Board.PlacePiece(new Minister(), Board.Positions[0][0]);
            //Board.PlacePiece(new Elephant(), Board.Positions[0][1]);
            //Board.PlacePiece(new Horse(), Board.Positions[0][2]);
            //Board.PlacePiece(new Rook(), Board.Positions[0][3]);
            Board.PlacePiece(new Pawn(2), Board.Positions[0][6]);
            Board.PlacePiece(new Pawn(2), Board.Positions[1][6]);
            Board.PlacePiece(new Pawn(2), Board.Positions[2][6]);
            Board.PlacePiece(new Pawn(2), Board.Positions[3][6]);

        }
    }
}

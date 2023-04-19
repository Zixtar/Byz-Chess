using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Byz_Chess.Pieces;

namespace Byz_Chess.Resources
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoardView : UserControl
    {
        public ChessBoard Board;

        private Position SelectedPosition
        {
            get => Board.SelectedPosition;
            set
            {
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
            Board.PlacePiece(new Pawn(), SelectedPosition);
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
    }
}

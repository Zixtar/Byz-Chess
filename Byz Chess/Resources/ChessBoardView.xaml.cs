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
using System.Linq;

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
                SelectedPosition.Drawing.Fill = SelectedPosition.Color;
                Board.SelectedPosition = value;
                SelectedPosition.Drawing.Fill = Brushes.GreenYellow;
            }
        }

        public ChessBoardView()
        {
            InitializeComponent();
            var positionPaths = board.Children.OfType<Path>().Where(x => x.Tag?.ToString() == "Position");
            SortedSet<Path> sortedPaths = new SortedSet<Path>(positionPaths, new PathComparer<Path>());
            Board = new ChessBoard(sortedPaths, 4);
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Path)sender;

            var row = cell.Name.First() - 'a';
            var col = Convert.ToInt32(cell.Name.Substring(1)) - 1;
            SelectedPosition = Board.Positions[row][col];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private class PathComparer<T> : IComparer<Path>
        {
            public int Compare(Path? x, Path? y) //horrible horrible comparer
            {
                var Name1 = x.Name;
                var Name2 = y.Name;
                if (Name1.Length != y.Name.Length && Name1.First() == Name2.First()) return Math.Sign((Name1.Length - Name2.Length));
                return String.Compare(Name1, Name2);
            }
        }
    }
}

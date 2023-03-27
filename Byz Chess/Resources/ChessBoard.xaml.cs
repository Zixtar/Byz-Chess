using System;
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

namespace Byz_Chess.Resources
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoard : UserControl
    {
        public ChessBoard()
        {
            InitializeComponent();
           
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Path)sender;
            var row = cell.Name.Substring(0,1);
            var col = cell.Name.Substring(1);

            MessageBox.Show(cell.Name);
        }

        Path cell = null;
        int number = 97;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const string root = "Path_";
            if(cell != null)
            {
                if(cell.Fill != null)
                    cell.Fill = Brushes.Blue;
            }
            cell = (Path)bitmap_0.FindName(root + number);
            number--;
            if (cell?.Fill == null)
            {
                MessageBox.Show("jumped 1");
                Button_Click(new object(), new RoutedEventArgs());
            }
            cell.Fill = Brushes.Red;
        }
    }
}

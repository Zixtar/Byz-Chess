using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Helpers;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Byz_Chess
{
    public class ChessBoard
    {
        public CircularList<List<Position>> Positions;

        public Position SelectedPosition;

        private Brush color1 = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FF502D16"));
        private Brush color2 = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFD38D5F"));
        private Brush[] Colors;
        public ChessBoard(IEnumerable<Path> positions, int height)
        {
            Colors = new Brush[]
            {
                color1, color2
            };
            SelectedPosition = Position.Empty;
            Positions = new CircularList<List<Position>>();
            InitChessBoard(positions, height);
        }

        private void InitChessBoard(IEnumerable<Path> positions, int height)
        {
            var paths = positions as Path[] ?? positions.ToArray();
            if (paths.Length % height != 0)
            {
                MessageBox.Show("Board size is invalid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var width = paths.Length / height;
            var i = 0;
            var tempList = new List<Position>();
            foreach (var position in paths)
            {

                tempList.Add(new Position()
                {
                    Drawing = position,
                    Color = Colors[(i + Positions.Count) % 2]
                });
                i++;
                if (i % width != 0) continue;
                Positions.Add(tempList);
                tempList = new List<Position>();
                i = 0;
            }
        }
    }
}

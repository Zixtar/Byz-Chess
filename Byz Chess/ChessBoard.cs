using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Byz_Chess.Pieces;
using Helpers;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Byz_Chess
{
    public class ChessBoard
    {
        public CircularList<List<Position>> Positions;

        public Position SelectedPosition;

        public readonly Brush Color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF502D16"));
        public readonly Brush Color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD38D5F"));
        private readonly Brush[] _colors;
        public ChessBoard(IEnumerable<Path> positions, int height)
        {
            _colors = new Brush[]
            {
                Color1, Color2
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
                    Color = _colors[(i + Positions.Count) % 2]
                });
                i++;
                if (i % width != 0) continue;
                Positions.Add(tempList);
                tempList = new List<Position>();
                i = 0;
            }
        }

        public void PlacePiece(IPiece piece, Position position)
        {
            position.Piece = piece;
        }

        public void ShowPossibleMoves()
        {
            var pieceMoves = SelectedPosition.GetMoves();
        }
    }
}

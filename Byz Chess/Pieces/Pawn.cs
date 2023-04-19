using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Byz_Chess.Pieces
{
    internal class Pawn : IPiece
    {
        private readonly List<Offset> _initialMoves = new()
        {
            new Offset(0,1),
            new Offset(0,2),
        };
        private readonly List<Offset> _moves = new()
        {
            new Offset(0,1)
        };

        private bool _moved = false;

        public Pawn()
        {
            Color = Brushes.DarkRed;
            DrawingImage = Globals.PiecesDictionary["WPawn"] as BitmapImage;
        }

            public Brush Color { get; set; }
        public BitmapImage? DrawingImage { get; }
        public List<Offset> GetMoves()
        {
            if (!_moved) return _initialMoves;
            return _moves;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        }
        public Brush Color { get; set; }
        public DrawingBrush DrawingImage { get; }
        public List<Offset> GetMoves()
        {
            if(!_moved) return _initialMoves;
            return _moves;
        }
    }
}

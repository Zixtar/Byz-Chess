using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Byz_Chess.Pieces
{
    public interface IPiece
    {
        public Brush Color { get; set; }
        DrawingBrush DrawingImage { get; }
        List<Offset> GetMoves();
    }
}

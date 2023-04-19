using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Byz_Chess.Pieces
{
    public interface IPiece
    {
        public Brush Color { get; set; }
        BitmapImage? DrawingImage { get; }
        List<Offset> GetMoves();
    }
}

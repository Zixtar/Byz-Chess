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
        public bool SideConscious { get; }
        public bool Grounded { get; }
        public int Team { get; }
        BitmapImage? DrawingImage { get; }
        List<Offset> GetMoves();
    }
}

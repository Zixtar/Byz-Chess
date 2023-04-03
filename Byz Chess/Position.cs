using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Byz_Chess
{
    public class Position
    {
        public static Position Empty => new Position();
        public Brush? Color;
        public Path Drawing = new Path();
        public string Name => Drawing.Name;
        public IPiece? Piece;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Byz_Chess.Pieces;

namespace Byz_Chess
{
    public class Position
    {
        public static Position Empty => new Position();
        public Brush? Color;
        public Resources.PositionUC? Drawing;
        public string Name => Drawing.Name;

        private IPiece _piece;

        public IPiece? Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                Color = Piece.Color;
            }
        }

        public IEnumerable<Offset> GetMoves()
        {
            if(Piece == null) return Enumerable.Empty<Offset>();
            return Piece.GetMoves();
        }
    }
}

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
        private string Name => Drawing.Name;
        public int Row => Name.First() - 'A';
        public bool MoveShadow
        {
            get => Drawing != null && Drawing.MoveShadow;
            set
            {
                if (Drawing != null) Drawing.MoveShadow = value;
            }
        }

        public int Column => Convert.ToInt32(Name[1..]) - 1;

        private IPiece _piece;

        public IPiece? Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                Drawing.Piece = Piece != null ? Piece.DrawingImage : null;
            }
        }

        public IEnumerable<Offset> GetMoves()
        {
            if (Piece == null) return Enumerable.Empty<Offset>();
            return Piece.GetMoves();
        }
    }
}

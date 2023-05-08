using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Byz_Chess.Pieces
{
    public class King : IPiece
    {
        public readonly List<Offset> _moves = Globals.KingMoves.MovesList;
        public bool SideConscious => false;
        public bool Grounded => false;
        public int Team { get; }
        public BitmapImage? DrawingImage { get; }
        public King(int team)
        {
            Team = team;
            DrawingImage = team switch
            {
                1 => Globals.PiecesDictionary["WKing"] as BitmapImage,
                2 => Globals.PiecesDictionary["BKing"] as BitmapImage,
                _ => DrawingImage
            };
        }
        public List<Offset> GetMoves()
        {
            return _moves;
        }
    }
}

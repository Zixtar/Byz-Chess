using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Byz_Chess.Pieces
{
    class Rook : IPiece
    {
        public readonly List<Offset> _moves = Globals.RookMoves.MovesList;
        public bool SideConscious => false;
        public Brush Color { get; set; }
        public bool Grounded => true;
        public int Team { get; }
        public BitmapImage? DrawingImage { get; }
        public Rook(int team)
        {
            Team = team;
            DrawingImage = team switch
            {
                1 => Globals.PiecesDictionary["WRook"] as BitmapImage,
                2 => Globals.PiecesDictionary["BRook"] as BitmapImage,
                _ => DrawingImage
            };
        }
        public List<Offset> GetMoves()
        {
            return _moves;
        }
    }
}

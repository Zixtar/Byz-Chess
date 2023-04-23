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
        private readonly List<Offset> _moves = new()
        {
            new Offset(0,1),
            new Offset(0,2),
            new Offset(0,3),
            new Offset(0,4),
            new Offset(0,5),
            new Offset(0,6),
            new Offset(0,7),
            new Offset(0,8),
            new Offset(0,9),
            new Offset(0,10),
            new Offset(0,11),
            new Offset(0,12),
            new Offset(0,13),
            new Offset(0,14),
            new Offset(0,15),
            new Offset(1,0),
            new Offset(2,0),
            new Offset(3, 0),
            new Offset(-1, 0),
            new Offset(-2, 0),
            new Offset(-3, 0)
        };
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Byz_Chess.Pieces
{
    class Elephant : IPiece
    {
        public static readonly List<Offset> _moves = Globals.ElephantMoves.MovesList;

        public bool SideConscious => false;
        public bool Grounded => false;
        public int Team { get; }
        public BitmapImage? DrawingImage { get; }
        public Elephant(int team)
        {
            Team = team;
            DrawingImage = team switch
            {
                1 => Globals.PiecesDictionary["WBishop"] as BitmapImage,
                2 => Globals.PiecesDictionary["BBishop"] as BitmapImage,
                _ => DrawingImage
            };
        }
        public List<Offset> GetMoves()
        {
            return _moves;
        }
    }
}

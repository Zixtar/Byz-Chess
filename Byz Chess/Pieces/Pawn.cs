using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Byz_Chess.Pieces
{
    internal class Pawn : IPiece
    {
        public readonly List<Offset> _moves = Globals.PawnMoves.MovesList;
        
        public bool Grounded => false;
        public int Team { get; }

        public Pawn(int team)
        {
            Team = team;
            DrawingImage = team switch
            {
                1 => Globals.PiecesDictionary["WPawn"] as BitmapImage,
                2 => Globals.PiecesDictionary["BPawn"] as BitmapImage,
                _ => DrawingImage
            };
        }

        public bool SideConscious => true;
        public BitmapImage? DrawingImage { get; }
        public List<Offset> GetMoves()
        {
            return _moves;
        }
    }
}

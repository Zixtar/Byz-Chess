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
        private readonly List<Offset> _initialMoves = new()
        {
            new Offset(0,1),
            new Offset(0,2),
            new Offset(1,1, true),
            new Offset(-1,1, true),
        };
        private readonly List<Offset> _moves = new()
        {
            new Offset(0,1),
            new Offset(1,1, true),
            new Offset(-1,1, true),
        };

        private bool _moved = false;

        public void Moved()
        {
            _moved = true;
        }

        public bool Grounded => true;
        public int Team { get; }

        public Pawn(int team)
        {
            Color = Brushes.DarkRed;
            Team = team;
            DrawingImage = team switch
            {
                1 => Globals.PiecesDictionary["WPawn"] as BitmapImage,
                2 => Globals.PiecesDictionary["BPawn"] as BitmapImage,
                _ => DrawingImage
            };
        }

        public bool SideConscious => true;
        public Brush Color { get; set; }
        public BitmapImage? DrawingImage { get; }
        public List<Offset> GetMoves()
        {
            if (!_moved) return _initialMoves;
            return _moves;
        }
    }
}

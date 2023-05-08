using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Byz_Chess.Pieces;

namespace Byz_Chess
{
    public static class Globals
    {
        private static ResourceDictionary _piecesDictionary;
        public static ResourceDictionary PiecesDictionary
        {
            get
            {
                if (_piecesDictionary == null || _piecesDictionary.Source == null)
                {
                    var myResourceDictionary = new ResourceDictionary();
                    myResourceDictionary.Source =
                        new Uri("/Byz Chess;component/Resources/Pieces.xaml",
                            UriKind.RelativeOrAbsolute);
                    PiecesDictionary = myResourceDictionary;
                }

                return _piecesDictionary;
            }
            set => _piecesDictionary = value;
        }

        public static readonly Moves RookMoves = new Moves
        {
            ClassType = typeof(Rook),
            MovesList = new List<Offset>
            {
                new Offset(0, 1),
                new Offset(0, 2),
                new Offset(0, 3),
                new Offset(0, 4),
                new Offset(0, 5),
                new Offset(0, 6),
                new Offset(0, 7),
                new Offset(0, 8),
                new Offset(0, 9),
                new Offset(0, 10),
                new Offset(0, 11),
                new Offset(0, 12),
                new Offset(0, 13),
                new Offset(0, 14),
                new Offset(0, 15),
                new Offset(1, 0),
                new Offset(2, 0),
                new Offset(3, 0),
                new Offset(-1, 0),
                new Offset(-2, 0),
                new Offset(-3, 0)
            }
        };
        public static readonly Moves PawnMoves = new Moves
        {
            ClassType = typeof(Pawn),
            MovesList = new List<Offset>
            {
                new Offset(0,1),
                new Offset(1,1, true),
                new Offset(-1,1, true),
            }
        };
        public static readonly Moves MinisterMoves = new Moves
        {
            ClassType = typeof(Minister),
            MovesList = new List<Offset>
            {
                new Offset(1,1),
                new Offset(1,-1),
                new Offset(-1,-1),
                new Offset(-1,1),
            }
        };
        public static readonly Moves KingMoves = new Moves
        {
            ClassType = typeof(King),
            MovesList = new List<Offset>
            {
                new Offset(0,1),
                new Offset(0,-1),
                new Offset(1,1),
                new Offset(1,0),
                new Offset(1,-1),
                new Offset(-1,-1),
                new Offset(-1,0),
                new Offset(-1,1),
            }
        };
        public static readonly Moves HorseMoves = new Moves
        {
            ClassType = typeof(Horse),
            MovesList = new List<Offset>
            {
                new Offset(2,1),
                new Offset(1,2),
                new Offset(2,-1),
                new Offset(-1,2),
                new Offset(-2,1),
                new Offset(1,-2),
                new Offset(-2,-1),
                new Offset(-1,-2),
            }
        };
        public static readonly Moves ElephantMoves = new Moves
        {
            ClassType = typeof(Elephant),
            MovesList = new List<Offset>
            {
                new Offset(2, 2),
                new Offset(2, -2),
                new Offset(-2, 2),
                new Offset(-2, -2),

            }
        };

        public static List<Moves> AllPossibleMoves = new List<Moves> { RookMoves, PawnMoves, MinisterMoves, KingMoves, HorseMoves, ElephantMoves };


        public class Moves
        {
            public List<Offset> MovesList;
            public Type ClassType;
        }
    }
}

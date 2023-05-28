using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byz_Chess.Pieces
{
    public struct Offset
    {
        public int row;
        public int col;
        public bool taking = false;

        public Offset(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public Offset(int row, int col, bool taking)
        {
            this.row = row;
            this.col = col;
            this.taking = taking;
        }
    }

    public struct Move
    {
        public Position OldPosition;
        public Position NewPosition;

        public Move(Position oldPos, Position newPos)
        {
            OldPosition = oldPos;
            NewPosition = newPos;
        }
    }
}

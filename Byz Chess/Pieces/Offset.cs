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

        public Offset(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}

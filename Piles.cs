using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Nim
{
    class Piles
    {
        private int pieces;


        public Piles(int total)
        {
            this.pieces = total;
        }   


        // Accessor

        public int GetPieces()
        {
            return this.pieces;
        }


        // Methods


        public void UpdatePieces(int pieceInput)
        {
            int temp = GetPieces() - pieceInput;

            this.pieces = temp;
        }
    }
}

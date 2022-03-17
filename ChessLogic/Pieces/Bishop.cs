using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    class Bishop : Piece
    {
        public Bishop(string Color)
        {
            NotationName = "B";
            Name = "Bishop";
            this.Color = Color;
        }
        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public List<int[]> availableMoves(int x, int y, Board.Board board)
        {
            List<int[]> availableMoves = new List<int[]>();

            return availableMoves;
        }
    }
}

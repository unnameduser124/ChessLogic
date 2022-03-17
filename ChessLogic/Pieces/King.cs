using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    class King : Piece
    {
        public King(string Color)
        {
            NotationName = "K";
            Name = "King";
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

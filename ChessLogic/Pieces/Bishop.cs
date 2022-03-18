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

        public List<int[]> availableMoves(Board.Board board)
        {
            int x = -1, y = -1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.ChessBoard[i, j] == this)
                    {
                        x = i;
                        y = j;
                    }
                }
            }

            List<int[]> availableMoves = new List<int[]>();

            return availableMoves;
        }
    }
}

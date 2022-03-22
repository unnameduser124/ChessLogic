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

            if (x != -1)
            {
                if (x + 1 < 8)
                {
                    if (board.ChessBoard[x + 1, y] == null)
                    {
                        availableMoves.Add(new int[] { x + 1, y });
                    }
                    else if (board.ChessBoard[x + 1, y].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 1, y });
                    }
                }
                if (x - 1 >= 0)
                {
                    if (board.ChessBoard[x - 1, y] == null)
                    {
                        availableMoves.Add(new int[] { x - 1, y });
                    }
                    else if (board.ChessBoard[x - 1, y].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 1, y });
                    }
                }
                if (y + 1 < 8)
                {
                    if (board.ChessBoard[x, y + 1] == null)
                    {
                        availableMoves.Add(new int[] { x, y + 1 });
                    }
                    else if (board.ChessBoard[x, y + 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x, y + 1 });
                    }
                }
                if (y - 1 >= 0)
                {
                    if (board.ChessBoard[x, y - 1] == null)
                    {
                        availableMoves.Add(new int[] { x, y - 1 });
                    }
                    else if (board.ChessBoard[x, y - 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x, y - 1 });
                    }
                }

                if(x + 1 < 8 && y + 1 < 8)
                {
                    if (board.ChessBoard[x + 1, y + 1] == null)
                    {
                        availableMoves.Add(new int[] { x + 1, y + 1 });
                    }
                    else if (board.ChessBoard[x + 1, y + 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 1, y + 1 });
                    }
                }
                if(x + 1 < 8 && y - 1 >= 0)
                {
                    if (board.ChessBoard[x + 1, y - 1] == null)
                    {
                        availableMoves.Add(new int[] { x + 1, y - 1 });
                    }
                    else if (board.ChessBoard[x + 1, y - 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 1, y - 1 });
                    }
                }
                if(x - 1 >= 0 && y + 1 < 8)
                {
                    if (board.ChessBoard[x - 1, y + 1] == null)
                    {
                        availableMoves.Add(new int[] { x - 1, y + 1 });
                    }
                    else if (board.ChessBoard[x - 1, y + 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 1, y + 1 });
                    }
                }
                if(x - 1 >= 0 && y - 1 >= 0)
                {
                    if (board.ChessBoard[x - 1, y - 1] == null)
                    {
                        availableMoves.Add(new int[] { x - 1, y - 1 });
                    }
                    else if (board.ChessBoard[x - 1, y - 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 1, y - 1 });
                    }
                }
            }

            return availableMoves;
        }

        public Piece copy()
        {
            return new King(Color);
        }
    }
}

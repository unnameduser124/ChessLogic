using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    public class Pawn: Piece
    {
        public Pawn(string Color)
        {
            NotationName = "";
            Name = "Pawn";
            this.Color = Color;
        }

        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public List<int[]> availableMoves(Board.Board board)
        {
            int x = -1, y = -1;
            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    if(board.ChessBoard[i, j] == this)
                    {
                        x = i;
                        y = j;
                    }
                }
            }

            List<int[]> availableMoves = new List<int[]>();

            if (x != -1)
            {
                if (Color.ToLower() == "white")
                {
                    if (y + 1 < 8)
                    {
                        if (board.ChessBoard[x, y + 1] == null)
                        {
                            availableMoves.Add(new int[] { x, y + 1 });
                        }
                    }
                    if (y == 1)
                    {
                        if (board.ChessBoard[x, y + 2] == null && board.ChessBoard[x, y + 1] == null)
                        {
                            availableMoves.Add(new int[] { x, y + 2 });
                        }
                    }
                    if (y + 1 < 8 && x + 1 < 8)
                    {
                        if (board.ChessBoard[x + 1, y + 1] != null)
                        {
                            if (board.ChessBoard[x + 1, y + 1].Color.ToLower() != Color.ToLower())
                            {
                                availableMoves.Add(new int[] { x + 1, y + 1 });
                            }
                        }
                    }
                    if (y + 1 < 8 && x - 1 >= 0)
                    {
                        if (board.ChessBoard[x - 1, y + 1] != null)
                        {
                            if (board.ChessBoard[x - 1, y + 1].Color.ToLower() != Color.ToLower())
                            {
                                availableMoves.Add(new int[] { x - 1, y + 1 });
                            }
                        }
                    }
                }
                else
                {
                    if (y - 1 >= 0)
                    {
                        if (board.ChessBoard[x, y - 1] == null)
                        {
                            availableMoves.Add(new int[] { x, y - 1 });
                        }
                    }
                    if (y == 6)
                    {
                        if (board.ChessBoard[x, y - 2] == null && board.ChessBoard[x, y - 1] == null)
                        {
                            availableMoves.Add(new int[] { x, y - 2 });
                        }
                    }
                    if (y - 1 >= 0 && x + 1 < 8)
                    {
                        if (board.ChessBoard[x + 1, y - 1] != null)
                        {
                            if (board.ChessBoard[x + 1, y - 1].Color.ToLower() != Color.ToLower())
                            {
                                availableMoves.Add(new int[] { x + 1, y - 1 });
                            }
                        }
                    }
                    if (y - 1 >= 0 && x - 1 >= 0)
                    {
                        if (board.ChessBoard[x - 1, y - 1] != null)
                        {
                            if (board.ChessBoard[x - 1, y - 1].Color.ToLower() != Color.ToLower())
                            {
                                availableMoves.Add(new int[] { x - 1, y - 1 });
                            }
                        }
                    }
                }
            }
            return availableMoves;
        }
    }
}

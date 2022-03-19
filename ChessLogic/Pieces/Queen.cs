using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    class Queen : Piece
    {
        public Queen(string Color)
        {
            NotationName = "Q";
            Name = "Queen";
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
                //moves horizontally incrementing x
                for (int i = 1; i < 8; i++)
                {
                    if (x + i < 8)
                    {
                        if (board.ChessBoard[x + i, y] == null)
                        {
                            availableMoves.Add(new int[] { x + i, y });
                        }
                        else if (board.ChessBoard[x + i, y].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x + i, y });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //moves horizontally decrementing x
                for (int i = 1; i < 8; i++)
                {
                    if (x - i >= 0)
                    {
                        if (board.ChessBoard[x - i, y] == null)
                        {
                            availableMoves.Add(new int[] { x - i, y });
                        }
                        else if (board.ChessBoard[x - i, y].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x - i, y });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //moves vertically incrementing y
                for (int i = 1; i < 8; i++)
                {
                    if (y + i < 8)
                    {
                        if (board.ChessBoard[x, y + i] == null)
                        {
                            availableMoves.Add(new int[] { x, y + i });
                        }
                        else if (board.ChessBoard[x, y + i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x, y + i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //moves vertically decrementing y
                for (int i = 1; i < 8; i++)
                {
                    if (y - i >= 0)
                    {
                        if (board.ChessBoard[x, y - i] == null)
                        {
                            availableMoves.Add(new int[] { x, y - i });
                        }
                        else if (board.ChessBoard[x, y - i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x, y - i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //moves incrementing x and y
                for (int i = 1; i < 8; i++)
                {
                    if (x + i < 8 && y + i < 8)
                    {
                        if (board.ChessBoard[x + i, y + i] == null)
                        {
                            availableMoves.Add(new int[] { x + i, y + i });
                        }
                        else if (board.ChessBoard[x + i, y + i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x + i, y + i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //moves decrementing x and y
                for (int i = 1; i < 8; i++)
                {
                    if (x - i >= 0 && y - i >= 0)
                    {
                        if (board.ChessBoard[x - i, y - i] == null)
                        {
                            availableMoves.Add(new int[] { x - i, y - i });
                        }
                        else if (board.ChessBoard[x - i, y - i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x - i, y - i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //moves decrementing x and incrementing y
                for (int i = 1; i < 8; i++)
                {
                    if (x - i >= 0 && y + i < 8)
                    {
                        if (board.ChessBoard[x - i, y + i] == null)
                        {
                            availableMoves.Add(new int[] { x - i, y + i });
                        }
                        else if (board.ChessBoard[x - i, y + i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x - i, y + i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //moves incrementing x and decrementing y
                for (int i = 1; i < 8; i++)
                {
                    if (x + i < 8 && y - i >= 0)
                    {
                        if (board.ChessBoard[x + i, y - i] == null)
                        {
                            availableMoves.Add(new int[] { x + i, y - i });
                        }
                        else if (board.ChessBoard[x + i, y - i].Color.ToLower() != Color.ToLower())
                        {
                            availableMoves.Add(new int[] { x + i, y - i });
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return availableMoves;
        }
    }
}

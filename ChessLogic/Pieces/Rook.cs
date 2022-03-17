using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    class Rook : Piece
    {
        public Rook(string Color)
        {
            NotationName = "R";
            Name = "Rook";
            this.Color = Color;
        }
        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public List<int []> availableMoves(int x, int y, Board.Board board)
        {
            List<int []> availableMoves = new List<int []>();

            //moves horizontally incrementing x
            for(int i=1; i<8; i++)
            {
                if(x + i < 8)
                {
                    if(board.ChessBoard[x + i, y] == null)
                    {
                        availableMoves.Add(new int[] { x + i, y });
                    }
                    else if(board.ChessBoard[x + i, y].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + i, y });
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
                if (y - i > 0)
                {
                    if (board.ChessBoard[x, y - i] == null)
                    {
                        availableMoves.Add(new int[] { x, y - i });
                    }
                    else if (board.ChessBoard[x, y - i].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x, y - i });
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return availableMoves;
        }
    }
}

using System.Collections.Generic;

namespace ChessLogic.Pieces
{
    class Knight : Piece
    {
        public Knight(string Color)
        {
            NotationName = "N";
            Name = "Knight";
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
                //how does the knight move?

                if (x + 2 < 8 && y + 1 < 8)
                {
                    if (board.ChessBoard[x + 2, y + 1] == null)
                    {
                        availableMoves.Add(new int[] { x + 2, y + 1 });
                    }
                    else if (board.ChessBoard[x + 2, y + 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 2, y + 1 });
                    }
                }

                if (x + 2 < 8 && y - 1 >= 0)
                {
                    if (board.ChessBoard[x + 2, y - 1] == null)
                    {
                        availableMoves.Add(new int[] { x + 2, y - 1 });
                    }
                    else if (board.ChessBoard[x + 2, y - 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 2, y - 1 });
                    }
                }

                if (x + 1 < 8 && y + 2 < 8)
                {
                    if (board.ChessBoard[x + 1, y + 2] == null)
                    {
                        availableMoves.Add(new int[] { x + 1, y + 2 });
                    }
                    else if (board.ChessBoard[x + 1, y + 2].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 1, y + 2 });
                    }
                }

                if (x + 1 < 8 && y - 2 >= 0)
                {
                    if (board.ChessBoard[x + 1, y - 2] == null)
                    {
                        availableMoves.Add(new int[] { x + 1, y - 2 });
                    }
                    else if (board.ChessBoard[x + 1, y - 2].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x + 1, y - 2 });
                    }
                }

                if (x - 1 >= 0 && y + 2 < 8)
                {
                    if (board.ChessBoard[x - 1, y + 2] == null)
                    {
                        availableMoves.Add(new int[] { x - 1, y + 2 });
                    }
                    else if (board.ChessBoard[x - 1, y + 2].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 1, y + 2 });
                    }
                }

                if (x - 1 >= 0 && y - 2 >= 0)
                {
                    if (board.ChessBoard[x - 1, y - 2] == null)
                    {
                        availableMoves.Add(new int[] { x - 1, y - 2 });
                    }
                    else if (board.ChessBoard[x - 1, y - 2].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 1, y - 2 });
                    }
                }

                if (x - 2 >= 0 && y + 1 < 8)
                {
                    if (board.ChessBoard[x - 2, y + 1] == null)
                    {
                        availableMoves.Add(new int[] { x - 2, y + 1 });
                    }
                    else if (board.ChessBoard[x - 2, y + 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 2, y + 1 });
                    }
                }

                if (x - 2 >= 0 && y - 1 >= 0)
                {
                    if (board.ChessBoard[x - 2, y - 1] == null)
                    {
                        availableMoves.Add(new int[] { x - 2, y - 1 });
                    }
                    else if (board.ChessBoard[x - 2, y - 1].Color.ToLower() != Color.ToLower())
                    {
                        availableMoves.Add(new int[] { x - 2, y - 1 });
                    }
                }
            }

            return availableMoves;
        }

        public Piece copy()
        {
            return new Knight(Color);
        }
    }
}

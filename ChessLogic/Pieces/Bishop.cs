using System.Collections.Generic;

namespace ChessLogic.Pieces
{
    class Bishop : Piece
    {
        public Bishop(string Color)
        {
            NotationName = "B";
            FENsymbol = "b";
            Name = "bishop";
            this.Color = Color;
            x = -1;
            y = -1;
        }
        public Bishop(string Color, int x, int y)
        {
            NotationName = "B";
            FENsymbol = "b";
            Name = "bishop";
            this.Color = Color;
            this.x = x;
            this.y = y;
        }
        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string FENsymbol { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public List<int[]> availableMoves(Board.Game board)
        {
            /*int x = -1, y = -1;
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
            }*/

            List<int[]> availableMoves = new List<int[]>();
            if (x != -1)
            {
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

        public Piece copy()
        {
            return new Bishop(Color, x, y);
        }
    }
}

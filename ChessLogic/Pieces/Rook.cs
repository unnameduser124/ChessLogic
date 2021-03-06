using System.Collections.Generic;

namespace ChessLogic.Pieces
{
    class Rook : Piece
    {
        public Rook(string Color)
        {
            NotationName = "R";
            FENsymbol = "r";
            Name = "rook";
            this.Color = Color;
            x = -1;
            y = -1;
        }

        public Rook(string Color, int x, int y)
        {
            NotationName = "R";
            FENsymbol = "r";
            Name = "rook";
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
            }

            return availableMoves;
        }

        public Piece copy()
        {
            return new Rook(Color, x, y);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace ChessLogic.Pieces
{
    public class King : Piece
    {
        public King(string Color)
        {
            NotationName = "K";
            FENsymbol = "k";
            Name = "king";
            CastlingLong = true;
            CastlingShort = true;
            this.Color = Color;
            x = -1;
            y = -1;
        }
        public King(string Color, int x, int y)
        {
            NotationName = "K";
            FENsymbol = "k";
            Name = "king";
            CastlingLong = true;
            CastlingShort = true;
            this.Color = Color;
            this.x = x;
            this.y = y;
        }
        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool CastlingLong { get; set; }
        public bool CastlingShort { get; set; }
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

                if (x + 1 < 8 && y + 1 < 8)
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
                if (x + 1 < 8 && y - 1 >= 0)
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
                if (x - 1 >= 0 && y + 1 < 8)
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
                if (x - 1 >= 0 && y - 1 >= 0)
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

            if (x == 4)
            {
                if (CastlingLong)
                {
                    if (y == 7 || y == 0)
                    {
                        if (board.ChessBoard[x - 1, y] == null && board.ChessBoard[x - 2, y] == null && board.ChessBoard[x - 3, y] == null)
                        {
                            if (!(availableMovesContains(new int[] { x - 1, y }, board, x, y) || availableMovesContains(new int[] { x - 2, y }, board, x, y)))
                            {
                                availableMoves.Add(new int[] { 2, y });
                            }
                        }
                    }

                }
                if (CastlingShort)
                {
                    if (y == 7 || y == 0)
                    {
                        if (board.ChessBoard[x + 1, y] == null && board.ChessBoard[x + 2, y] == null)
                        {
                            if (!(availableMovesContains(new int[] { x + 1, y }, board, x, y) || availableMovesContains(new int[] { x + 2, y }, board, x, y)))
                            {
                                availableMoves.Add(new int[] { 6, y });
                            }
                        }
                    }

                }
            }

            return availableMoves;
        }

        public bool availableMovesContains(int[] position, Board.Game board, int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.ChessBoard[i, j] != null)
                    {
                        if (board.ChessBoard[i, j].Name != Name && board.ChessBoard[i, j].Color.ToLower() != Color.ToLower())
                        {
                            foreach (var move in board.ChessBoard[i, j].availableMoves(board))
                            {
                                if (move.SequenceEqual(position))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public Piece copy()
        {
            return new King(Color, x, y);
        }
    }
}

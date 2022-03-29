using System;
using System.Collections.Generic;

namespace ChessLogic.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(string Color)
        {
            NotationName = "";
            FENsymbol = "p";
            Name = "Pawn";
            this.Color = Color;
        }

        public string NotationName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string FENsymbol { get; set; }

        public bool EnPassant { get; set; }

        public List<int[]> availableMoves(Board.Game board)
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
                    if (x - 1 >= 0 && y + 1 < 8)
                    {
                        if (board.ChessBoard[x - 1, y] != null && board.ChessBoard[x - 1, y + 1] == null)
                        {
                            if (board.ChessBoard[x - 1, y].Color.ToLower() == "black" && board.ChessBoard[x - 1, y].Name.ToLower() == "pawn")
                            {
                                if ((board.ChessBoard[x - 1, y] as Pawn).EnPassant)
                                {
                                    availableMoves.Add(new int[] { x - 1, y + 1 });
                                }
                            }
                        }
                    }
                    if (x + 1 < 8 && y + 1 < 8)
                    {
                        if (board.ChessBoard[x + 1, y] != null && board.ChessBoard[x + 1, y + 1] == null)
                        {
                            if (board.ChessBoard[x + 1, y].Color.ToLower() == "black" && board.ChessBoard[x + 1, y].Name.ToLower() == "pawn")
                            {
                                if ((board.ChessBoard[x + 1, y] as Pawn).EnPassant)
                                {
                                    availableMoves.Add(new int[] { x + 1, y + 1 });
                                }
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
                    if (x - 1 >= 0 && y - 1 < 8)
                    {
                        if (board.ChessBoard[x - 1, y] != null && board.ChessBoard[x - 1, y + 1] == null)
                        {
                            if (board.ChessBoard[x - 1, y].Color.ToLower() == "white" && board.ChessBoard[x - 1, y].Name.ToLower() == "pawn")
                            {
                                if ((board.ChessBoard[x - 1, y] as Pawn).EnPassant)
                                {
                                    availableMoves.Add(new int[] { x - 1, y + 1 });
                                }
                            }
                        }
                    }
                    if (x + 1 < 8 && y - 1 >=0)
                    {
                        if (board.ChessBoard[x + 1, y] != null && board.ChessBoard[x + 1, y - 1] == null)
                        {
                            if (board.ChessBoard[x + 1, y].Color.ToLower() == "white" && board.ChessBoard[x + 1, y].Name.ToLower() == "pawn")
                            {
                                if ((board.ChessBoard[x + 1, y] as Pawn).EnPassant)
                                {
                                    availableMoves.Add(new int[] { x + 1, y - 1 });
                                }
                            }
                        }
                    }

                }
                
            }
            return availableMoves;
        }

        public Piece copy()
        {
            var newPawn = new Pawn(Color);
            newPawn.EnPassant = EnPassant;
            return newPawn;
        }
    }
}

using ChessLogic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Board
{
    public class Board
    {
        public Board()
        {
            startingPosition();
        }

        public Board(string position)
        {
            ChessBoard = new Piece[8, 8];
            MoveHistory = new List<Move>();
            if (position.ToLower() == "test")
            {
                ChessBoard[0, 0] = new King("White");
                ChessBoard[7, 0] = new King("Black");
                ChessBoard[1, 0] = new Rook("White");
                ChessBoard[6, 0] = new Rook("Black");
            }
        }
        public Piece[,] ChessBoard { get; set; }

        public List<Move> MoveHistory { get; private set; }

        public void startingPosition()
        {
            ChessBoard = new Piece[8, 8];
            for (int i=0; i<8; i++)
            {
                ChessBoard[i, 1] = new Pawn("white");
                ChessBoard[i, 6] = new Pawn("black");
            }

            ChessBoard[0, 0] = new Rook("White");
            ChessBoard[7, 0] = new Rook("White");
            ChessBoard[0, 7] = new Rook("Black");
            ChessBoard[7, 7] = new Rook("Black");

            ChessBoard[1, 0] = new Knight("White");
            ChessBoard[6, 0] = new Knight("White");
            ChessBoard[1, 7] = new Knight("Black");
            ChessBoard[6, 7] = new Knight("Black");

            ChessBoard[2, 0] = new Bishop("White");
            ChessBoard[5, 0] = new Bishop("White");
            ChessBoard[2, 7] = new Bishop("Black");
            ChessBoard[5, 7] = new Bishop("Black");

            ChessBoard[3, 0] = new Queen("White");
            ChessBoard[4, 0] = new King("White");
            ChessBoard[3, 7] = new Queen("White");
            ChessBoard[4, 7] = new King("White");
        }

        public bool movePiece(int fromX, int fromY, int toX, int toY)
        {
            Console.WriteLine($"from = {(char)(fromX+97)}{fromY+1} to = {(char)(toX + 97)}{toY+1}");

            if (fromX>=0 && fromX<8 && fromY>=0 && fromY<8 && toX>=0 && toX<8 && toY>=0 && toY < 8)
            {
                var piece = ChessBoard[fromX, fromY];
                var destination = ChessBoard[toX, toY];
                if (piece != null)
                {
                    foreach(var move in piece.availableMoves(this))
                    {
                        if(new int[] { toX, toY }.SequenceEqual(move))
                        {
                            if (destination == null || destination.Color.ToLower() != piece.Color.ToLower())
                            {
                                ChessBoard[toX, toY] = null;
                                if (destination != null)
                                {
                                    MoveHistory.Add(new Move(fromX, fromY, toX, toY, destination.copy()));
                                }
                                else
                                {
                                    MoveHistory.Add(new Move(fromX, fromY, toX, toY));
                                }
                                (ChessBoard[fromX, fromY], ChessBoard[toX, toY]) = (ChessBoard[toX, toY], ChessBoard[fromX, fromY]);
                                if (check() == ChessBoard[toX, toY].Color.ToLower())
                                {
                                    undoMove();
                                    return false;
                                }
                                return true;
                            }
                            break;
                        }
                    }
                }
            }

            return false;
        }

        public string check()
        {
            int[] whiteKingPosition = null;
            int[] blackKingPosition = null;

            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        if (ChessBoard[i, j].Name.ToLower() == "king")
                        {
                            if (ChessBoard[i, j].Color.ToLower() == "white")
                            {
                                whiteKingPosition = new int[] { i, j };
                            }
                            else
                            {
                                blackKingPosition = new int[] { i, j };
                            }
                        }

                        if (blackKingPosition != null && whiteKingPosition != null)
                        {
                            break;
                        }
                    }
                }
                if (blackKingPosition != null && whiteKingPosition != null)
                {
                    break;
                }
            }


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        foreach(var move in ChessBoard[i, j].availableMoves(this))
                        {
                            if((move[0]==whiteKingPosition[0] && move[1]==whiteKingPosition[1]))
                            {
                                return "white";
                            }
                            else if(move[0] == blackKingPosition[0] && move[1] == blackKingPosition[1]){
                                return "black";
                            }
                        }
                    }
                }
            }

            return "false";
        }

        public void undoMove()
        {
            (ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY], ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY]) = (ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY], ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY]);
            ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY] = MoveHistory.Last().pieceCaptured;
        }
    }
}

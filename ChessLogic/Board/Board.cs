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

        public Board(string piece)
        {
            ChessBoard = new Piece[8, 8];
            if (piece.ToLower() == "rook")
            {
                ChessBoard[0, 0] = new Rook("White");
                ChessBoard[7, 0] = new Rook("White");
                ChessBoard[0, 7] = new Rook("Black");
                ChessBoard[7, 7] = new Rook("Black");
            }
            else if(piece.ToLower() == "pawn")
            {
                for (int i = 0; i < 8; i++)
                {
                    ChessBoard[i, 1] = new Pawn("white");
                    ChessBoard[i, 6] = new Pawn("black");
                }

                ChessBoard[0, 2] = new Rook("Black");
                ChessBoard[7, 2] = new Rook("Black");
                ChessBoard[0, 5] = new Rook("White");
                ChessBoard[7, 5] = new Rook("White");
            }
            else if(piece.ToLower() == "bishop")
            {
                ChessBoard[2, 0] = new Bishop("White");
                ChessBoard[5, 0] = new Bishop("White");
                ChessBoard[5, 3] = new Bishop("Black");
                ChessBoard[3, 3] = new Bishop("Black");

                ChessBoard[1, 1] = new Pawn("White");
            }
            else if(piece.ToLower() == "queen")
            {
                ChessBoard[2, 0] = new Bishop("White");
                ChessBoard[5, 0] = new Bishop("White");
                ChessBoard[5, 3] = new Bishop("Black");
                ChessBoard[3, 3] = new Bishop("Black");
                ChessBoard[3, 1] = new Queen("White");

                ChessBoard[1, 1] = new Pawn("White");
            }
            else if(piece.ToLower() == "knight")
            {
                ChessBoard[3, 3] = new Knight("White");
                ChessBoard[0, 3] = new Knight("White");
                ChessBoard[7, 7] = new Knight("Black");
                ChessBoard[5, 6] = new Pawn("White");
                ChessBoard[6, 5] = new Pawn("Black");
            }
            else if(piece.ToLower() == "king")
            {
                ChessBoard[3, 3] = new King("White");
                ChessBoard[0, 0] = new King("White");
                ChessBoard[0, 3] = new King("White");

                ChessBoard[1, 4] = new Pawn("White");
                ChessBoard[1, 3] = new Pawn("Black");
            }
        }
        public Piece[,] ChessBoard { get; set; }

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
    }
}

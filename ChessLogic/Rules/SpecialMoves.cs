using ChessLogic.Board;
using ChessLogic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Rules
{
    public abstract class SpecialMoves
    {
        public static void EnPassantCheck(int fromX, int fromY, int toX, int toY, Game game)
        {
            if (game.ChessBoard[fromX, fromY].Name == "pawn")
            {
                //after a pawn moves two spaces up or down the board (depending on color) it's attribute "EnPassant" changes to true
                if (fromY + 2 == toY || fromY - 2 == toY)
                {
                    (game.ChessBoard[fromX, fromY] as Pawn).EnPassant = true;
                }

                if (toX - 1 == fromX || toX + 1 == fromX)
                {
                    //doing en passant, meaning actually capturing a pawn
                    if (game.ChessBoard[fromX, fromY].Color == "white")
                    {
                        if (game.ChessBoard[toX, toY - 1] != null)
                        {
                            game.MoveHistory.Last().pieceCaptured = game.ChessBoard[toX, toY - 1].copy();
                            game.ChessBoard[toX, toY - 1] = null;
                        }


                    }
                    else if (game.ChessBoard[fromX, fromY].Color == "black")
                    {
                        if (game.ChessBoard[toX, toY + 1] != null)
                        {
                            game.MoveHistory.Last().pieceCaptured = game.ChessBoard[toX, toY + 1].copy();
                            game.ChessBoard[toX, toY + 1] = null;
                        }
                    }
                }



            }
        }

        public static void castlingRights(int fromX, int fromY, int toX, int toY, Game game)
        {
            //revoking king's castling rights when it moves
            if (game.ChessBoard[toX, toY].Name == "king")
            {
                (game.ChessBoard[toX, toY] as King).CastlingLong = false;
                (game.ChessBoard[toX, toY] as King).CastlingShort = false;
            }
            //revoking king's castling rights if it's color rook moves
            else if (game.ChessBoard[toX, toY].Name == "rook")
            {
                if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 0 }))
                {
                    if (game.ChessBoard[4, 0] != null)
                    {
                        if (game.ChessBoard[4, 0].Name == "king")
                        {
                            (game.ChessBoard[4, 0] as King).CastlingLong = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 0 }))
                {
                    if (game.ChessBoard[4, 0] != null)
                    {
                        if (game.ChessBoard[4, 0].Name == "king")
                        {
                            (game.ChessBoard[4, 0] as King).CastlingShort = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 7 }))
                {
                    if (game.ChessBoard[4, 7] != null)
                    {
                        if (game.ChessBoard[4, 7].Name == "king")
                        {
                            (game.ChessBoard[4, 7] as King).CastlingLong = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 7 }))
                {
                    if (game.ChessBoard[4, 7] != null)
                    {
                        if (game.ChessBoard[4, 7].Name == "king")
                        {
                            (game.ChessBoard[4, 7] as King).CastlingShort = false;
                        }
                    }
                }
            }
        }

        public static void castleCheck(int fromX, int fromY, int toX, int toY, Game game)
        {
            //castling, meaning moving rook to proper square depending whether it is a long or short castle
            if (game.ChessBoard[fromX, fromY] != null)
            {
                if (game.ChessBoard[fromX, fromY].Name == "king" && fromX - 2 == toX)
                {
                    if ((game.ChessBoard[fromX, fromY].Color == "white" && StatusCheck.checkwhite(game)) || (game.ChessBoard[fromX, fromY].Color == "black" && StatusCheck.checkblack(game)))
                    {
                        return;
                    }
                    (game.ChessBoard[fromX - 4, fromY], game.ChessBoard[toX + 1, toY]) = (game.ChessBoard[toX + 1, toY], game.ChessBoard[fromX - 4, fromY]);
                    game.ChessBoard[toX + 1, toY].x = toX + 1;
                    game.MoveHistory.Last().Castling = true;
                }
                else if (game.ChessBoard[fromX, fromY].Name == "king" && fromX + 2 == toX)
                {
                    if ((game.ChessBoard[fromX, fromY].Color == "white" && StatusCheck.checkwhite(game)) || (game.ChessBoard[fromX, fromY].Color == "black" && StatusCheck.checkblack(game)))
                    {
                        return;
                    }
                    (game.ChessBoard[fromX + 3, fromY], game.ChessBoard[toX - 1, toY]) = (game.ChessBoard[toX - 1, toY], game.ChessBoard[fromX + 3, fromY]);
                    game.ChessBoard[toX - 1, toY].x = toX - 1;
                    game.MoveHistory.Last().Castling = true;
                }
            }
        }

        public static void pawnPromotion(int fromX, int fromY, int toY, string promotion, Game game)
        {
            //promoting a pawn if it moves to 8th or 1st rank based on desired piece provided in 'promotion' parameter
            var movedPiece = game.ChessBoard[fromX, fromY];
            if (movedPiece != null)
            {
                if (movedPiece.Name == "pawn" && (toY == 7 || toY == 0))
                {
                    if (promotion == "queen")
                    {
                        game.ChessBoard[fromX, fromY] = new Queen(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "knight")
                    {
                        game.ChessBoard[fromX, fromY] = new Knight(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "rook")
                    {
                        game.ChessBoard[fromX, fromY] = new Rook(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "bishop")
                    {
                        game.ChessBoard[fromX, fromY] = new Bishop(movedPiece.Color, fromX, fromY);
                    }
                    else
                    {
                        game.MoveHistory.Last().piecePromoted = null;
                        return;
                    }
                    game.MoveHistory.Last().piecePromoted = game.ChessBoard[fromX, fromY];
                }
            }

        }

        public static void enPassantCancel(Game game)
        {
            if (game.MoveHistory.Any())
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (game.ChessBoard[i, j] != null)
                        {
                            if (game.ChessBoard[i, j].Name == "pawn")
                            {
                                var pawn = game.ChessBoard[i, j] as Pawn;
                                if (pawn.EnPassant && game.MoveHistory.Last().toX != i || game.MoveHistory.Last().toY != j)
                                {
                                    pawn.EnPassant = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

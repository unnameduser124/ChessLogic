using ChessLogic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ChessBoard[0, 6] = new Pawn("Black");
                ChessBoard[1, 4] = new Pawn("White");
                ChessBoard[0, 3] = new Pawn("Black");
                ChessBoard[1, 1] = new Pawn("White");
            }
            else
            {
                startingPosition();
            }
        }
        public Piece[,] ChessBoard { get; set; }

        public List<Move> MoveHistory { get; private set; }

        public void startingPosition()
        {
            ChessBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
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

        public bool movePiece(int fromX, int fromY, int toX, int toY, string promotion = "")
        {
            Console.WriteLine($"from = {(char)(fromX + 97)}{fromY + 1} to = {(char)(toX + 97)}{toY + 1}");

            void EnPassantCheck()
            {
                if (ChessBoard[fromX, fromY].Name.ToLower() == "pawn")
                {
                    //after a pawn moves two spaces up or down the board (depending on color) it's attribute "EnPassant" changes to true
                    if (fromY + 2 == toY || fromY - 2 == toY)
                    {
                        (ChessBoard[fromX, fromY] as Pawn).EnPassant = true;
                    }

                    //doing en passant, meaning actually capturing a pawn
                    if (ChessBoard[fromX, fromY].Color.ToLower() == "white")
                    {
                        if (ChessBoard[toX, toY - 1] != null)
                        {
                            MoveHistory.Last().pieceCaptured = ChessBoard[toX, toY - 1].copy();
                            ChessBoard[toX, toY - 1] = null;
                        }


                    }
                    else if (ChessBoard[fromX, fromY].Color.ToLower() == "black")
                    {
                        if (ChessBoard[toX, toY + 1] != null)
                        {
                            MoveHistory.Last().pieceCaptured = ChessBoard[toX, toY + 1].copy();
                            ChessBoard[toX, toY + 1] = null;
                        }


                    }


                }
            }

            void castlingRights()
            {
                //revoking king's castling rights when it moves
                if (ChessBoard[toX, toY].Name == "King")
                {
                    (ChessBoard[toX, toY] as King).CastlingLong = false;
                    (ChessBoard[toX, toY] as King).CastlingShort = false;
                }
                //revoking king's castling rights if it's color rook moves
                else if (ChessBoard[toX, toY].Name == "Rook")
                {
                    if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 0 }))
                    {
                        if (ChessBoard[4, 0] != null)
                        {
                            if (ChessBoard[4, 0].Name == "King")
                            {
                                (ChessBoard[4, 0] as King).CastlingLong = false;
                            }
                        }
                    }
                    else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 0 }))
                    {
                        if (ChessBoard[4, 0] != null)
                        {
                            if (ChessBoard[4, 0].Name == "King")
                            {
                                (ChessBoard[4, 0] as King).CastlingShort = false;
                            }
                        }
                    }
                    else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 7 }))
                    {
                        if (ChessBoard[4, 7] != null)
                        {
                            if (ChessBoard[4, 7].Name == "King")
                            {
                                (ChessBoard[4, 7] as King).CastlingLong = false;
                            }
                        }
                    }
                    else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 7 }))
                    {
                        if (ChessBoard[4, 7] != null)
                        {
                            if (ChessBoard[4, 7].Name == "King")
                            {
                                (ChessBoard[4, 7] as King).CastlingShort = false;
                            }
                        }
                    }
                }
            }

            void castleCheck()
            {
                //castling, meaning moving rook to proper square depending whether it is a long or short castle
                if (ChessBoard[fromX, fromY] != null)
                {
                    if (ChessBoard[fromX, fromY].Name == "King" && fromX - 2 == toX)
                    {
                        (ChessBoard[fromX - 4, fromY], ChessBoard[toX + 1, toY]) = (ChessBoard[toX + 1, toY], ChessBoard[fromX - 4, fromY]);
                        MoveHistory.Last().Castling = true;
                    }
                    else if (ChessBoard[fromX, fromY].Name == "King" && fromX + 2 == toX)
                    {
                        (ChessBoard[fromX + 3, fromY], ChessBoard[toX - 1, toY]) = (ChessBoard[toX - 1, toY], ChessBoard[fromX + 3, fromY]);
                        MoveHistory.Last().Castling = true;
                    }
                }
            }

            void pawnPromotion()
            {
                //promoting a pawn if it moves to 8th or 1st rank based on desired piece provided in 'promotion' parameter
                var movedPiece = ChessBoard[fromX, fromY];
                if (movedPiece.Name.ToLower() == "pawn" && (toY == 7 || toY == 0))
                {
                    if (promotion == "queen")
                    {
                        ChessBoard[fromX, fromY] = new Queen(movedPiece.Color);
                    }
                    else if (promotion == "knight")
                    {
                        ChessBoard[fromX, fromY] = new Knight(movedPiece.Color);
                    }
                    else if (promotion == "rook")
                    {
                        ChessBoard[fromX, fromY] = new Rook(movedPiece.Color);
                    }
                    else if (promotion == "bishop")
                    {
                        ChessBoard[fromX, fromY] = new Bishop(movedPiece.Color);
                    }
                    else
                    {
                        MoveHistory.Last().piecePromoted = null;
                        return;
                    }
                    MoveHistory.Last().piecePromoted = ChessBoard[fromX, fromY];
                }
            }


            if (fromX >= 0 && fromX < 8 && fromY >= 0 && fromY < 8 && toX >= 0 && toX < 8 && toY >= 0 && toY < 8)
            {
                var piece = ChessBoard[fromX, fromY];
                var destination = ChessBoard[toX, toY];
                if (piece != null)
                {
                    foreach (var move in piece.availableMoves(this))
                    {

                        if (new int[] { toX, toY }.SequenceEqual(move))
                        {

                            if (destination == null || destination.Color.ToLower() != piece.Color.ToLower())
                            {
                                if (destination != null)
                                {
                                    //actions performed if a piece is captured
                                    MoveHistory.Add(new Move(fromX, fromY, toX, toY, destination.copy()));
                                    ChessBoard[toX, toY] = null;
                                    pawnPromotion();
                                }
                                else
                                {
                                    //actions performed if a piece is moved to an empty square 
                                    MoveHistory.Add(new Move(fromX, fromY, toX, toY));
                                    castleCheck();
                                    EnPassantCheck();
                                    pawnPromotion();
                                }
                                //actually moving a piece (swap of values in the board array)
                                (ChessBoard[fromX, fromY], ChessBoard[toX, toY]) = (ChessBoard[toX, toY], ChessBoard[fromX, fromY]);

                                //checks if move was valid
                                if (check() == ChessBoard[toX, toY].Color.ToLower())
                                {
                                    undoMove();
                                    if (ChessBoard[fromX, fromY].Name.ToLower() == "pawn")
                                    {
                                        (ChessBoard[fromX, fromY] as Pawn).EnPassant = false;

                                    }
                                    return false;
                                }
                                //rolls back pawn move if it was moved to 8ht or 1st rank and not promoted ('promotion' parameter is empty)
                                else if ((toY == 7 || toY == 0) && ChessBoard[toX, toY].Name.ToLower() == "pawn")
                                {
                                    undoMove();
                                    return false;
                                }
                                //checks castling rights
                                else
                                {
                                    castlingRights();
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

        //function checking if either one of the kings is in check
        public string check()
        {
            int[] whiteKingPosition = null;
            int[] blackKingPosition = null;

            //searches for white and black king position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
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

            //returns false if either black or white king was not found
            if (blackKingPosition == null || whiteKingPosition == null)
            {
                return "false";
            }

            //checks if any of the pieces one the boards attacks opponent's king
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        foreach (var move in ChessBoard[i, j].availableMoves(this))
                        {
                            if (move.SequenceEqual(whiteKingPosition))
                            {
                                return "white";
                            }
                            else if (move.SequenceEqual(blackKingPosition))
                            {
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
            if (MoveHistory.Any())
            {
                //moves back the piece that was moved in the last turn
                (ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY], ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY]) = (ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY], ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY]);

                //places back captured piece if any was captured
                if (MoveHistory.Last().pieceCaptured != null)
                {
                    //placing back in case of en passant
                    if (MoveHistory.Last().pieceCaptured.Name.ToLower() == "pawn")
                    {
                        if ((MoveHistory.Last().pieceCaptured as Pawn).EnPassant)
                        {
                            if (MoveHistory.Last().pieceCaptured.Color.ToLower() == "white")
                            {
                                ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY + 1] = MoveHistory.Last().pieceCaptured;
                            }
                            else
                            {
                                ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY - 1] = MoveHistory.Last().pieceCaptured;
                            }
                        }
                        else
                        {
                            ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY] = MoveHistory.Last().pieceCaptured;
                        }
                    }
                    //placing back in any other case
                    else
                    {
                        ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY] = MoveHistory.Last().pieceCaptured;
                    }
                }
                else
                {
                    ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY] = MoveHistory.Last().pieceCaptured;
                }

                //actions performed if the last move was castling - placing back the rook
                if (MoveHistory.Last().Castling)
                {
                    var fromX = MoveHistory.Last().fromX;
                    var fromY = MoveHistory.Last().fromY;
                    var toX = MoveHistory.Last().toX;
                    var toY = MoveHistory.Last().toY;
                    if (fromX - 2 == toX)
                    {
                        (ChessBoard[3, fromY], ChessBoard[0, toY]) = (ChessBoard[0, toY], ChessBoard[3, fromY]);
                    }
                    else
                    {
                        (ChessBoard[5, fromY], ChessBoard[7, toY]) = (ChessBoard[7, toY], ChessBoard[5, fromY]);
                    }
                }

                //actions performed if the last move was a pawn promotion - turning promoted piece back into a pawn
                if (MoveHistory.Last().piecePromoted != null)
                {
                    var fromX = MoveHistory.Last().fromX;
                    var fromY = MoveHistory.Last().fromY;
                    var toX = MoveHistory.Last().toX;
                    var toY = MoveHistory.Last().toY;
                    ChessBoard[fromX, fromY] = new Pawn(ChessBoard[fromX, fromY].Color);
                    ChessBoard[toX, toY] = MoveHistory.Last().pieceCaptured;
                }

                MoveHistory.Remove(MoveHistory.Last());
            }


        }

    }
}

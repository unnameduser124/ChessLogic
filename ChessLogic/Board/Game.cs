using ChessLogic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLogic.Board
{
    public class Game
    {

        public Game(string position = "")
        {
            ChessBoard = new Piece[8, 8];
            MoveHistory = new List<Move>();
            PositionHistory = new List<String>();
            whiteTurn = true;
            turnCounter = 1;
            movesSinceLastCapture = 0;
            gameStatus = GameStatus.inProgress;
            if (position != "")
            {
                setupFromFEN(position);
            }
            else
            {
                startingPosition();
            }
        }
        public Piece[,] ChessBoard { get; set; }

        public List<Move> MoveHistory { get; private set; }

        public List<String> PositionHistory { get; set; }

        public bool whiteTurn { get; set; }

        public int turnCounter { get; set; }

        public int movesSinceLastCapture { get; set; }

        public GameStatus gameStatus { get; set; }

        public enum GameStatus
        {
            whiteWon,
            blackWon,
            draw,
            inProgress
        }

        public void startingPosition()
        {
            ChessBoard = new Piece[8, 8];
            MoveHistory = new List<Move>();
            PositionHistory = new List<String>();
            whiteTurn = true;
            turnCounter = 1;
            movesSinceLastCapture = 0;
            gameStatus = GameStatus.inProgress;
            for (int i = 0; i < 8; i++)
            {
                ChessBoard[i, 1] = new Pawn("white", i, 1);
                ChessBoard[i, 6] = new Pawn("black", i, 6);
            }

            ChessBoard[0, 0] = new Rook("white", 0, 0);
            ChessBoard[7, 0] = new Rook("white", 7, 0);
            ChessBoard[0, 7] = new Rook("black", 0, 7);
            ChessBoard[7, 7] = new Rook("black", 7, 7);

            ChessBoard[1, 0] = new Knight("white", 1, 0);
            ChessBoard[6, 0] = new Knight("white", 6, 0);
            ChessBoard[1, 7] = new Knight("black", 1, 7);
            ChessBoard[6, 7] = new Knight("black", 6, 7);

            ChessBoard[2, 0] = new Bishop("white", 2, 0);
            ChessBoard[5, 0] = new Bishop("white", 5, 0);
            ChessBoard[2, 7] = new Bishop("black", 2, 7);
            ChessBoard[5, 7] = new Bishop("black", 5, 7);

            ChessBoard[3, 0] = new Queen("white", 3, 0);
            ChessBoard[4, 0] = new King("white", 4, 0);
            ChessBoard[3, 7] = new Queen("black", 3, 7);
            ChessBoard[4, 7] = new King("black", 4, 7);

            savePosition();
        }

        public bool movePiece(int fromX, int fromY, int toX, int toY, string promotion = "")
        {
            gameStatusCheck();
            if (gameStatus == GameStatus.inProgress)
            {
                //checking if moved piece is in the right color
                if (ChessBoard[fromX, fromY].Color == "black" && whiteTurn)
                {
                    return false;
                }
                else if (ChessBoard[fromX, fromY].Color == "white" && !whiteTurn)
                {
                    return false;
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
                                if (destination == null || destination.Color != piece.Color)
                                {
                                    if (destination != null)
                                    {
                                        //actions performed if a piece is captured
                                        MoveHistory.Add(new Move(fromX, fromY, toX, toY, piece.NotationName, turnCounter, destination.copy()));
                                        ChessBoard[toX, toY] = null;
                                        pawnPromotion(fromX, fromY, toY, promotion);
                                    }
                                    else
                                    {
                                        //actions performed if a piece is moved to an empty square 
                                        MoveHistory.Add(new Move(fromX, fromY, toX, toY, piece.NotationName, turnCounter));
                                        castleCheck(fromX, fromY, toX, toY);
                                        EnPassantCheck(fromX, fromY, toX, toY);
                                        pawnPromotion(fromX, fromY, toY, promotion);
                                    }
                                    ChessBoard[toX, toY] = null;
                                    //actually moving a piece (swap of values in the board array)
                                    (ChessBoard[fromX, fromY], ChessBoard[toX, toY]) = (ChessBoard[toX, toY], ChessBoard[fromX, fromY]);
                                    ChessBoard[toX, toY].x = toX;
                                    ChessBoard[toX, toY].y = toY;
                                    savePosition();

                                    if (!moveValidation(fromX, fromY, toX, toY))
                                    {
                                        return false;
                                    }
                                    castlingRights(fromX, fromY, toX, toY);
                                    enPassantCancel();

                                    whiteTurn = !whiteTurn;

                                    if (!whiteTurn)
                                    {
                                        turnCounter++;
                                    }

                                    if (MoveHistory.Last().pieceCaptured == null && MoveHistory.Last().pieceName != "")
                                    {
                                        movesSinceLastCapture++;
                                    }
                                    else
                                    {
                                        movesSinceLastCapture = 0;
                                    }


                                    if (checkwhite() || checkblack())
                                    {
                                        MoveHistory.Last().Check = true;
                                    }

                                    //external function
                                    gameStatusCheck();
                                    return true;
                                }
                                break;
                            }
                        }
                    }
                }
                enPassantCancel();
                return false;
            }


            return false;
        }

        void EnPassantCheck(int fromX, int fromY, int toX, int toY)
        {
            if (ChessBoard[fromX, fromY].Name == "pawn")
            {
                //after a pawn moves two spaces up or down the board (depending on color) it's attribute "EnPassant" changes to true
                if (fromY + 2 == toY || fromY - 2 == toY)
                {
                    (ChessBoard[fromX, fromY] as Pawn).EnPassant = true;
                }

                if (toX - 1 == fromX || toX + 1 == fromX)
                {
                    //doing en passant, meaning actually capturing a pawn
                    if (ChessBoard[fromX, fromY].Color == "white")
                    {
                        if (ChessBoard[toX, toY - 1] != null)
                        {
                            MoveHistory.Last().pieceCaptured = ChessBoard[toX, toY - 1].copy();
                            ChessBoard[toX, toY - 1] = null;
                        }


                    }
                    else if (ChessBoard[fromX, fromY].Color == "black")
                    {
                        if (ChessBoard[toX, toY + 1] != null)
                        {
                            MoveHistory.Last().pieceCaptured = ChessBoard[toX, toY + 1].copy();
                            ChessBoard[toX, toY + 1] = null;
                        }
                    }
                }



            }
        }

        void castlingRights(int fromX, int fromY, int toX, int toY)
        {
            //revoking king's castling rights when it moves
            if (ChessBoard[toX, toY].Name == "king")
            {
                (ChessBoard[toX, toY] as King).CastlingLong = false;
                (ChessBoard[toX, toY] as King).CastlingShort = false;
            }
            //revoking king's castling rights if it's color rook moves
            else if (ChessBoard[toX, toY].Name == "rook")
            {
                if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 0 }))
                {
                    if (ChessBoard[4, 0] != null)
                    {
                        if (ChessBoard[4, 0].Name == "king")
                        {
                            (ChessBoard[4, 0] as King).CastlingLong = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 0 }))
                {
                    if (ChessBoard[4, 0] != null)
                    {
                        if (ChessBoard[4, 0].Name == "king")
                        {
                            (ChessBoard[4, 0] as King).CastlingShort = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 0, 7 }))
                {
                    if (ChessBoard[4, 7] != null)
                    {
                        if (ChessBoard[4, 7].Name == "king")
                        {
                            (ChessBoard[4, 7] as King).CastlingLong = false;
                        }
                    }
                }
                else if (new int[] { fromX, fromY }.SequenceEqual(new int[] { 7, 7 }))
                {
                    if (ChessBoard[4, 7] != null)
                    {
                        if (ChessBoard[4, 7].Name == "king")
                        {
                            (ChessBoard[4, 7] as King).CastlingShort = false;
                        }
                    }
                }
            }
        }

        void castleCheck(int fromX, int fromY, int toX, int toY)
        {
            //castling, meaning moving rook to proper square depending whether it is a long or short castle
            if (ChessBoard[fromX, fromY] != null)
            {
                if (ChessBoard[fromX, fromY].Name == "king" && fromX - 2 == toX)
                {
                    if ((ChessBoard[fromX, fromY].Color == "white" && checkwhite()) || (ChessBoard[fromX, fromY].Color == "black" && checkblack()))
                    {
                        return;
                    }
                    (ChessBoard[fromX - 4, fromY], ChessBoard[toX + 1, toY]) = (ChessBoard[toX + 1, toY], ChessBoard[fromX - 4, fromY]);
                    ChessBoard[toX + 1, toY].x = toX + 1;
                    MoveHistory.Last().Castling = true;
                }
                else if (ChessBoard[fromX, fromY].Name == "king" && fromX + 2 == toX)
                {
                    if ((ChessBoard[fromX, fromY].Color == "white" && checkwhite()) || (ChessBoard[fromX, fromY].Color == "black" && checkblack()))
                    {
                        return;
                    }
                    (ChessBoard[fromX + 3, fromY], ChessBoard[toX - 1, toY]) = (ChessBoard[toX - 1, toY], ChessBoard[fromX + 3, fromY]);
                    ChessBoard[toX - 1, toY].x = toX - 1;
                    MoveHistory.Last().Castling = true;
                }
            }
        }

        void pawnPromotion(int fromX, int fromY, int toY, string promotion)
        {
            //promoting a pawn if it moves to 8th or 1st rank based on desired piece provided in 'promotion' parameter
            var movedPiece = ChessBoard[fromX, fromY];
            if (movedPiece != null)
            {
                if (movedPiece.Name == "pawn" && (toY == 7 || toY == 0))
                {
                    if (promotion == "queen")
                    {
                        ChessBoard[fromX, fromY] = new Queen(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "knight")
                    {
                        ChessBoard[fromX, fromY] = new Knight(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "rook")
                    {
                        ChessBoard[fromX, fromY] = new Rook(movedPiece.Color, fromX, fromY);
                    }
                    else if (promotion == "bishop")
                    {
                        ChessBoard[fromX, fromY] = new Bishop(movedPiece.Color, fromX, fromY);
                    }
                    else
                    {
                        MoveHistory.Last().piecePromoted = null;
                        return;
                    }
                    MoveHistory.Last().piecePromoted = ChessBoard[fromX, fromY];
                }
            }

        }

        bool moveValidation(int fromX, int fromY, int toX, int toY)
        {
            //checks if move was valid
            if ((checkwhite() && ChessBoard[toX, toY].Color == "white") || (checkblack() && ChessBoard[toX, toY].Color == "black"))
            {
                undoMove();
                if (ChessBoard[fromX, fromY].Name == "pawn")
                {
                    (ChessBoard[fromX, fromY] as Pawn).EnPassant = false;

                }
                enPassantCancel();
                return false;
            }
            //rolls back pawn move if it was moved to 8ht or 1st rank and not promoted ('promotion' parameter is empty)
            else if ((toY == 7 || toY == 0) && ChessBoard[toX, toY].Name == "pawn")
            {
                undoMove();
                enPassantCancel();
                return false;
            }
            return true;
        }

        void gameStatusCheck()
        {
            if (turnCounter >= 0)
            {
                if (repetitionDrawCheck())
                {
                    gameStatus = GameStatus.draw;
                }
                else if (stalemate())
                {
                    gameStatus = GameStatus.draw;
                }
                else if (checkmate())
                {

                    if (whiteTurn)
                    {
                        gameStatus = GameStatus.blackWon;
                        MoveHistory.Last().Checkmate = true;
                    }
                    else
                    {
                        gameStatus = GameStatus.whiteWon;
                        MoveHistory.Last().Checkmate = true;
                    }
                }
                else if (movesSinceLastCapture > 99)
                {
                    gameStatus = GameStatus.draw;
                }
                else if (insufficientMaterial())
                {
                    gameStatus = GameStatus.draw;
                }
            }
        }


        public void undoMove()
        {
            if (MoveHistory.Any())
            {
                //moves back the piece that was moved in the last turn
                (ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY], ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY]) = (ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY], ChessBoard[MoveHistory.Last().toX, MoveHistory.Last().toY]);
                ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY].x = MoveHistory.Last().fromX;
                ChessBoard[MoveHistory.Last().fromX, MoveHistory.Last().fromY].y = MoveHistory.Last().fromY;

                //places back captured piece if any was captured
                if (MoveHistory.Last().pieceCaptured != null)
                {
                    //placing back in case of en passant
                    if (MoveHistory.Last().pieceCaptured.Name == "pawn")
                    {
                        if ((MoveHistory.Last().pieceCaptured as Pawn).EnPassant)
                        {
                            if (MoveHistory.Last().pieceCaptured.Color == "white")
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
                        ChessBoard[0, fromY].x = 3;
                        ChessBoard[0, fromY].y = fromY;
                    }
                    else
                    {
                        (ChessBoard[5, fromY], ChessBoard[7, toY]) = (ChessBoard[7, toY], ChessBoard[5, fromY]);
                        ChessBoard[7, toY].x = 7;
                        ChessBoard[7, toY].y = fromY;
                    }
                }

                //actions performed if the last move was a pawn promotion - turning promoted piece back into a pawn
                if (MoveHistory.Last().piecePromoted != null)
                {
                    var fromX = MoveHistory.Last().fromX;
                    var fromY = MoveHistory.Last().fromY;
                    var toX = MoveHistory.Last().toX;
                    var toY = MoveHistory.Last().toY;
                    ChessBoard[fromX, fromY] = new Pawn(ChessBoard[fromX, fromY].Color, fromX, fromY);
                    ChessBoard[toX, toY] = MoveHistory.Last().pieceCaptured;
                }
                MoveHistory.Remove(MoveHistory.Last());
                PositionHistory.Remove(PositionHistory.Last());
            }
        }

        public void enPassantCancel()
        {
            if (MoveHistory.Any())
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (ChessBoard[i, j] != null)
                        {
                            if (ChessBoard[i, j].Name == "pawn")
                            {
                                var pawn = ChessBoard[i, j] as Pawn;
                                if (pawn.EnPassant && MoveHistory.Last().toX != i || MoveHistory.Last().toY != j)
                                {
                                    pawn.EnPassant = false;
                                }
                            }
                        }
                    }
                }
            }
        }


        public King findKing(string color)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        if (ChessBoard[i, j].Name == "king" && ChessBoard[i, j].Color == color)
                        {
                            return (ChessBoard[i, j] as King);
                        }
                    }
                }
            }
            return new King("none");
        }

        public bool checkwhite()
        {
            int[] whiteKingPosition = null;

            //searches for white and black king position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        if (ChessBoard[i, j].Name == "king")
                        {
                            if (ChessBoard[i, j].Color == "white")
                            {
                                whiteKingPosition = new int[] { i, j };
                            }
                        }

                        if (whiteKingPosition != null)
                        {
                            break;
                        }
                    }
                }
                if (whiteKingPosition != null)
                {
                    break;
                }
            }

            //returns false if either black or white king was not found
            if (whiteKingPosition == null)
            {
                return false;
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
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        public bool checkblack()
        {
            int[] blackKingPosition = null;

            //searches for white and black king position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        if (ChessBoard[i, j].Name == "king")
                        {
                            if (ChessBoard[i, j].Color == "black")
                            {
                                blackKingPosition = new int[] { i, j };
                            }
                        }

                        if (blackKingPosition != null)
                        {
                            break;
                        }
                    }
                }
                if (blackKingPosition != null)
                {
                    break;
                }
            }

            //returns false if either black or white king was not found
            if (blackKingPosition == null)
            {
                return false;
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
                            if (move.SequenceEqual(blackKingPosition))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        bool repetitionDrawCheck()
        {
            if (PositionHistory.Any())
            {
                var positionChecked = PositionHistory.Last();
                int positionCounter = 0;
                foreach (var position in PositionHistory)
                {
                    if (positionChecked == position)
                    {
                        positionCounter++;
                        if (positionCounter > 2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool checkmate()
        {
            if (!checkblack() && !checkwhite())
            {
                return false;
            }
            var clonedBoard = clone();
            clonedBoard.gameStatus = GameStatus.inProgress;
            clonedBoard.turnCounter = -10;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = clonedBoard.ChessBoard[i, j];
                    if (piece != null)
                    {
                        if (piece.Color == "white" && clonedBoard.whiteTurn)
                        {
                            foreach (var move in piece.availableMoves(clonedBoard))
                            {
                                if (clonedBoard.movePiece(i, j, move[0], move[1]))
                                {
                                    return false;
                                }
                                if(piece.Name == "pawn" && move[1] == 7)
                                {
                                    if (clonedBoard.movePiece(i, j, move[0], move[1], "queen"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "rook"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "knight"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "bishop"))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (piece.Color == "black" && !clonedBoard.whiteTurn)
                        {
                            foreach (var move in piece.availableMoves(clonedBoard))
                            {
                                if (clonedBoard.movePiece(i, j, move[0], move[1]))
                                {
                                    return false;
                                }
                                if (piece.Name == "pawn" && move[1] == 0)
                                {
                                    if (clonedBoard.movePiece(i, j, move[0], move[1], "queen"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "rook"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "knight"))
                                    {
                                        return false;
                                    }
                                    else if (clonedBoard.movePiece(i, j, move[0], move[1], "bishop"))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        bool stalemate()
        {
            if (checkblack() || checkwhite())
            {
                return false;
            }
            var clonedBoard = clone();
            clonedBoard.gameStatus = GameStatus.inProgress;
            clonedBoard.turnCounter = -10;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = clonedBoard.ChessBoard[i, j];
                    if (piece != null)
                    {
                        if (piece.Color == "white" && clonedBoard.whiteTurn)
                        {
                            foreach (var move in piece.availableMoves(clonedBoard))
                            {
                                if (clonedBoard.movePiece(i, j, move[0], move[1]))
                                {
                                    return false;
                                }
                            }
                        }
                        else if (piece.Color == "black" && !clonedBoard.whiteTurn)
                        {
                            foreach (var move in piece.availableMoves(clonedBoard))
                            {
                                if (clonedBoard.movePiece(i, j, move[0], move[1]))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        bool insufficientMaterial()
        {
            int knightCounter = 0;
            string bishopColor = "";
            for(int i=0; i<8; i++)
            {
                for(int j=0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        if(ChessBoard[i,j].Name == "pawn")
                        {
                            return false;
                        }
                        else if (ChessBoard[i, j].Name == "rook")
                        {
                            return false;
                        }
                        else if (ChessBoard[i, j].Name == "queen")
                        {
                            return false;
                        }
                        else if (ChessBoard[i, j].Name == "bishop")
                        {
                            if (bishopColor == "")
                            {
                                if (i % 2 == j % 2)
                                {
                                    bishopColor = "black";
                                }
                                else
                                {
                                    bishopColor = "white";
                                }
                            }
                            else
                            {
                                if(bishopColor == "white" && i % 2 == j % 2)
                                {
                                    return false;
                                }
                                else if(bishopColor == "black" && i % 2 != j % 2)
                                {
                                    return false;
                                }
                            }
                        }
                        else if(ChessBoard[i,j].Name == "knight")
                        {
                            knightCounter++;
                            if (knightCounter > 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        void setupFromFEN(string fen)
        {
            var column = 0;
            var row = 7;
            var index = 0;
            foreach (var item in fen)
            {
                if (item == 'k')
                {
                    ChessBoard[column, row] = new King("black", column, row);
                    column++;
                }
                else if (item == 'K')
                {
                    ChessBoard[column, row] = new King("white", column, row);
                    column++;
                }
                else if (item == 'q')
                {
                    ChessBoard[column, row] = new Queen("black", column, row);
                    column++;
                }
                else if (item == 'Q')
                {
                    ChessBoard[column, row] = new Queen("white", column, row);
                    column++;
                }
                else if (item == 'b')
                {
                    ChessBoard[column, row] = new Bishop("black", column, row);
                    column++;
                }
                else if (item == 'B')
                {
                    ChessBoard[column, row] = new Bishop("white", column, row);
                    column++;
                }
                else if (item == 'n')
                {
                    ChessBoard[column, row] = new Knight("black", column, row);
                    column++;
                }
                else if (item == 'N')
                {
                    ChessBoard[column, row] = new Knight("white", column, row);
                    column++;
                }
                else if (item == 'p')
                {
                    ChessBoard[column, row] = new Pawn("black", column, row);
                    column++;
                }
                else if (item == 'P')
                {
                    ChessBoard[column, row] = new Pawn("white", column, row);
                    column++;
                }
                else if (item == 'r')
                {
                    ChessBoard[column, row] = new Rook("black", column, row);
                    column++;
                }
                else if (item == 'R')
                {
                    ChessBoard[column, row] = new Rook("white", column, row);
                    column++;
                }
                else if (item == '/')
                {
                    column = 0;
                    row--;
                }
                else
                {
                    column += item - '0';
                }
                if (item == ' ')
                {
                    index = fen.IndexOf(item);
                    break;
                }
            }

            whiteTurn = fen[++index] == 'w';
            index++;

            var kingwhite = findKing("white");
            var kingblack = findKing("black");
            if (kingwhite.Color != "none")
            {
                kingwhite.CastlingLong = false;
                kingwhite.CastlingShort = false;
            }
            if (kingblack.Color != "none")
            {
                kingblack.CastlingLong = false;
                kingblack.CastlingShort = false;
            }
            while (fen[++index] != ' ')
            {
                if (fen[index] == 'K')
                {
                    if (kingwhite.Color != "none")
                    {
                        kingwhite.CastlingShort = true;
                    }
                }
                else if (fen[index] == 'k')
                {
                    if (kingblack.Color != "none")
                    {
                        kingblack.CastlingShort = true;
                    }
                }
                else if (fen[index] == 'Q')
                {
                    if (kingwhite.Color != "none")
                    {
                        kingwhite.CastlingLong = true;
                    }
                }
                else if (fen[index] == 'q')
                {
                    if (kingblack.Color != "none")
                    {
                        kingblack.CastlingLong = true;
                    }
                }
                else
                {
                    break;
                }

            }

            index = fen.Length;
            var movesString = "";
            while (fen[--index] != ' ')
            {
                movesString += fen[index];
            }
            movesString = GlobalFunctions.Reverse(movesString);
            turnCounter = int.Parse(movesString);
            movesString = "";
            while (fen[--index] != ' ')
            {
                movesString += fen[index];
            }
            movesString = GlobalFunctions.Reverse(movesString);
            movesSinceLastCapture = int.Parse(movesString);

            movesString = "";
            while (fen[--index] != ' ')
            {
                movesString += fen[index];
            }
            movesString = GlobalFunctions.Reverse(movesString);
            if (movesString != "-")
            {
                var enPassantPosition = GlobalFunctions.cordToIntArray(movesString);
                if (ChessBoard[enPassantPosition[0], enPassantPosition[1] - 1] != null)
                {
                    if (ChessBoard[enPassantPosition[0], enPassantPosition[1] - 1].Name == "pawn")
                    {
                        (ChessBoard[enPassantPosition[0], enPassantPosition[1] - 1] as Pawn).EnPassant = true;
                    }
                }
                else if (ChessBoard[enPassantPosition[0], enPassantPosition[1] + 1] != null)
                {
                    if (ChessBoard[enPassantPosition[0], enPassantPosition[1] + 1].Name == "pawn")
                    {
                        (ChessBoard[enPassantPosition[0], enPassantPosition[1] + 1] as Pawn).EnPassant = true;
                    }
                }
            }

        }

        void savePosition()
        {
            var fen = GlobalFunctions.FEN(this);
            var justPositionfen = fen.Remove(fen.IndexOf(' '), fen.Length- fen.IndexOf(' '));
            PositionHistory.Add(justPositionfen);
        }

        public Game clone()
        {
            var newChessBoard = new Piece[8, 8];
            var turn = whiteTurn;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard[i, j] != null)
                    {
                        newChessBoard[i, j] = ChessBoard[i, j].copy();
                    }
                }
            }
            var newGame = new Game()
            {
                ChessBoard = newChessBoard,
                whiteTurn = turn,
                MoveHistory = new List<Move>(),
                PositionHistory = new List<String>()
            };
            return newGame;
        }

    }
}

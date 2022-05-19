using ChessLogic.Notation;
using ChessLogic.Pieces;
using ChessLogic.Rules;
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
                NotationGenerator.setupFromFEN(position, this);
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
                                        SpecialMoves.pawnPromotion(fromX, fromY, toY, promotion, this);
                                    }
                                    else
                                    {
                                        //actions performed if a piece is moved to an empty square 
                                        MoveHistory.Add(new Move(fromX, fromY, toX, toY, piece.NotationName, turnCounter));
                                        SpecialMoves.castleCheck(fromX, fromY, toX, toY, this);
                                        SpecialMoves.EnPassantCheck(fromX, fromY, toX, toY, this);
                                        SpecialMoves.pawnPromotion(fromX, fromY, toY, promotion, this);
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
                                    SpecialMoves.castlingRights(fromX, fromY, toX, toY, this);
                                    SpecialMoves.enPassantCancel(this);

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


                                    if (StatusCheck.checkwhite(this) || StatusCheck.checkblack(this))
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
                SpecialMoves.enPassantCancel(this);
                return false;
            }


            return false;
        }



        bool moveValidation(int fromX, int fromY, int toX, int toY)
        {

            if (!MoveValidation.kingInCheckValidation(fromX, fromY, toX, toY, this) || !MoveValidation.pawnPromotionValidation(toX, toY, this))
            {
                return false;
            }
            return true;
        }

        void gameStatusCheck()
        {
            if (turnCounter >= 0)
            {
                if (StatusCheck.repetitionDraw(this))
                {
                    gameStatus = GameStatus.draw;
                }
                else if (StatusCheck.stalemate(this))
                {
                    gameStatus = GameStatus.draw;
                }
                else if (StatusCheck.checkmate(this))
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
                else if (StatusCheck.insufficientMaterial(this))
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

        void savePosition()
        {
            var fen = NotationGenerator.FEN(this);
            var justPositionfen = fen.Remove(fen.IndexOf(' '), fen.Length - fen.IndexOf(' '));
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

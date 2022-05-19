using ChessLogic.Board;
using System.Linq;
using static ChessLogic.Board.Game;

namespace ChessLogic.Rules
{
    public abstract class StatusCheck
    {

        public static bool checkwhite(Game game)
        {
            int[] whiteKingPosition = null;

            //searches for white and black king position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.ChessBoard[i, j] != null)
                    {
                        if (game.ChessBoard[i, j].Name == "king")
                        {
                            if (game.ChessBoard[i, j].Color == "white")
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
                    if (game.ChessBoard[i, j] != null)
                    {
                        foreach (var move in game.ChessBoard[i, j].availableMoves(game))
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
        public static bool checkblack(Game game)
        {
            int[] blackKingPosition = null;

            //searches for white and black king position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.ChessBoard[i, j] != null)
                    {
                        if (game.ChessBoard[i, j].Name == "king")
                        {
                            if (game.ChessBoard[i, j].Color == "black")
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
                    if (game.ChessBoard[i, j] != null)
                    {
                        foreach (var move in game.ChessBoard[i, j].availableMoves(game))
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
        public static bool repetitionDraw(Game game)
        {
            if (game.PositionHistory.Any())
            {
                var positionChecked = game.PositionHistory.Last();
                int positionCounter = 0;
                foreach (var position in game.PositionHistory)
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
        public static bool checkmate(Game game)
        {
            if (!checkblack(game) && !checkwhite(game))
            {
                return false;
            }
            var clonedBoard = game.clone();
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
                                if (piece.Name == "pawn" && move[1] == 7)
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
        public static bool stalemate(Game game)
        {
            if (checkblack(game) || checkwhite(game))
            {
                return false;
            }
            var clonedBoard = game.clone();
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
        public static bool insufficientMaterial(Game game)
        {
            int knightCounter = 0;
            string bishopColor = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.ChessBoard[i, j] != null)
                    {
                        if (game.ChessBoard[i, j].Name == "pawn")
                        {
                            return false;
                        }
                        else if (game.ChessBoard[i, j].Name == "rook")
                        {
                            return false;
                        }
                        else if (game.ChessBoard[i, j].Name == "queen")
                        {
                            return false;
                        }
                        else if (game.ChessBoard[i, j].Name == "bishop")
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
                                if (bishopColor == "white" && i % 2 == j % 2)
                                {
                                    return false;
                                }
                                else if (bishopColor == "black" && i % 2 != j % 2)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (game.ChessBoard[i, j].Name == "knight")
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
    }
}

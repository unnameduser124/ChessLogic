

using ChessLogic.Board;
using ChessLogic.Pieces;
using System;
using System.Linq;

public class GlobalFunctions
{
    static public string FEN(Game game)
    {
        string fen = "";
        for (int i = 7; i >= 0; i--)
        {
            int freeSquareCounter = 0;
            for (int j = 0; j < 8; j++)
            {
                if (game.ChessBoard[j, i] == null)
                {
                    freeSquareCounter++;
                }
                else
                {
                    if (freeSquareCounter > 0)
                    {
                        fen += freeSquareCounter.ToString();
                        if (game.ChessBoard[j, i].Color.ToLower() == "white")
                        {
                            fen += game.ChessBoard[j, i].FENsymbol.ToUpper();
                        }
                        else
                        {
                            fen += game.ChessBoard[j, i].FENsymbol;
                        }
                        freeSquareCounter = 0;
                    }
                    else
                    {
                        if (game.ChessBoard[j, i].Color.ToLower() == "white")
                        {
                            fen += game.ChessBoard[j, i].FENsymbol.ToUpper();
                        }
                        else
                        {
                            fen += game.ChessBoard[j, i].FENsymbol;
                        }
                    }
                }
            }
            if (freeSquareCounter > 0)
            {
                fen += freeSquareCounter.ToString() + "/";
            }
            else
            {
                fen += "/";
            }
        }
        fen = fen.Remove(fen.Length - 1, 1);
        if (game.whiteTurn)
        {
            fen += " w ";
        }
        else
        {
            fen += " b ";
        }
        var whiteKing = game.findKing("white");
        var blackKing = game.findKing("black");
        if (whiteKing.Color.ToLower() != "none")
        {
            if (whiteKing.CastlingShort)
            {
                fen += "K";
            }
            if (whiteKing.CastlingLong)
            {
                fen += "Q";
            }
        }
        if (blackKing.Color.ToLower() != "none")
        {
            if (blackKing.CastlingShort)
            {
                fen += "k";
            }
            if (blackKing.CastlingLong)
            {
                fen += "q";
            }
        }


        if (!blackKing.CastlingLong && !blackKing.CastlingShort && !whiteKing.CastlingLong && !whiteKing.CastlingShort)
        {
            fen += "- ";
        }


        if (fen[fen.Length - 1] != ' ')
        {
            fen += " ";
        }

        var enPassant = false;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (game.ChessBoard[i, j] != null)
                {
                    if (game.ChessBoard[i, j].Name.ToLower() == "pawn")
                    {
                        var pawn = game.ChessBoard[i, j] as Pawn;
                        if (pawn.EnPassant)
                        {
                            if (pawn.Color.ToLower() == "white")
                            {
                                fen += $"{convertCordToChar(i)}{j}";
                            }
                            else
                            {
                                fen += $"{convertCordToChar(i)}{j + 2}";
                            }
                            enPassant = true;
                        }
                    }
                }
            }
        }

        if (fen[fen.Length - 1] != ' ')
        {
            fen += " ";
        }
        if (enPassant == false)
        {
            fen += "- ";
        }
        fen += $"{game.movesSinceLastCapture} ";
        fen += $"{game.turnCounter}";
        return fen;
    }

    static public string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    static public string convertCords(int[] cord)
    {
        return (char)(cord[0] + 97) + (cord[1] + 1).ToString();
    }

    static public char convertCordToChar(int cord)
    {
        return (char)(cord + 97);
    }

    static public int[] cordToIntArray(string cord)
    {
        var position = new int[2];
        position[0] = cord[0] - 97;
        position[1] = cord[1] - '0' - 1;

        return position;
    }

    public string generatePGN(Game game)
    {
        var turn = true;
        var PGNstring = "";
        foreach (var move in game.MoveHistory)
        {
            if (move.pieceCaptured == null)
            {
                if (turn == true)
                {
                    if (move.Check)
                    {
                        if (move.Castling && move.fromX == move.toX - 2)
                        {
                            PGNstring += $" {move.turnNumber}. O-O+";
                        }
                        else if (move.Castling && move.fromX == move.toX + 2)
                        {
                            PGNstring += $" {move.turnNumber}. O-O-O+";
                        }
                        else
                        {
                            if (move.pieceName != "")
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                            else
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                        }
                        turn = !turn;
                    }
                    else
                    {
                        if (move.Castling && move.fromX == move.toX - 2)
                        {
                            PGNstring += $" {move.turnNumber}. O-O";
                        }
                        else if (move.Castling && move.fromX == move.toX + 2)
                        {
                            PGNstring += $" {move.turnNumber}. O-O-O";
                        }
                        else
                        {
                            if (move.pieceName != "")
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
                            else
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
                        }
                        turn = !turn;
                    }
                }
                else
                {
                    if (move.Check)
                    {
                        if (move.Castling && move.fromX == move.toX - 2)
                        {
                            PGNstring += $" O-O+";
                        }
                        else if (move.Castling && move.fromX == move.toX + 2)
                        {
                            PGNstring += $" O-O-O+";
                        }
                        else
                        {
                            if (move.pieceName != "")
                            {
                                PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                            else
                            {
                                PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                        }
                        turn = !turn;
                    }
                    else
                    {
                        if (move.Castling && move.fromX == move.toX - 2)
                        {
                            PGNstring += $" O-O";
                        }
                        else if (move.Castling && move.fromX == move.toX + 2)
                        {
                            PGNstring += $" O-O-O";
                        }
                        else
                        {
                            if (move.pieceName != "")
                            {
                                PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
                            else
                            {
                                PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
                        }
                        turn = !turn;
                    }
                }
            }
            else
            {
                if (turn == true)
                {
                    if (move.pieceName == "")
                    {
                        if (move.Check)
                        {
                            PGNstring += $" {move.turnNumber}. {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            turn = !turn;
                        }
                        else
                        {
                            PGNstring += $" {move.turnNumber}. {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            turn = !turn;
                        }
                    }

                    else
                    {
                        if (move.Check)
                        {
                            PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                        }
                        else
                        {
                            PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                        }
                        turn = !turn;
                    }
                }
                else
                {
                    if (move.pieceName == "")
                    {
                        if (move.Check)
                        {
                            PGNstring += $" {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                        }
                        else
                        {
                            PGNstring += $" {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                        }
                        turn = !turn;
                    }
                    else
                    {
                        if (move.Check)
                        {
                            PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                        }
                        else
                        {
                            PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                        }
                        turn = !turn;
                    }
                }
            }
        }

        if (game.MoveHistory.Last().Checkmate)
        {
            PGNstring = PGNstring.Remove(PGNstring.Length - 1, 1);
            PGNstring += "#";
        }

        return PGNstring;
    }
}
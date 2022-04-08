

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
                        if (game.ChessBoard[j, i].Color == "white")
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
                        if (game.ChessBoard[j, i].Color == "white")
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
        if (whiteKing.Color != "none")
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
        if (blackKing.Color != "none")
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
                    if (game.ChessBoard[i, j].Name == "pawn")
                    {
                        var pawn = game.ChessBoard[i, j] as Pawn;
                        if (pawn.EnPassant)
                        {
                            if (pawn.Color == "white")
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

    static public int convertCharToCord(char cord)
    {
        return cord - 97;
    }

    static public int[] cordToIntArray(string cord)
    {
        var position = new int[2];
        position[0] = cord[0] - 97;
        position[1] = cord[1] - '0' - 1;

        return position;
    }

    static public string generatePGN(Game game)
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
                        else if (move.piecePromoted != null)
                        {
                            PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}+";
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
                        else if (move.piecePromoted != null)
                        {
                            PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}";
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
                        else if (move.piecePromoted != null)
                        {
                            PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}+";
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
                        else if (move.piecePromoted != null)
                        {
                            PGNstring += $" {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}";
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
                            if (move.piecePromoted != null)
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}+";
                            }
                            else
                            {
                                PGNstring += $" {move.turnNumber}. {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                            turn = !turn;
                        }
                        else
                        {

                            if (move.piecePromoted != null)
                            {
                                PGNstring += $" {move.turnNumber}. {move.pieceName}{GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}";
                            }
                            else
                            {
                                PGNstring += $" {move.turnNumber}. {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
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
                            if (move.piecePromoted != null)
                            {
                                PGNstring += $" {GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}+";
                            }
                            else
                            {
                                PGNstring += $" {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}+";
                            }
                        }
                        else
                        {
                            if (move.piecePromoted != null)
                            {
                                PGNstring += $" {GlobalFunctions.convertCords(new int[] { move.fromX, move.fromY })}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })} = {move.piecePromoted.NotationName}";
                            }
                            else
                            {
                                PGNstring += $" {GlobalFunctions.convertCordToChar(move.fromX)}x{GlobalFunctions.convertCords(new int[] { move.toX, move.toY })}";
                            }
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

    static public Game importPGN(string pgn)
    {
        var game = new Game();
        var newPgn = "";
        int lastBracketIndex = -1;
        int[] curlyBraces = new int[2] { 0, 0 };
        for(int i=0; i<pgn.Length; i++)
        {
            if (pgn[i] == ']')
            {
                lastBracketIndex = i;
            }
        }
        newPgn = pgn.Remove(0, lastBracketIndex+1);
        for(int i=0; i<newPgn.Length; i++)
        {
            if (newPgn[i] == '{')
            {
                curlyBraces[0] = i;
            }
            else if (newPgn[i] == '}')
            {
                curlyBraces[1] = i;
            }
        }
        if(curlyBraces[0]!=0 && curlyBraces[1] != 0)
        {
            newPgn = newPgn.Remove(curlyBraces[0], curlyBraces[1] - curlyBraces[0]+2);
        }
        newPgn = newPgn.Replace(".", ". ");
        string anotherPgn = "";
        string tempText = "";
        for(int i=0; i<newPgn.Length; i++)
        {
            if (i == newPgn.Length - 1)
            {
                tempText += newPgn[i];
                tempText += ' ';
                if (!tempText.Contains('.'))
                {
                    anotherPgn += tempText;
                    tempText = "";
                }
                else
                {
                    tempText = "";
                }
            }
            else if (newPgn[i]!=' ')
            {
                tempText += newPgn[i];
            }
            else
            {
                tempText += ' ';
                if (!tempText.Contains('.'))
                {
                    anotherPgn += tempText;
                    tempText = "";
                }
                else
                {
                    tempText = "";
                }
            }
        }
        newPgn = anotherPgn;
        newPgn = newPgn.Replace(" +", "+");
        newPgn = newPgn.Replace("  ", " ");

        int turnNumber = 1;
        for(int i=0; i<newPgn.Length; i++)
        {
            if (newPgn[i] != ' ')
            {
                tempText += newPgn[i];
            }
            else
            {
                if((tempText.Length == 2 && !tempText.Contains('+')) || (tempText.Length == 3 && tempText.Contains('+')))
                {
                    for(int column=0; column<8; column++)
                    {
                        for(int row = 0; row<8; row++)
                        {
                            if(game.ChessBoard[column, row] != null)
                            {
                                if(game.ChessBoard[column, row].Name == "pawn")
                                {
                                    var position = cordToIntArray(tempText.Substring(0, 2));
                                    foreach(var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if(game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8; 
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                }
                else if((tempText.Length == 3 && !tempText.Contains("-")) || (tempText.Length==4) && tempText.Contains("+") && !tempText.Contains("-"))
                {
                    for (int column = 0; column < 8; column++)
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == tempText[0].ToString())
                                {
                                    var position = cordToIntArray(tempText.Substring(1, 2));
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                else if(tempText.Length == 4 && tempText[0]>='A' && tempText[0] <= 'Z' && tempText.Contains("x"))
                {
                    for (int column = 0; column < 8; column++)
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == tempText[0].ToString())
                                {
                                    var position = cordToIntArray(tempText.Substring(2, 2));
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                else if((tempText.Length == 4 || tempText.Length == 5) && tempText[0]>='a' && tempText[0] <= 'h' && tempText.Contains("x"))
                {
                    var column = convertCharToCord(tempText[0]);
                    for (int row = 0; row < 8; row++)
                    {
                        if (game.ChessBoard[column, row] != null)
                        {
                            if (game.ChessBoard[column, row].NotationName == "")
                            {
                                var position = cordToIntArray(tempText.Substring(2, 2));
                                foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                {
                                    if (position.SequenceEqual(move))
                                    {
                                        if (game.movePiece(column, row, move[0], move[1]))
                                        {
                                            row = 8;
                                            column = 8;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                else if(tempText.Length == 5 && tempText[0]>='A' && tempText[0] <= 'H' && tempText.Contains("x"))
                {
                    for (int column = 0; column < 8; column++)
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == tempText[0].ToString())
                                {
                                    var position = cordToIntArray(tempText.Substring(2, 2));
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(tempText.Length == 3 && tempText.Contains("-"))
                {
                    for (int column = 0; column < 8; column++)
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == "K")
                                {
                                    var position = new int[] { 6, row } ;
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(tempText.Length == 5 && tempText.Contains("-"))
                {
                    for (int column = 0; column < 8; column++)
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == "K")
                                {
                                    var position = new int[] { 2, row } ;
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (game.movePiece(column, row, move[0], move[1]))
                                            {
                                                row = 8;
                                                column = 8;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(tempText.Contains("="))
                {
                    if (tempText.Length == 4 || (tempText.Length == 5 && tempText.Contains("+")))
                    {
                        for (int column = 0; column < 8; column++)
                        {
                            for (int row = 0; row < 8; row++)
                            {
                                if (game.ChessBoard[column, row] != null)
                                {
                                    if (game.ChessBoard[column, row].NotationName == "")
                                    {
                                        var position = cordToIntArray(tempText.Substring(0, 2));
                                        foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                        {
                                            if (position.SequenceEqual(move))
                                            {
                                                if (tempText[3] == 'Q')
                                                {
                                                    if (game.movePiece(column, row, move[0], move[1], "queen"))
                                                    {
                                                        row = 8;
                                                        column = 8;
                                                        break;
                                                    }
                                                }
                                                else if (tempText[3] == 'R')
                                                {
                                                    if (game.movePiece(column, row, move[0], move[1], "rook"))
                                                    {
                                                        row = 8;
                                                        column = 8;
                                                        break;
                                                    }
                                                }
                                                else if (tempText[3] == 'N')
                                                {
                                                    if (game.movePiece(column, row, move[0], move[1], "knight"))
                                                    {
                                                        row = 8;
                                                        column = 8;
                                                        break;
                                                    }
                                                }
                                                else if (tempText[3] == 'B')
                                                {
                                                    if (game.movePiece(column, row, move[0], move[1], "bishop"))
                                                    {
                                                        row = 8;
                                                        column = 8;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tempText.Length == 6 || (tempText.Length == 7 && tempText.Contains("+")))
                    {
                        var column = convertCharToCord(tempText[0]);
                        for (int row = 0; row < 8; row++)
                        {
                            if (game.ChessBoard[column, row] != null)
                            {
                                if (game.ChessBoard[column, row].NotationName == "")
                                {
                                    var position = cordToIntArray(tempText.Substring(2, 2));
                                    foreach (var move in game.ChessBoard[column, row].availableMoves(game))
                                    {
                                        if (position.SequenceEqual(move))
                                        {
                                            if (tempText[5] == 'Q')
                                            {
                                                if (game.movePiece(column, row, move[0], move[1], "queen"))
                                                {
                                                    row = 8;
                                                    column = 8;
                                                    break;
                                                }
                                            }
                                            else if (tempText[5] == 'R')
                                            {
                                                if (game.movePiece(column, row, move[0], move[1], "rook"))
                                                {
                                                    row = 8;
                                                    column = 8;
                                                    break;
                                                }
                                            }
                                            else if (tempText[5] == 'N')
                                            {
                                                if (game.movePiece(column, row, move[0], move[1], "knight"))
                                                {
                                                    row = 8;
                                                    column = 8;
                                                    break;
                                                }
                                            }
                                            else if (tempText[5] == 'B')
                                            {
                                                if (game.movePiece(column, row, move[0], move[1], "bishop"))
                                                {
                                                    row = 8;
                                                    column = 8;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                turnNumber++;
                tempText = "";
            }
        }
        Console.WriteLine(generatePGN(game));
        return game;
    }
}
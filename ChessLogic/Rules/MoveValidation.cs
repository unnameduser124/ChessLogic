using ChessLogic.Board;
using ChessLogic.Pieces;

namespace ChessLogic.Rules
{
    class MoveValidation
    {
        public static bool kingInCheckValidation(int fromX, int fromY, int toX, int toY, Game game)
        {
            if ((StatusCheck.checkwhite(game) && game.ChessBoard[toX, toY].Color == "white") || 
                (StatusCheck.checkblack(game) && game.ChessBoard[toX, toY].Color == "black"))
            {
                game.undoMove();
                if (game.ChessBoard[fromX, fromY].Name == "pawn")
                {
                    (game.ChessBoard[fromX, fromY] as Pawn).EnPassant = false;

                }
                SpecialMoves.enPassantCancel(game);
                return false;
            }
            return true;
        }

        public static bool pawnPromotionValidation(int toX, int toY, Game game)
        {
            if ((toY == 7 || toY == 0) && game.ChessBoard[toX, toY].Name == "pawn")
            {
                game.undoMove();
                SpecialMoves.enPassantCancel(game);
                return false;
            }
            return true;
        }
    }
}

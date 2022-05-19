using ChessLogic.Notation;
using ChessLogic.Pieces;

namespace ChessLogic.Board
{
    public class Move
    {
        public int fromX { get; set; }
        public int fromY { get; set; }
        public int toX { get; set; }
        public int toY { get; set; }

        public Piece pieceCaptured { get; set; }

        public Piece piecePromoted { get; set; }

        public string pieceName { get; set; }

        public int turnNumber { get; set; }

        public bool Castling { get; set; }

        public bool Check { get; set; }

        public bool Checkmate { get; set; }

        public Move(int fromX, int fromY, int toX, int toY, string pieceName, int turnNumber, Piece pieceCaptured = null, Piece piecePromoted = null)
        {
            this.fromX = fromX;
            this.fromY = fromY;
            this.toX = toX;
            this.toY = toY;
            this.pieceCaptured = pieceCaptured;
            this.piecePromoted = piecePromoted;
            this.pieceName = pieceName;
            this.turnNumber = turnNumber;
            Castling = false;
        }

        public override string ToString()
        {
            return $"MOVE NUMBER: {turnNumber} FROM: {CoordinateConversion.convertCords(new int [] {fromX, fromY})}, TO: {CoordinateConversion.convertCords(new int[] { toX, toY })}";
        }
    }
}

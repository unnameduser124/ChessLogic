using ChessLogic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Castling { get; set; }

        public Move(int fromX, int fromY, int toX, int toY, Piece pieceCaptured)
        {
            this.fromX = fromX;
            this.fromY = fromY;
            this.toX = toX;
            this.toY = toY;
            this.pieceCaptured = pieceCaptured;
            this.piecePromoted = null;
        }

        public Move(int fromX, int fromY, int toX, int toY)
        {
            this.fromX = fromX;
            this.fromY = fromY;
            this.toX = toX;
            this.toY = toY;
            this.pieceCaptured = null;
            this.piecePromoted = null;
        }
    }
}

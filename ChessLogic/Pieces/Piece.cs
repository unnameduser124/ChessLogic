﻿using System.Collections.Generic;

namespace ChessLogic.Pieces
{
    public interface Piece
    {
        string NotationName { get; set; }
        string Name { get; set; }
        string FENsymbol { get; set; }
        string Color { get; set; }

        List<int[]> availableMoves(Board.Game board);

        Piece copy();
    }
}

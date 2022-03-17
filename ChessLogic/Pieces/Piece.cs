using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Pieces
{
    public interface Piece
    {
        string NotationName { get; set; }
        string Name { get; set; }

        string Color { get; set; }

        List<int []> availableMoves(int x, int y, Board.Board board);
    }
}

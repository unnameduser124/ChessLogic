using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLogic.Board;

namespace ChessLogic
{
    class Program
    {
        static void Main(string[] args)
        {


            char convertCord(int cord)
            {
                return (char)(cord + 97);
            }
            var board = new Board.Board("test");

            void position()
            {
                Console.WriteLine("Postion:");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board.ChessBoard[j, i] != null)
                        {
                            var piece = board.ChessBoard[j, i];
                            Console.WriteLine($"{piece.NotationName}{convertCord(j)}{i + 1} {piece.Color}");
                        }
                    }
                }
            }

            void move(int fromX, int fromY, int toX, int toY)
            {
                position();

                Console.WriteLine("Check: " + board.check());

                Console.WriteLine("Move success: " + board.movePiece(fromX, fromY, toX, toY));
            }

            move(0, 6, 0, 4);
            move(1, 4, 0, 5);
            position();
            board.undoMove();
            position();
            move(2, 6, 3, 5);
            move(2, 6, 2, 5);
            move(7, 1, 7, 3);
            move(6, 3, 7, 2);
            position();
            board.undoMove();
            position();

            Console.ReadLine();
        }
    }
}

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

            void move(int fromX, int fromY, int toX, int toY, string promotion = "")
            {
                position();

                Console.WriteLine("Check: " + board.check());

                Console.WriteLine("Move success: " + board.movePiece(fromX, fromY, toX, toY, promotion));
            }
            move(0, 6, 0, 7, "queen");
            move(0, 6, 1, 7, "bishop");
            move(0, 7, 1, 7);
            position();
            board.undoMove();
            position();
            

            Console.ReadLine();
        }
    }
}

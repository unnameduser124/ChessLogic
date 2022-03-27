using System;

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
            move(0, 6, 0, 4);
            move(1, 4, 0, 5);
            move(1, 1, 1, 3);
            move(0, 3, 1, 2);
            position();


            Console.ReadLine();
        }
    }
}

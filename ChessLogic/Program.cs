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

            Console.WriteLine("Postion:");
            for(int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    if (board.ChessBoard[j, i] != null)
                    {
                        var piece = board.ChessBoard[j, i];
                        Console.WriteLine($"{piece.NotationName}{convertCord(j)}{i + 1} {piece.Color}");
                    }
                }
            }

            Console.WriteLine("Check: " + board.check());

            Console.WriteLine("Move success: " +  board.movePiece(1, 0, 1, 1));

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

            Console.WriteLine("Check: " + board.check());
            Console.WriteLine("Move success: " + board.movePiece(1, 0, 2, 0));

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


            Console.WriteLine("Check: " + board.check()); 
            Console.WriteLine("Move success: " + board.movePiece(2, 0, 6, 0));

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
            Console.WriteLine("Check: " + board.check());


            Console.ReadLine();
        }
    }
}

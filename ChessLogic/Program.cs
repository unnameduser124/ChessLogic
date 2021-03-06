using ChessLogic.Board;
using ChessLogic.Notation;
using System;
using System.Threading;

namespace ChessLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var game = new Game();
            gameGenerator(game);*/

            Console.ReadLine();
        }

        static void position(Game game)
        {
            Console.WriteLine("Postion:");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.ChessBoard[j, i] != null)
                    {
                        var piece = game.ChessBoard[j, i];
                        Console.WriteLine($"{piece.NotationName}{convertCord(j)}{i + 1}|{convertCord(piece.x)}{piece.y + 1} {piece.Color}");
                    }
                }
            }
        }

        static char convertCord(int cord)
        {
            return (char)(cord + 97);
        }

        static int[] generateRandomMove()
        {
            var randomX = 0;
            var randomY = 0;
            Random rnd = new Random();
            randomX = rnd.Next(9);
            randomY = rnd.Next(9);
            Thread.Sleep((randomX + 1));
            return new int[] { randomX, randomY };
        }
        static bool randomMove(Game game)
        {
            var move = generateRandomMove();
            var randomX = move[0];
            var randomY = move[1];
            if (randomX < 8 && randomX >= 0 && randomY < 8 && randomY >= 0)
            {
                if (game.ChessBoard[randomX, randomY] != null)
                {
                    if (game.ChessBoard[randomX, randomY].availableMoves(game).Count > 0)
                    {
                        int x = 0;
                        int y = 0;
                        var moveList = game.ChessBoard[randomX, randomY].availableMoves(game);
                        var random = new Random();
                        var moveItem = random.Next(moveList.Count);
                        x = moveList[moveItem][0];
                        y = moveList[moveItem][1];
                        if (!game.movePiece(randomX, randomY, x, y))
                        {
                            Console.Write($".");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine($"FEN: {NotationGenerator.FEN(game)}");
                        }
                    }
                }
            }
            return true;
        }

        static void gameGenerator(Game game)
        {
            Console.WriteLine(NotationGenerator.FEN(game));
            while (game.gameStatus == Board.Game.GameStatus.inProgress)
            {
                randomMove(game);
            }
            Console.WriteLine(game.gameStatus);
            Console.WriteLine(NotationGenerator.generatePGN(game));
        }
    }
}

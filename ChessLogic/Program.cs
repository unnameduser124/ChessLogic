using System;
using System.Threading;
using System.Threading.Tasks;

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
            var board = new Board.Game();

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
            int [] generateRandomMove()
            {
                var randomX = 0;
                var randomY = 0;
                Random rnd = new Random();
                randomX = rnd.Next(9);
                randomY = rnd.Next(9);
                //Console.Write(randomX);
                Thread.Sleep((randomX + 1));
                return new int[] { randomX, randomY };
            }
            bool randomMove()
            {
                var move = generateRandomMove();
                var randomX  = move[0];
                var randomY = move[1];
                if(randomX<8 && randomX>=0 && randomY<8 && randomY >= 0)
                {
                    if(board.ChessBoard[randomX, randomY] != null)
                    {
                        if(board.ChessBoard[randomX, randomY].availableMoves(board).Count > 0)
                        {
                            int x = 0;
                            int y = 0;
                            var moveList = board.ChessBoard[randomX, randomY].availableMoves(board);
                            var random = new Random();
                            var moveItem = random.Next(moveList.Count);
                            x = moveList[moveItem][0];
                            y = moveList[moveItem][1];
                            if (!board.movePiece(randomX, randomY, x, y))
                            {
                                Console.Write($".");
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            position();
            board.FEN();
            while (board.gameStatus == Board.Game.GameStatus.inProgress || board.turnCounter<51)
            {
                randomMove();
            }
            Console.WriteLine(board.gameStatus);
            board.FEN();
            Console.WriteLine(board.generatePGN());
            Console.ReadLine();
        }
    }
}

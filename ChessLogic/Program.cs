﻿using System;
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
            var board = new Board.Board("rook");

            for(int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    if (board.ChessBoard[j, i] != null)
                    {
                        var piece = board.ChessBoard[j, i];
                        Console.WriteLine($"{piece.NotationName}{convertCord(j)}{i + 1}");
                    }
                }
            }
            Console.WriteLine("Available moves: ");

            foreach(var move in board.ChessBoard[0,0].availableMoves(0, 0, board))
            {
                Console.WriteLine($"R{convertCord(move[0])}{move[1]+1}");
            }

            Console.ReadLine();
        }
    }
}
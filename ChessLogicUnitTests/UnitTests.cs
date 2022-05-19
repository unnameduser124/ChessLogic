using ChessLogic.Board;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessLogicUnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void PawnMoveOneSquareTestWhite()
        {
            Game game = new Game();
            var success = game.movePiece(3, 1, 3, 2);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void PawnMoveTwoSquaresTestWhite()
        {
            Game game = new Game();
            var success = game.movePiece(4, 1, 4, 3);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void EnPassantWhite()
        {
            Game game = new Game();
            game.movePiece(0, 1, 0, 3);
            game.movePiece(0, 6, 0, 5);
            game.movePiece(0, 3, 0, 4);
            game.movePiece(1, 6, 1, 4);
            var success = game.movePiece(0, 4, 1, 5);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void PawnMoveOneSquareTestBlack()
        {
            Game game = new Game();
            game.movePiece(0, 1, 0, 2);
            var success = game.movePiece(3, 6, 3, 5);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void PawnMoveTwoSquaresTestBlack()
        {
            Game game = new Game();
            game.movePiece(0, 1, 0, 2);
            var success = game.movePiece(4, 6, 4, 4);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void EnPassantBlack()
        {
            Game game = new Game();
            game.movePiece(0, 1, 0, 2);
            game.movePiece(0, 6, 0, 4);
            game.movePiece(2, 1, 2, 3);
            game.movePiece(0, 4, 0, 3);
            game.movePiece(1, 1, 1, 3);
            var success = game.movePiece(0, 3, 1, 2);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void castlingShortWhite()
        {
            Game game = new Game();

            game.movePiece(4, 1, 4, 3);
            game.movePiece(4, 6, 4, 4);
            game.movePiece(5, 0, 4, 1);
            game.movePiece(5, 7, 4, 6);
            game.movePiece(6, 0, 5, 2);
            game.movePiece(6, 7, 5, 5);
            var success = game.movePiece(4, 0, 6, 0);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void castlingLongWhite()
        {
            Game game = new Game();

            game.movePiece(3, 1, 3, 3);
            game.movePiece(3, 6, 3, 4);
            game.movePiece(2, 0, 5, 3);
            game.movePiece(2, 7, 5, 4);
            game.movePiece(1, 0, 2, 2);
            game.movePiece(1, 7, 2, 5);
            game.movePiece(3, 0, 3, 1);
            game.movePiece(3, 7, 3, 6);

            var success = game.movePiece(4, 0, 2, 0);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void castlingShortBlack()
        {
            Game game = new Game();

            game.movePiece(4, 1, 4, 3);
            game.movePiece(4, 6, 4, 4);
            game.movePiece(5, 0, 4, 1);
            game.movePiece(5, 7, 4, 6);
            game.movePiece(6, 0, 5, 2);
            game.movePiece(6, 7, 5, 5);
            game.movePiece(4, 0, 6, 0);
            var success = game.movePiece(4, 7, 6, 7);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void castlingLongBlack()
        {
            Game game = new Game();

            game.movePiece(3, 1, 3, 3);
            game.movePiece(3, 6, 3, 4);
            game.movePiece(2, 0, 5, 3);
            game.movePiece(2, 7, 5, 4);
            game.movePiece(1, 0, 2, 2);
            game.movePiece(1, 7, 2, 5);
            game.movePiece(3, 0, 3, 1);
            game.movePiece(3, 7, 3, 6);
            game.movePiece(4, 0, 2, 0);

            var success = game.movePiece(4, 7, 2, 7);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void repetitionDrawWhite()
        {
            Game game = new Game();

            game.movePiece(1, 0, 2, 2);
            game.movePiece(1, 7, 2, 5);
            game.movePiece(2, 2, 1, 0);
            game.movePiece(2, 5, 1, 7);
            game.movePiece(1, 0, 2, 2);
            game.movePiece(1, 7, 2, 5);
            game.movePiece(2, 2, 1, 0);
            game.movePiece(2, 5, 1, 7);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }
        [TestMethod]
        public void repetitionDrawBlack()
        {
            Game game = new Game();

            game.movePiece(4, 1, 4, 3);

            game.movePiece(1, 7, 2, 5);
            game.movePiece(1, 0, 2, 2);
            game.movePiece(2, 5, 1, 7);
            game.movePiece(2, 2, 1, 0);

            game.movePiece(1, 7, 2, 5);
            game.movePiece(1, 0, 2, 2);
            game.movePiece(2, 5, 1, 7);
            game.movePiece(2, 2, 1, 0);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }

        [TestMethod]
        public void checkMateWhite()
        {
            Game game = new Game("1k6/3R4/1K6/8/8/8/8/8 w - - 0 1");
            game.movePiece(3, 6, 3, 7);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.whiteWon);
        }

        [TestMethod]
        public void checkMateBlack()
        {
            Game game = new Game("1K6/3r4/1k6/8/8/8/8/8 b - - 0 1");
            game.movePiece(3, 6, 3, 7);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.blackWon);
        }

        [TestMethod]
        public void stalemateWhite()//white is stalemated
        {
            Game game = new Game("1K6/3r4/1k6/8/8/8/8/8 w - - 0 1");
            game.movePiece(1, 7, 0, 7);
            game.movePiece(3, 6, 1, 6);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }
        [TestMethod]
        public void stalemateBlack()//black is stalemated
        {
            Game game = new Game("1k6/3R4/1K6/8/8/8/8/8 b - - 0 1");
            game.movePiece(1, 7, 0, 7);
            game.movePiece(3, 6, 1, 6);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }

        [TestMethod]
        public void insufficientMaterialTwoKings()
        {
            Game game = new Game("2K5/3r4/1k6/8/8/8/8/8 w - - 0 1");

            game.movePiece(2, 7, 3, 6);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }

        [TestMethod]
        public void insufficientMaterialKingAndBishop()
        {
            Game game = new Game("2K5/3b4/1k6/8/8/8/8/8 w - - 0 1");

            game.movePiece(2, 7, 3, 7);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }
        [TestMethod]
        public void insufficientMaterialKingAndKnight()
        {
            Game game = new Game("2K5/3n4/1k6/8/8/8/8/8 w - - 0 1");

            game.movePiece(2, 7, 3, 7);

            Assert.AreEqual(game.gameStatus, Game.GameStatus.draw);
        }

        [TestMethod]

        public void pawnPromotionWhite()
        {
            Game game = new Game("1k6/7P/1K6/8/8/8/8/8 w - - 0 1");

            var moveSuccess = game.movePiece(7, 6, 7, 7, "queen");

            var success = moveSuccess && game.ChessBoard[7, 7].Name == "queen";

            Assert.IsTrue(success);
        }

        [TestMethod]

        public void pawnPromotionBlack()
        {
            Game game = new Game("8/8/1K6/8/8/8/1k5p/8 b - - 0 1");

            var moveSuccess = game.movePiece(7, 1, 7, 0, "knight");

            var success = moveSuccess && game.ChessBoard[7, 0].Name == "knight";

            Assert.IsTrue(success);
        }
    }
}

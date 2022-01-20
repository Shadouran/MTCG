using Moq;
using MTCG.Battle;
using MTCG.DAL;
using MTCG.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Test
{
    [TestFixture]
    class EloCalculationTests
    {
        [Test]
        public void CalculateWinElo1000Versus1000Under30Games()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 1, Wins = 1, Losses = 0 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 1, Wins = 0, Losses = 1 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(1020, gameManager.CalculateElo("playerA", "playerB", 1));
        }

        [Test]
        public void CalculateLossElo1000Versus1000Under30Games()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 1, Wins = 0, Losses = 1 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 1, Wins = 1, Losses = 0 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(980, gameManager.CalculateElo("playerA", "playerB", 0));
        }

        [Test]
        public void CalculateWinElo1000Versus1000Over30GamesEloLEQ2400()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 60, Wins = 35, Losses = 25 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 89, Wins = 45, Losses = 44 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(1010, gameManager.CalculateElo("playerA", "playerB", 1));
        }

        [Test]
        public void CalculateLossElo1000Versus1000Over30GamesEloLEQ2400()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 60, Wins = 35, Losses = 25 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 1000, GamesPlayed = 89, Wins = 45, Losses = 44 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(990, gameManager.CalculateElo("playerA", "playerB", 0));
        }

        [Test]
        public void CalculateWinElo2450Versus2476Over30GamesWithWinRateOver51Percent()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 2450, GamesPlayed = 115, Wins = 76, Losses = 39 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 2476, GamesPlayed = 200, Wins = 110, Losses = 90 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(2455, gameManager.CalculateElo("playerA", "playerB", 1));
        }

        [Test]
        public void CalculateLossElo2450Versus2476Over30GamesWithWinRateOver51Percent()
        {
            var cardRepo = new Mock<ICardRepository>();
            var cardPackageRepo = new Mock<ICardPackageRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var stackRepo = new Mock<IStackRepository>();
            var statsRepo = new Mock<IStatisticRepository>();
            var userRepo = new Mock<IUserRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var battleHandler = new Mock<IBattleHandler>();
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerA")).Returns(new Statistics() { ELO = 2450, GamesPlayed = 115, Wins = 76, Losses = 39 });
            statsRepo.Setup(repo => repo.SelectStatisticsByUsername("playerB")).Returns(new Statistics() { ELO = 2476, GamesPlayed = 200, Wins = 110, Losses = 90 });
            var gameManager = new GameManager(cardPackageRepo.Object, stackRepo.Object, userRepo.Object, cardRepo.Object, deckRepo.Object, statsRepo.Object, tradeRepo.Object, battleHandler.Object);

            Assert.AreEqual(2445, gameManager.CalculateElo("playerA", "playerB", 0));
        }
    }
}

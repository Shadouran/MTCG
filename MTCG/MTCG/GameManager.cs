using MTCG.Battle;
using MTCG.Core.Request;
using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    class GameManager : IGameManager
    {
        private readonly ICardPackageRepository CardPackageRepository;
        private readonly IStackRepository StackRepository;
        private readonly IUserRepository UserRepository;
        private readonly ICardRepository CardRepository;
        private readonly IDeckRepository DeckRepository;
        private readonly IStatisticRepository StatisticsRepository;
        private readonly ITradeRepository TradeRepository;

        private readonly IBattleHandler BattleHandler;

        public GameManager(ICardPackageRepository cardPackageRepository, IStackRepository stackRepository, IUserRepository userRepository, ICardRepository cardRepository, IDeckRepository deckRepository, IStatisticRepository statisticsRepository, ITradeRepository tradeRepository, IBattleHandler battleHandler)
        {
            CardPackageRepository = cardPackageRepository;
            StackRepository = stackRepository;
            UserRepository = userRepository;
            CardRepository = cardRepository;
            DeckRepository = deckRepository;
            StatisticsRepository = statisticsRepository;
            TradeRepository = tradeRepository;
            BattleHandler = battleHandler;
        }

        // User
        public User LoginUser(Credentials credentials)
        {
            var user = UserRepository.GetUserByCredentials(credentials.Username, credentials.Password);
            return user ?? throw new UserNotFoundException();
        }

        public void RegisterUser(Credentials credentials)
        {
            var user = new User()
            {
                Username = credentials.Username,
                Password = credentials.Password
            };
            if(!UserRepository.InsertUser(user))
            {
                throw new DuplicateUserException();
            }
            StatisticsRepository.InsertStatisticsByUsername(user.Username);
        }
        //-------
        // CardPackage
        public void AddCardPackage(ICollection<Card> cards, User user)
        {
            if(user.Username == "admin")
            {
                var cardPackage = new CardPackage()
                {
                    Cards = cards,
                    Uid = Guid.NewGuid()
                };
                CardRepository.InsertCards(cards);
                CardPackageRepository.InsertCardPackage(cardPackage);
            }
            else
                throw new UserNotAuthorizedException();
        }

        public CardPackage PurchaseFirstCardPackage(User user)
        {
            CardPackage cardPackage = CardPackageRepository.GetFirstCardPackage();
            if (cardPackage != null && UserRepository.UpdateCoinsByAuthToken(user.Token, -5) >= 0)
            {
                CardPackageRepository.RemoveCardPackage(cardPackage);
                StackRepository.InsertCards(user.Username, cardPackage.Cards);
            }
            else
                throw new Exception();
            return cardPackage;
        }
        //-------
        // Stack
        public ICollection<Card> GetStack(User user)
        {
            if (user.Token == null)
                throw new UserNotAuthorizedException();
            var cards = StackRepository.GetCardsByAuthToken(user.Token);
            return cards ?? throw new Exception();
        }
        //-------
        // Deck
        public ICollection<Card> GetDeck(User user)
        {
            if (user.Token == null)
                throw new UserNotAuthorizedException();
            var cards = DeckRepository.GetCardsByAuthToken(user.Token);
            return cards ?? throw new UnconfiguredDeckException();
        }

        public void SetDeck(User user, IEnumerable<string> cardIds)
        {
            if (user.Token == null)
                throw new UserNotAuthorizedException();
            if (cardIds.Count() != 4)
                throw new InvalidCardCountException("Deck does not have 4 cards");
            var cards = StackRepository.GetCardsByAuthToken(user.Token);
            var stackCardIds = cards.Select(card => card.Id).ToList();
            if (!cardIds.Except(stackCardIds).Any())
            {
                var cardIdInTrades = TradeRepository.SelectTradesByUsername(user.Username);
                if (cardIdInTrades != null && cardIdInTrades.Select(trade => trade.CardId).ToList().Intersect(cardIds).Any())
                    throw new InvalidCardException("Cannot set cards that are in a trade");
                DeckRepository.ResetDeck(user.Token);
                DeckRepository.InsertCardsByAuthToken(user.Token, cardIds);
            }
            else
                throw new UnavailableCardException("User does not own all cards");
        }
        //-------
        // User Profile
        public User GetUserProfile(User user, string username)
        {
            if (user.Username != username)
                throw new UserNotAuthorizedException("User is not authorized");
            var fetchedUser = UserRepository.GetUserProfileByAuthToken(user.Token);
            return fetchedUser ?? throw new UserNotFoundException("User does not exist");
        }

        public void UpdateUserProfile(User user, User userProfile, string username)
        {
            if(user.Username != username)
                throw new UserNotAuthorizedException("User is not authorized");
            UserRepository.SetUserProfileByAuthToken(user.Token, userProfile);
        }
        //-------
        // Stats
        public Statistics GetStatistics(User user)
        {
            Statistics stats = StatisticsRepository.SelectStatisticsByAuthToken(user.Token);
            return stats ?? throw new UserNotAuthorizedException();
        }

        public Dictionary<string, Statistics> GetScoreboard()
        {
            var stats = StatisticsRepository.SelectStatistics();
            return stats ?? throw new DataAccessFailedException();
        }
        //-------
        // Battle
        public string RegisterForBattle(User user)
        {
            BattleHandler.QueueUser(user);
            if(BattleHandler.GetUserNumberInQueue() >= 2)
            {
                var battleLog = BattleHandler.StartBattle(DeckRepository);
                StatisticsRepository.UpdateGamesPlayedByUsername(battleLog.GetPlayerA(), 1);
                StatisticsRepository.UpdateGamesPlayedByUsername(battleLog.GetPlayerB(), 1);
                if (battleLog.GetWinner() != null && battleLog.GetLoser() != null)
                    UpdateStatistics(battleLog.GetWinner(), battleLog.GetLoser());
                return battleLog.GetLog();
            }
            return string.Empty;
        }


        private void UpdateStatistics(string winner, string loser)
        {
            int eloWinner = CalculateElo(winner, loser, 1);
            StatisticsRepository.UpdateELOByUsername(winner, eloWinner);
            int eloLoser = CalculateElo(loser, winner, 0);
            StatisticsRepository.UpdateELOByUsername(loser, eloLoser);
            StatisticsRepository.UpdateWinsByUsername(winner, 1);
            StatisticsRepository.UpdateLossesByUsername(loser, 1);
        }

        private int CalculateElo(string playerA, string playerB, int achievedPoints)
        {
            var statisticsA = StatisticsRepository.SelectStatisticsByUsername(playerA);
            var statisticsB = StatisticsRepository.SelectStatisticsByUsername(playerB);
            var expectedScore = 1 / (1 + Math.Pow(10, (statisticsB.ELO - statisticsA.ELO) / 400));
            var k = 20;
            if (statisticsA.GamesPlayed <= 30)
                k = 40;
            else if (statisticsA.GamesPlayed > 30 && statisticsA.ELO > 2400)
                k = 10;
            return Convert.ToInt32(Math.Round(statisticsA.ELO + k * (achievedPoints - expectedScore)));
        }
        //-------
        // Trade
        public ICollection<Trade> GetTrades()
        {
            return TradeRepository.SelectTrades() ?? throw new NoTradesFoundException("No trades found");
        }

        public void PostTrade(string username, Trade trade)
        {
            trade.Username = username;
            if (DeckRepository.SelectCard(trade.CardId) != null)
                throw new InvalidCardException("Cannot trade cards that are in a deck");
            TradeRepository.InsertTrade(trade);
        }

        public void Trade(string username, string tradeId, string offerId)
        {
            Trade trade = TradeRepository.SelectTrade(tradeId);
            if(trade == null)
                throw new InvalidTradeException("Trade does not exist");
            if (trade.Username == username)
                throw new InvalidTradeException("You can not trade with yourself");
            if(DeckRepository.SelectCard(tradeId) != null)
                throw new InvalidCardException("Cannot trade cards that are in a deck");
            var offer = CardRepository.SelectCard(offerId);
            if(!ValidTrade(trade, offer))
                throw new InvalidTradeException("Trade failed");
            StackRepository.UpdateOwnerOfCard(trade.CardId, username);
            StackRepository.UpdateOwnerOfCard(offerId, trade.Username);
            TradeRepository.DeleteTrade(trade.Username, tradeId);
        }

        public void DeleteTrade(string username, string tradeId)
        {
            TradeRepository.DeleteTrade(username, tradeId);
        }

        private static bool ValidTrade(Trade trade, Card offer)
        {
            var validMinimumDamage = trade.MinimumDamage <= offer.Damage;
            var validElement = trade.Element == Element.None || trade.Element == offer.Element;
            var validCardType = false;
            var monsterTypes = Enum.GetNames(typeof(CardType)).Except(new List<string>() { "Spell", "None" });
            if(trade.CardType != CardType.Spell)
                foreach (var type in monsterTypes)
                    if (Enum.Parse<CardType>(type) == offer.CardType)
                    {
                        validCardType = true;
                        break;
                    }
            else if (offer.CardType == CardType.Spell)
                validCardType = true;

            return validCardType && validMinimumDamage && validElement;
        }
    }
}

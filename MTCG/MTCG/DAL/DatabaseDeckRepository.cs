using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    class DatabaseDeckRepository : IDeckRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS decks (username VARCHAR, cardid VARCHAR, PRIMARY KEY(username, cardid), CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username) ON DELETE CASCADE, CONSTRAINT fk_cardid FOREIGN KEY (cardid) REFERENCES cards(id) ON DELETE CASCADE)";
        private const string GetCardsByAuthTokenCommand = "SELECT c.* FROM cards c INNER JOIN decks d ON c.id = d.cardid INNER JOIN users u ON u.username = d.username WHERE u.token=@token";
        private const string InsertCardByAuthTokenCommand = "INSERT INTO decks(username, cardid) VALUES((SELECT username FROM users WHERE token=@token), @cardid)";
        private const string DeleteCardsByAuthTokenCommand = "DELETE FROM decks d USING users u WHERE u.username = d.username AND u.token=@token";
        private const string SelectCardByCardIdCommand = "SELECT c.* FROM cards c INNER JOIN decks d ON c.id = d.cardid WHERE c.id=@cardid";

        private readonly NpgsqlConnection _connection;
        

        public DatabaseDeckRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTable();
        }

        public ICollection<Card> GetCardsByAuthToken(string authToken)
        {
            using var cmd = new NpgsqlCommand(GetCardsByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();

            List<Card> cards = new();
            while (reader.Read())
            {
                var card = new Card()
                {
                    Id = reader.GetString(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Damage = reader.GetDouble(reader.GetOrdinal("damage")),
                    Element = Enum.Parse<Element>(reader.GetString(reader.GetOrdinal("element"))),
                    CardType = Enum.Parse<CardType>(reader.GetString(reader.GetOrdinal("cardtype")))
                };
                cards.Add(card);
            }
            reader.Close();
            DatabaseLock.Semaphore.Release();
            if (cards.Count < 0 || cards.Count > 4)
                return null;
            return cards;
        }

        public void InsertCardByAuthToken(string authToken, string cardIds)
        {
            using var cmd = new NpgsqlCommand(InsertCardByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("cardid", cardIds);
            cmd.Parameters.AddWithValue("token", authToken);
            cmd.ExecuteNonQuery();
        }

        public void InsertCardsByAuthToken(string authToken, IEnumerable<string> cardIds)
        {
            DatabaseLock.Semaphore.WaitOne();
            var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var cardId in cardIds)
                    InsertCardByAuthToken(authToken, cardId);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DatabaseLock.Semaphore.Release();
        }

        public void ResetDeck(string authToken)
        {
            using var cmd = new NpgsqlCommand(DeleteCardsByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public Card SelectCard(string cardId)
        {
            using var cmd = new NpgsqlCommand(SelectCardByCardIdCommand, _connection);
            cmd.Parameters.AddWithValue("cardid", cardId);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();

            Card card = null;
            if (reader.Read())
                card = new()
                {
                    Id = reader.GetString(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Damage = reader.GetDouble(reader.GetOrdinal("damage")),
                    Element = Enum.Parse<Element>(reader.GetString(reader.GetOrdinal("element"))),
                    CardType = Enum.Parse<CardType>(reader.GetString(reader.GetOrdinal("cardtype")))
                };
            reader.Close();
            DatabaseLock.Semaphore.Release();
            return card;
        }

        private void EnsureTable()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}

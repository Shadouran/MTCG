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
    class DatabaseStackRepository : IStackRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS stacks (username VARCHAR, cardid VARCHAR, PRIMARY KEY(username, cardid), CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username) ON DELETE CASCADE, CONSTRAINT fk_cardid FOREIGN KEY (cardid) REFERENCES cards(id) ON DELETE CASCADE)";
        private const string InsertCardCommand = "INSERT INTO stacks (username, cardid) VALUES (@username, @cardid)";
        private const string GetCardsByAuthTokenCommand = "SELECT c.* FROM cards c INNER JOIN stacks s ON c.id = s.cardid INNER JOIN users u ON u.username = s.username WHERE u.token=@token";
        private const string UpdateUsernameCommand = "UPDATE stacks SET username=@username WHERE cardid=@cardid";

        private readonly NpgsqlConnection _connection;
        

        public DatabaseStackRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTable();
        }

        private void EnsureTable()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public void DeleteCard(string username)
        {
            throw new NotImplementedException();
        }

        public void InsertCard(string username, Card card)
        {
            using var cmd = new NpgsqlCommand(InsertCardCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("cardid", card.Id);
            cmd.ExecuteNonQuery();
        }

        public void InsertCards(string username, ICollection<Card> cards)
        {
            DatabaseLock.Semaphore.WaitOne();
            var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var card in cards)
                    InsertCard(username, card);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DatabaseLock.Semaphore.Release();
        }

        public ICollection<Card> GetCardsByAuthToken(string authToken)
        {
            using var cmd = new NpgsqlCommand(GetCardsByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();

            List<Card> cards = new();
            while(reader.Read())
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
            if (cards.Count == 0)
                return null;
            return cards;
        }

        public void UpdateOwnerOfCard(string cardId, string username)
        {
            using var cmd = new NpgsqlCommand(UpdateUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("cardid", cardId);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }
    }
}

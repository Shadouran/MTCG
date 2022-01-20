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
    class DatabaseCardRepository : ICardRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS cards (id VARCHAR PRIMARY KEY, name VARCHAR, damage REAL, element VARCHAR, cardtype VARCHAR)";

        private const string InsertCardCommand = "INSERT INTO cards(id, name, damage, element, cardtype) VALUES (@id, @name, @damage, @element, @cardtype)";
        private const string SelectCardCommand = "SELECT * FROM cards WHERE id=@id";


        private readonly NpgsqlConnection _connection;
        

        public DatabaseCardRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public void InsertCard(Card card)
        {
            using var cmd = new NpgsqlCommand(InsertCardCommand, _connection);
            cmd.Parameters.AddWithValue("id", card.Id);
            cmd.Parameters.AddWithValue("name", card.Name);
            cmd.Parameters.AddWithValue("damage", card.Damage);
            cmd.Parameters.AddWithValue("element", card.Element.ToString());
            cmd.Parameters.AddWithValue("cardtype", card.CardType.ToString());
            cmd.ExecuteNonQuery();
        }

        public void InsertCards(ICollection<Card> cards)
        {
            DatabaseLock.Semaphore.WaitOne();
            var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var card in cards)
                    InsertCard(card);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DatabaseLock.Semaphore.Release();
        }

        public void RemoveCard(Card card)
        {
            throw new NotImplementedException();
        }

        public Card SelectCard(string offerId)
        {
            using var cmd = new NpgsqlCommand(SelectCardCommand, _connection);
            cmd.Parameters.AddWithValue("id", offerId);
            DatabaseLock.Semaphore.WaitOne();
            Card card = null;
            var reader = cmd.ExecuteReader();
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

        public void UpdateCard(Card card)
        {
            throw new NotImplementedException();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}

using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    class DatabaseTradeRepository : ITradeRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS trades (id VARCHAR PRIMARY KEY, username VARCHAR, cardid VARCHAR, cardtype VARCHAR, element VARCHAR, mindamage REAL, CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username), CONSTRAINT fk_cardid FOREIGN KEY (cardid) REFERENCES cards(id))";

        private const string SelectTradesCommand = "SELECT * FROM trades";
        private const string SelectTradeCommand = "SELECT * FROM trades WHERE id=@id";
        private const string SelectTradesByUsernameCommand = "SELECT * FROM trades WHERE username=@username";
        private const string InsertTradeCommand = "INSERT INTO trades (id, username, cardid, cardtype, element, mindamage) VALUES (@id, @username, @cardid, @cardtype, @element, @mindamage)";
        private const string DeleteTradeCommand = "DELETE FROM trades WHERE id=@id AND username=@username";

        private readonly NpgsqlConnection _connection;

        public DatabaseTradeRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTable();
        }

        public void DeleteTrade(string username, string tradeId)
        {
            using var cmd = new NpgsqlCommand(DeleteTradeCommand, _connection);
            cmd.Parameters.AddWithValue("id", tradeId);
            cmd.Parameters.AddWithValue("username", username);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public void InsertTrade(Trade trade)
        {
            using var cmd = new NpgsqlCommand(InsertTradeCommand, _connection);
            cmd.Parameters.AddWithValue("id", trade.Id);
            cmd.Parameters.AddWithValue("username", trade.Username);
            cmd.Parameters.AddWithValue("cardid", trade.CardId);
            cmd.Parameters.AddWithValue("cardtype", trade.CardType.ToString());
            cmd.Parameters.AddWithValue("element", trade.Element.ToString());
            cmd.Parameters.AddWithValue("mindamage", trade.MinimumDamage);
            if (SelectTrade(trade.Id) != null)
                throw new InvalidTradeException("Trade is already posted");
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public Trade SelectTrade(string tradeId)
        {
            using var cmd = new NpgsqlCommand(SelectTradeCommand, _connection);
            cmd.Parameters.AddWithValue("id", tradeId);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();
            Trade trade = null;
            if (reader.Read())
                trade = new()
                {
                    Id = Convert.ToString(reader.GetValue(reader.GetOrdinal("id"))),
                    Username = Convert.ToString(reader.GetValue(reader.GetOrdinal("username"))),
                    CardId = Convert.ToString(reader.GetValue(reader.GetOrdinal("cardid"))),
                    CardType = Enum.Parse<CardType>(Convert.ToString(reader.GetValue(reader.GetOrdinal("cardtype")))),
                    Element = Enum.Parse<Element>(Convert.ToString(reader.GetValue(reader.GetOrdinal("element")))),
                    MinimumDamage = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("mindamage")))
                };
            reader.Close();
            DatabaseLock.Semaphore.Release();
            return trade;
        }

        public ICollection<Trade> SelectTrades()
        {
            using var cmd = new NpgsqlCommand(SelectTradesCommand, _connection);
            DatabaseLock.Semaphore.WaitOne();
            List<Trade> trades = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                trades.Add(new Trade
                {
                    Id = Convert.ToString(reader.GetValue(reader.GetOrdinal("id"))),
                    Username = Convert.ToString(reader.GetValue(reader.GetOrdinal("username"))),
                    CardId = Convert.ToString(reader.GetValue(reader.GetOrdinal("cardid"))),
                    CardType = Enum.Parse<CardType>(Convert.ToString(reader.GetValue(reader.GetOrdinal("cardtype")))),
                    Element = Enum.Parse<Element>(Convert.ToString(reader.GetValue(reader.GetOrdinal("element")))),
                    MinimumDamage = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("mindamage")))
                });
            reader.Close();
            DatabaseLock.Semaphore.Release();
            if (trades.Count == 0)
                return null;
            return trades;
        }

        public ICollection<Trade> SelectTradesByUsername(string username)
        {
            using var cmd = new NpgsqlCommand(SelectTradesByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            DatabaseLock.Semaphore.WaitOne();
            List<Trade> trades = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                trades.Add(new Trade
                {
                    Id = Convert.ToString(reader.GetValue(reader.GetOrdinal("id"))),
                    Username = Convert.ToString(reader.GetValue(reader.GetOrdinal("username"))),
                    CardId = Convert.ToString(reader.GetValue(reader.GetOrdinal("cardid"))),
                    CardType = Enum.Parse<CardType>(Convert.ToString(reader.GetValue(reader.GetOrdinal("cardtype")))),
                    Element = Enum.Parse<Element>(Convert.ToString(reader.GetValue(reader.GetOrdinal("element")))),
                    MinimumDamage = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("mindamage")))
                });
            reader.Close();
            DatabaseLock.Semaphore.Release();
            if (trades.Count == 0)
                return null;
            return trades;
        }

        private void EnsureTable()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}

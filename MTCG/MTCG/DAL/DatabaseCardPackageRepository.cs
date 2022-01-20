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
    class DatabaseCardPackageRepository : ICardPackageRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS cardpackages(uid VARCHAR, cardid VARCHAR, PRIMARY KEY(uid, cardid), CONSTRAINT fk_id FOREIGN KEY (cardid) REFERENCES cards(id) ON DELETE CASCADE)";

        private const string InsertCardPackageCommand = "INSERT INTO cardpackages(uid, cardid) VALUES (@uid, @cardid)";
        private const string GetRandomCardPackageIdCommand = "SELECT uid, cardid FROM cardpackages WHERE uid=(SELECT uid FROM cardpackages ORDER BY random() LIMIT 1)";
        private const string GetFirstCardPackageIdCommand = "SELECT uid, cardid FROM cardpackages WHERE uid=(SELECT uid FROM cardpackages LIMIT 1)";
        private const string RemoveCardPackageCommand = "DELETE FROM cardpackages WHERE uid=@uid";

        private readonly NpgsqlConnection _connection;
        

        public DatabaseCardPackageRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public CardPackage GetRandomCardPackage()
        {
            using var cmd = new NpgsqlCommand(GetRandomCardPackageIdCommand, _connection);
            CardPackage cardPackage = null;
            DatabaseLock.Semaphore.WaitOne();
            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    List<Card> cards = new();
                    cardPackage = new();
                    cardPackage.Uid = new Guid(reader.GetString(reader.GetOrdinal("uid")));
                    cards.Add(new Card() { Id = reader.GetString(reader.GetOrdinal("cardid")) });
                    while (reader.Read())
                        cards.Add(new Card() { Id = reader.GetString(reader.GetOrdinal("cardid")) });
                    cardPackage.Cards = cards;
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            DatabaseLock.Semaphore.Release();
            return cardPackage;
        }

        public CardPackage GetFirstCardPackage()
        {
            using var cmd = new NpgsqlCommand(GetFirstCardPackageIdCommand, _connection);
            CardPackage cardPackage = null;
            DatabaseLock.Semaphore.WaitOne();
            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    List<Card> cards = new();
                    cardPackage = new();
                    cardPackage.Uid = new Guid(reader.GetString(reader.GetOrdinal("uid")));
                    cards.Add(new Card() { Id = reader.GetString(reader.GetOrdinal("cardid")) });
                    while (reader.Read())
                        cards.Add(new Card() { Id = reader.GetString(reader.GetOrdinal("cardid")) });
                    cardPackage.Cards = cards;
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            DatabaseLock.Semaphore.Release();
            return cardPackage;
        }

        public void InsertCardPackage(CardPackage cardPackage)
        {
            DatabaseLock.Semaphore.WaitOne();
            var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var card in cardPackage.Cards)
                {
                    using var cmd = new NpgsqlCommand(InsertCardPackageCommand, _connection);
                    cmd.Parameters.AddWithValue("uid", cardPackage.Uid.ToString());
                    cmd.Parameters.AddWithValue("cardid", card.Id);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DatabaseLock.Semaphore.Release();
        }

        public void RemoveCardPackage(CardPackage cardPackage)
        {
            using var cmd = new NpgsqlCommand(RemoveCardPackageCommand, _connection);
            cmd.Parameters.AddWithValue("uid", cardPackage.Uid.ToString());
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}

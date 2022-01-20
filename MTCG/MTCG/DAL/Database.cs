using MTCG.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    class Database
    {
        private const string ClearDatabaseCommand = "DROP SCHEMA PUBLIC CASCADE; CREATE SCHEMA PUBLIC;";

        private readonly NpgsqlConnection _connection;

        //public IMessageRepository MessageRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public ICardRepository CardRepository { get; private set; }
        public ICardPackageRepository CardPackageRepository { get; private set; }
        public IStackRepository StackRepository { get; private set; }
        public IDeckRepository DeckRepository { get; private set; }
        public IStatisticRepository StatisticRepository { get; private set; }
        public ITradeRepository TradeRepository { get; private set; }

        public Database(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();

                ClearDatabase();
                UserRepository = new DatabaseUserRepository(_connection);
                CardRepository = new DatabaseCardRepository(_connection);
                CardPackageRepository = new DatabaseCardPackageRepository(_connection);
                StackRepository = new DatabaseStackRepository(_connection);
                DeckRepository = new DatabaseDeckRepository(_connection);
                TradeRepository = new DatabaseTradeRepository(_connection);
                StatisticRepository = new DatabaseStatisticRepository(_connection);

            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }

        private void ClearDatabase()
        {
            using var cmd = new NpgsqlCommand(ClearDatabaseCommand, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}

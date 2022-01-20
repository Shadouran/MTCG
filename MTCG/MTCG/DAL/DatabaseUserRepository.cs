using Npgsql;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MTCG.DAL
{
    class DatabaseUserRepository : IUserRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS users (username VARCHAR PRIMARY KEY, password VARCHAR, token VARCHAR, coins SMALLINT DEFAULT 20, name VARCHAR, bio VARCHAR, image VARCHAR)";

        private const string InsertUserCommand = "INSERT INTO users(username, password, token) VALUES (@username, @password, @token)";
        private const string SelectUserByTokenCommand = "SELECT username, password FROM users WHERE token=@token";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";
        private const string SelectUserProfileByTokenCommand = "SELECT name, bio, image FROM users WHERE token=@token";
        private const string UpdateCoinsCommand = "UPDATE users SET coins=(SELECT coins FROM users WHERE token=@token)+@coins WHERE token=@token RETURNING coins";
        private const string UpdateUserProfileByAuthTokenCommand = "UPDATE users SET name=@name, bio=@bio, image=@image WHERE token=@token";

        private readonly NpgsqlConnection _connection;

        public DatabaseUserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public int UpdateCoinsByAuthToken(string authToken, int coins)
        {
            using var cmd = new NpgsqlCommand(UpdateCoinsCommand, _connection);
            cmd.Parameters.AddWithValue("coins", coins);
            cmd.Parameters.AddWithValue("token", authToken);
            DatabaseLock.Semaphore.WaitOne();
            var transaction = _connection.BeginTransaction();
            var result = cmd.ExecuteScalar();

            if(Convert.ToInt32(result) >= 0)
            {
                transaction.Commit();
                DatabaseLock.Semaphore.Release();
                return Convert.ToInt32(result);
            }
            transaction.Rollback();
            DatabaseLock.Semaphore.Release();
            return Convert.ToInt32(result);
        }

        public User GetUserByAuthToken(string authToken)
        {
            User user = null;
            using var cmd = new NpgsqlCommand(SelectUserByTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);

            DatabaseLock.Semaphore.WaitOne();
            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    user = ReadUser(reader);
                reader.Close();
            }
            catch (Exception)
            {
            }
            DatabaseLock.Semaphore.Release();

            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            User user = null;
            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            DatabaseLock.Semaphore.WaitOne();
            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    user = ReadUser(reader);
                reader.Close();
            }
            catch (Exception)
            {

            }
            DatabaseLock.Semaphore.Release();

            return user;
        }

        public User GetUserProfileByAuthToken(string authToken)
        {
            User user = null;
            using var cmd = new NpgsqlCommand(SelectUserProfileByTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);

            DatabaseLock.Semaphore.WaitOne();
            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    user = new User
                    {
                        Name = reader.GetValue(reader.GetOrdinal("name")).ToString(),
                        Bio = reader.GetValue(reader.GetOrdinal("bio")).ToString(),
                        Image = reader.GetValue(reader.GetOrdinal("image")).ToString()
                    };
                reader.Close();
            }
            catch (Exception)
            {
            }
            DatabaseLock.Semaphore.Release();

            return user;
        }

        public bool InsertUser(User user)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertUserCommand, _connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("token", user.Token);
                DatabaseLock.Semaphore.WaitOne();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            DatabaseLock.Semaphore.Release();
            return affectedRows > 0;
        }

        public void SetUserProfileByAuthToken(string authToken, User userProfile)
        {
            using var cmd = new NpgsqlCommand(UpdateUserProfileByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);
            cmd.Parameters.AddWithValue("name", userProfile.Name);
            cmd.Parameters.AddWithValue("bio", userProfile.Bio);
            cmd.Parameters.AddWithValue("image", userProfile.Image);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        private User ReadUser(IDataRecord record)
        {
            var user = new User
            {
                Username = Convert.ToString(record["username"]),
                Password = Convert.ToString(record["password"])
            };
            return user;
        }
    }
}

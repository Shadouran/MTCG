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
    class DatabaseStatisticRepository : IStatisticRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS statistics (username VARCHAR PRIMARY KEY, elo INT DEFAULT 1000, wins INT DEFAULT 0, losses INT DEFAULT 0, gamesplayed INT DEFAULT 0, CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username))";
        private const string InsertStatisticsByUsernameCommand = "INSERT INTO statistics (username) VALUES (@username)";
        private const string UpdateELOByUsernameCommand = "UPDATE statistics SET elo=(SELECT elo FROM statistics WHERE username=@username)+@elo WHERE username=@username";
        private const string UpdateWinsByUsernameCommand = "UPDATE statistics SET wins=(SELECT wins FROM statistics WHERE username=@username)+@wins WHERE username=@username";
        private const string UpdateLossesByUsernameCommand = "UPDATE statistics SET losses=(SELECT losses FROM statistics WHERE username=@username)+@losses WHERE username=@username";
        private const string UpdateGamesPlayedByUsernameCommand = "UPDATE statistics SET gamesplayed=(SELECT gamesplayed FROM statistics WHERE username=@username)+@gamesplayed WHERE username=@username";
        private const string SelectStatisticsByAuthTokenCommand = "SELECT * FROM statistics s INNER JOIN users u ON u.username = s.username WHERE u.token=@token";
        private const string SelectStatisticsByUsernameCommand = "SELECT * FROM statistics WHERE username=@username";
        private const string SelectStatisticsCommand = "SELECT * FROM statistics WHERE username NOT IN ('admin') ORDER BY elo DESC";

        private NpgsqlConnection _connection;
        

        public DatabaseStatisticRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTable();
        }

        public Statistics SelectStatisticsByAuthToken(string authToken)
        {
            Statistics stats = null;
            using var cmd = new NpgsqlCommand(SelectStatisticsByAuthTokenCommand, _connection);
            cmd.Parameters.AddWithValue("token", authToken);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
                stats = new()
                {
                    ELO = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("elo"))),
                    Wins = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("wins"))),
                    Losses = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("losses"))),
                    GamesPlayed = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("gamesplayed")))
                };
            reader.Close();
            DatabaseLock.Semaphore.Release();
            return stats;
        }

        public void InsertStatisticsByUsername(string username)
        {
            using var cmd = new NpgsqlCommand(InsertStatisticsByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        private void EnsureTable()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public Dictionary<string, Statistics> SelectStatistics()
        {
            using var cmd = new NpgsqlCommand(SelectStatisticsCommand, _connection);
            Dictionary<string, Statistics> stats = new();
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                stats.Add(
                    Convert.ToString(reader.GetValue(reader.GetOrdinal("username"))),
                    new Statistics
                    {
                        ELO = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("elo"))),
                        Wins = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("wins"))),
                        Losses = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("losses"))),
                        GamesPlayed = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("gamesplayed")))
                    });
            reader.Close();
            DatabaseLock.Semaphore.Release();
            if (stats.Count < 0)
                return null;
            return stats;
        }

        public void UpdateELOByUsername(string username, int elo)
        {
            using var cmd = new NpgsqlCommand(UpdateELOByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("elo", elo);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public void UpdateWinsByUsername(string username, int wins)
        {
            using var cmd = new NpgsqlCommand(UpdateWinsByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("wins", wins);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public void UpdateLossesByUsername(string username, int losses)
        {
            using var cmd = new NpgsqlCommand(UpdateLossesByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("losses", losses);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public void UpdateGamesPlayedByUsername(string username, int gamesPlayed)
        {
            using var cmd = new NpgsqlCommand(UpdateGamesPlayedByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("gamesPlayed", gamesPlayed);
            DatabaseLock.Semaphore.WaitOne();
            cmd.ExecuteNonQuery();
            DatabaseLock.Semaphore.Release();
        }

        public Statistics SelectStatisticsByUsername(string username)
        {
            Statistics stats = null;
            using var cmd = new NpgsqlCommand(SelectStatisticsByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("username", username);
            DatabaseLock.Semaphore.WaitOne();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
                stats = new()
                {
                    ELO = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("elo"))),
                    Wins = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("wins"))),
                    Losses = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("losses"))),
                    GamesPlayed = Convert.ToUInt32(reader.GetValue(reader.GetOrdinal("gamesplayed")))
                };
            reader.Close();
            DatabaseLock.Semaphore.Release();
            return stats;
        }
    }
}

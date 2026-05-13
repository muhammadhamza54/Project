using System;
using System.Threading;
using StackExchange.Redis;
using Npgsql;
using System.Text.Json;

namespace Worker {
    class Program {
        static void Main(string[] args) {
            // Resolves connection strings via internal cluster Service DNS records
            var redis = ConnectionMultiplexer.Connect("redis").GetDatabase();
            var pgConn = "Host=db;Username=postgres;Password=postgrespassword;Database=postgres";

            while (true) {
                try {
                    string voteData = redis.ListLeftPop("votes");
                    if (voteData != null) {
                        var json = JsonDocument.Parse(voteData).RootElement;
                        string voterId = json.GetProperty("voter_id").GetString();
                        string vote = json.GetProperty("vote").GetString();

                        using var conn = new NpgsqlConnection(pgConn);
                        conn.Open();
                        using var cmd = new NpgsqlCommand("INSERT INTO votes (id, vote) VALUES (@id, @v) ON CONFLICT (id) DO UPDATE SET vote = @v", conn);
                        cmd.Parameters.AddWithValue("id", voterId);
                        cmd.Parameters.AddWithValue("v", vote);
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception ex) { Console.WriteLine(ex.Message); }
                Thread.Sleep(100);
            }
        }
    }
}

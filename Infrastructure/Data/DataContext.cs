using System;
using Npgsql;

namespace Infrastructure.Data;

public class DataContext
{
    private const string ConnectionString = "Server=localhost;Database=Exam15; Port=5433;Username=postgres;Password=m.613524;";

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}

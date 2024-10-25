using MySql.Data.MySqlClient;
using System.Data;

public class MySQLDbContext
{
    private readonly string _connectionString;

    public MySQLDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}


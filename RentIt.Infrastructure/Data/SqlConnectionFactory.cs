using Npgsql;
using RentIt.Application.Abstractions.Data;
using System.Data;

namespace RentIt.Infrastructure.Data;
internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string m_ConnectionString;

    public SqlConnectionFactory(string connectionString)
    {
        m_ConnectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(m_ConnectionString);
        connection.Open();

        return connection;
    }
}

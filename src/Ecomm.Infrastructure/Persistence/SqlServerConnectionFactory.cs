using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Ecomm.Application.Abstractions.Persistence;

namespace Ecomm.Infrastructure.Persistence;

/// <summary>
/// Reads the "Default" connection string from configuration (appsettings.json, user secrets,
/// environment variables, ...) and opens a connection for this app's one real database.
/// </summary>
public sealed class SqlServerConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() =>
        new SqlConnection(configuration.GetConnectionString("Default"));
}

using System.Data;

namespace FamilyFeet.Api.Infrastructure;

public interface IDbConnectionFactory
{
    /// <summary>Creates a new (closed) connection. The caller must dispose it.</summary>
    IDbConnection CreateConnection();
}

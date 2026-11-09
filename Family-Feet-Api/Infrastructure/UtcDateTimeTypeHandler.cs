using System.Data;
using Dapper;

namespace FamilyFeet.Api.Infrastructure;

/// <summary>
/// SQL Server returns DateTime values with Kind = Unspecified. This handler marks them as UTC
/// so they are serialized with the "Z" suffix (e.g. 2027-01-15T10:30:00Z).
/// </summary>
public sealed class UtcDateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value) => DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.DbType = DbType.DateTime;
        parameter.Value = value;
    }
}

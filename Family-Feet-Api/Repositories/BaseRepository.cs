using System.Data;
using Dapper;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Infrastructure;

namespace FamilyFeet.Api.Repositories;

public abstract class BaseRepository
{
    protected BaseRepository(IDbConnectionFactory connectionFactory) => ConnectionFactory = connectionFactory;

    protected IDbConnectionFactory ConnectionFactory { get; }

    protected static DynamicParameters CreateParameters()
    {
        var parameters = new DynamicParameters();
        parameters.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("MSG", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
        parameters.Add("COUNTER", dbType: DbType.Int32, direction: ParameterDirection.Output);
        return parameters;
    }

    protected static Response<T> BuildResponse<T>(DynamicParameters parameters, T? data) => new()
    {
        Result = parameters.Get<int?>("RESULT") ?? ResultCode.ServerError,
        Msg = parameters.Get<string?>("MSG") ?? string.Empty,
        Count = parameters.Get<int?>("COUNTER") ?? 0,
        Data = data
    };

    protected static CommandDefinition StoredProcedure(string name, DynamicParameters parameters, CancellationToken ct) =>
        new(name, parameters, commandType: CommandType.StoredProcedure, cancellationToken: ct);

    /// <summary>Executes a SP that returns one row (or none).</summary>
    protected async Task<Response<T>> QuerySingleAsync<T>(string procedure, DynamicParameters parameters, CancellationToken ct)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<T>(StoredProcedure(procedure, parameters, ct));
        return BuildResponse(parameters, rows.FirstOrDefault());
    }

    /// <summary>Executes a SP that returns a list. Count = total records.</summary>
    protected async Task<Response<IEnumerable<T>>> QueryListAsync<T>(string procedure, DynamicParameters parameters, CancellationToken ct)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<T>(StoredProcedure(procedure, parameters, ct));
        return BuildResponse<IEnumerable<T>>(parameters, rows.ToList());
    }

    /// <summary>Executes a SP that returns no rows (delete). Data = true when Result is 1.</summary>
    protected async Task<Response<bool>> ExecuteNonQueryAsync(string procedure, DynamicParameters parameters, CancellationToken ct)
    {
        using var connection = ConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(StoredProcedure(procedure, parameters, ct));

        var response = BuildResponse<bool>(parameters, false);
        response.Data = response.IsSuccess;
        return response;
    }
}
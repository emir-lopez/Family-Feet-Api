using System.Data;
using Dapper;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.User;
using FamilyFeet.Api.Infrastructure;
using FamilyFeet.Api.Repositories.Interface;

namespace FamilyFeet.Api.Repositories;

public sealed class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Response<UserDtoGet>> LoginAsync(UserDtoLogin dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters(); // Crea @RESULT, @MSG, @COUNTER automáticamente por tu BaseRepository
        parameters.Add("Email", dto.Email, DbType.String);
        parameters.Add("PasswordHash", dto.Password, DbType.String);

        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<UserDtoGet>(
            StoredProcedure("dbo.Login_User", parameters, ct));

        return BuildResponse(parameters, rows.FirstOrDefault());
    }
}
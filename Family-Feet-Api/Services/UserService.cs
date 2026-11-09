using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.User;
using FamilyFeet.Api.Repositories.Interface;
using FamilyFeet.Api.Services.Interface;
using Microsoft.Extensions.Logging;

namespace FamilyFeet.Api.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Response<UserDtoGet>> LoginAsync(UserDtoLogin dto, CancellationToken ct = default)
    {
        // Validaciones básicas antes de ir a base de datos
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return Response<UserDtoGet>.Fail(ResultCode.BadRequest, "Email and Password are required.");
        }

        var response = await _repository.LoginAsync(dto, ct);

        if (response.Result == ResultCode.ServerError)
        {
            _logger.LogError("Operation Login failed in the database: {Message}", response.Msg);
        }

        return response;
    }
}
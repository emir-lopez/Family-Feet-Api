using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.User;

namespace FamilyFeet.Api.Services.Interface;

public interface IUserService
{
    Task<Response<UserDtoGet>> LoginAsync(UserDtoLogin dto, CancellationToken ct = default);
}
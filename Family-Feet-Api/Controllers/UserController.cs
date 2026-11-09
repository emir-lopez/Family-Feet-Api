using FamilyFeet.Api.Dtos.User;
using FamilyFeet.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Controllers;

[Route("api/[controller]")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDtoLogin dto, CancellationToken ct)
    {
        var response = await _userService.LoginAsync(dto, ct);

        return Ok(response);
    }
}
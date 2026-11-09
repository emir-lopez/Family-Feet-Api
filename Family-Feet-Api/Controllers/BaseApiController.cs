using FamilyFeet.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Controllers;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(Response<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Translates the Result returned by the stored procedure into the matching HTTP status code:
    /// 1 -> 200, 0 -> 400, 2 -> 404, 3 -> 409, anything else -> 500.
    /// </summary>
    protected IActionResult ToActionResult<T>(Response<T> response) =>
        StatusCode(ResultCode.ToHttpStatus(response.Result), response);
}

namespace FamilyFeet.Api.Common;

/// <summary>
/// Values returned by the stored procedures through the @RESULT output parameter.
/// They are also mapped to HTTP status codes by the API.
/// </summary>
public static class ResultCode
{
    public const int ServerError = -1;
    public const int BadRequest = 0;
    public const int Success = 1;
    public const int NotFound = 2;
    public const int Conflict = 3;

    public static int ToHttpStatus(int resultCode) => resultCode switch
    {
        Success => StatusCodes.Status200OK,
        BadRequest => StatusCodes.Status400BadRequest,
        NotFound => StatusCodes.Status404NotFound,
        Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    public static int FromHttpStatus(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => NotFound,
        StatusCodes.Status409Conflict => Conflict,
        >= 500 => ServerError,
        _ => BadRequest
    };
}

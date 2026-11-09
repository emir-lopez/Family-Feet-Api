using FamilyFeet.Api.Common;

namespace FamilyFeet.Api.Services;

public abstract class BaseService
{
    private readonly ILogger _logger;

    protected BaseService(ILogger logger) => _logger = logger;

    protected Response<T> Log<T>(Response<T> response, string operation)
    {
        if (response.Result == ResultCode.ServerError)
        {
            _logger.LogError("Operation {Operation} failed in the database: {Message}", operation, response.Msg);
        }

        return response;
    }
}
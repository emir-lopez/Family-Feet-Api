using System.Text.Json.Serialization;
using FamilyFeet.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Extension;

public static class ControllerExtension
{
    public static IServiceCollection AddApiController(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                // Enums travel as text ("Scheduled") instead of numbers.
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Validation errors use the same Response envelope as everything else.
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(entry => entry.Value?.Errors.Count > 0)
                        .ToDictionary(
                            entry => entry.Key,
                            entry => entry.Value!.Errors
                                .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "The value is invalid." : error.ErrorMessage)
                                .ToArray());

                    var response = new Response<Dictionary<string, string[]>>
                    {
                        Result = ResultCode.BadRequest,
                        Msg = "One or more validation errors occurred.",
                        Data = errors,
                        Count = errors.Count
                    };

                    return new BadRequestObjectResult(response);
                };
            });

        return services;
    }
}

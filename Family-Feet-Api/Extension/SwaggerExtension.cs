using System.Reflection;
using Microsoft.OpenApi.Models;

namespace FamilyFeet.Api.Extension;

public static class SwaggerExtension
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Family Feet API",
                Version = "v1",
                Description = "REST API for the Family Feet clinic appointments. All data access goes through SQL Server stored procedures."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}

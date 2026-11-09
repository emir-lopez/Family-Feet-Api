using FamilyFeet.Api.Extension;
using FamilyFeet.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// 1. Configurar política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services
    .AddApiController()
    .AddApiSwagger()
    .AddInfrastructure()
    .AddRepositoryLayer()
    .AddServiceLayer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Family Feet API v1");
        options.DocumentTitle = "Family Feet API";
    });

    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseHttpsRedirection();

// 2. ORDEN CORRECTO: Routing -> CORS -> Authorization
app.UseRouting();
app.UseCors("PermitirFrontend");
app.UseAuthorization();

app.MapControllers();
//app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
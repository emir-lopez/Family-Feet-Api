using FamilyFeet.Api.Infrastructure;
using FamilyFeet.Api.Repositories;
using FamilyFeet.Api.Repositories.Interface;
using FamilyFeet.Api.Services;
using FamilyFeet.Api.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyFeet.Api.Extension;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        return services;
    }

    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
    {
        // Repositorios registrados para inyección de dependencias
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();


        return services;
    }

    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        // Servicios registrados para inyección de dependencias
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();

        return services;
    }
}
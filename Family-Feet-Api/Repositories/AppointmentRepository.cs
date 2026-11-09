using System.Data;
using Dapper;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Appointment;
using FamilyFeet.Api.Infrastructure;
using FamilyFeet.Api.Repositories.Interface;

namespace FamilyFeet.Api.Repositories;

public sealed class AppointmentRepository : BaseRepository, IAppointmentRepository
{
    public AppointmentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Response<AppointmentDtoGet>> CreateAsync(AppointmentDtoPost dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();                                   // @RESULT, @MSG, @COUNTER (OUTPUT)
        parameters.Add("IdPatient", dto.IdPatient, DbType.Int32);
        parameters.Add("IdDoctor", dto.IdDoctor, DbType.Int32);
        parameters.Add("AppointmentDate", dto.AppointmentDate, DbType.DateTime);
        parameters.Add("Reason", dto.Reason, DbType.String);

        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<AppointmentDtoGet>(
            StoredProcedure("dbo.Create_Appointment", parameters, ct));

        // The OUTPUT values are available once the result set has been fully read.
        return BuildResponse(parameters, rows.FirstOrDefault());
    }

    public async Task<Response<AppointmentDtoGet>> UpdateAsync(int appointmentId, AppointmentDtoPut dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", appointmentId, DbType.Int32);
        parameters.Add("AppointmentDate", dto.AppointmentDate, DbType.DateTime);
        parameters.Add("Reason", dto.Reason, DbType.String);

        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<AppointmentDtoGet>(
            StoredProcedure("dbo.Update_Appointment", parameters, ct));

        return BuildResponse(parameters, rows.FirstOrDefault());
    }

    public async Task<Response<bool>> DeleteAsync(int appointmentId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", appointmentId, DbType.Int32);

        using var connection = ConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(StoredProcedure("dbo.Delete_Appointment", parameters, ct));

        var response = BuildResponse<bool>(parameters, false);
        response.Data = response.IsSuccess;
        return response;
    }

    public async Task<Response<AppointmentDtoGet>> GetByIdAsync(int appointmentId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", appointmentId, DbType.Int32);

        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<AppointmentDtoGet>(
            StoredProcedure("dbo.Get_Appointment_ById", parameters, ct));

        return BuildResponse(parameters, rows.FirstOrDefault());
    }

    public async Task<Response<IEnumerable<AppointmentDtoGet>>> GetPaginatedAsync(AppointmentDtoFilter filter, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("PageNumber", filter.PageNumber, DbType.Int32);
        parameters.Add("PageSize", filter.PageSize, DbType.Int32);
        parameters.Add("IdDoctor", filter.IdDoctor, DbType.Int32);

        using var connection = ConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<AppointmentDtoGet>(
            StoredProcedure("dbo.Get_Appointment_Paginated", parameters, ct));

        // @COUNTER carries the TOTAL number of records (not just the current page).
        return BuildResponse<IEnumerable<AppointmentDtoGet>>(parameters, rows.ToList());
    }
}

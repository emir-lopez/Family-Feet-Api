using System.Data;
using Dapper;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Patient;
using FamilyFeet.Api.Infrastructure;
using FamilyFeet.Api.Repositories.Interface;

namespace FamilyFeet.Api.Repositories;

public sealed class PatientRepository : BaseRepository, IPatientRepository
{
    public PatientRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public Task<Response<IEnumerable<PatientDtoGet>>> GetPaginatedAsync(PatientDtoFilter filter, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("PageNumber", filter.PageNumber, DbType.Int32);
        parameters.Add("PageSize", filter.PageSize, DbType.Int32);
        parameters.Add("Search", string.IsNullOrWhiteSpace(filter.Search) ? null : filter.Search.Trim(), DbType.String, size: 100);

        return QueryListAsync<PatientDtoGet>("dbo.Get_Patient_Paginated", parameters, ct);
    }

    public Task<Response<PatientDtoGet>> GetByIdAsync(int patientId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", patientId, DbType.Int32);

        return QuerySingleAsync<PatientDtoGet>("dbo.Get_Patient_ById", parameters, ct);
    }

    public Task<Response<PatientDtoGet>> CreateAsync(PatientDtoPost dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Email", dto.Email, DbType.String, size: 100);
        parameters.Add("Password", dto.Password, DbType.String, size: 100);
        parameters.Add("FirstName", dto.FirstName, DbType.String, size: 100);
        parameters.Add("LastName", dto.LastName, DbType.String, size: 100);
        parameters.Add("Phone", dto.Phone, DbType.String, size: 20);
        parameters.Add("BirthDate", dto.BirthDate, DbType.Date);

        return QuerySingleAsync<PatientDtoGet>("dbo.Create_Patient", parameters, ct);
    }

    public Task<Response<PatientDtoGet>> UpdateAsync(int patientId, PatientDtoPut dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", patientId, DbType.Int32);
        parameters.Add("FirstName", dto.FirstName, DbType.String, size: 100);
        parameters.Add("LastName", dto.LastName, DbType.String, size: 100);
        parameters.Add("Phone", dto.Phone, DbType.String, size: 20);
        parameters.Add("BirthDate", dto.BirthDate, DbType.Date);

        return QuerySingleAsync<PatientDtoGet>("dbo.Update_Patient", parameters, ct);
    }

    public Task<Response<bool>> DeleteAsync(int patientId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", patientId, DbType.Int32);

        return ExecuteNonQueryAsync("dbo.Delete_Patient", parameters, ct);
    }
}
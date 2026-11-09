using System.Data;
using Dapper;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Doctor;
using FamilyFeet.Api.Infrastructure;
using FamilyFeet.Api.Repositories.Interface;

namespace FamilyFeet.Api.Repositories;

public sealed class DoctorRepository : BaseRepository, IDoctorRepository
{
    public DoctorRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public Task<Response<IEnumerable<DoctorDtoGet>>> GetPaginatedAsync(DoctorDtoFilter filter, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("PageNumber", filter.PageNumber, DbType.Int32);
        parameters.Add("PageSize", filter.PageSize, DbType.Int32);
        parameters.Add("Search", string.IsNullOrWhiteSpace(filter.Search) ? null : filter.Search.Trim(), DbType.String, size: 100);

        return QueryListAsync<DoctorDtoGet>("dbo.Get_Doctor_Paginated", parameters, ct);
    }

    public Task<Response<DoctorDtoGet>> GetByIdAsync(int doctorId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", doctorId, DbType.Int32);

        return QuerySingleAsync<DoctorDtoGet>("dbo.Get_Doctor_ById", parameters, ct);
    }

    public Task<Response<DoctorDtoGet>> CreateAsync(DoctorDtoPost dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Email", dto.Email, DbType.String, size: 100);
        parameters.Add("Password", dto.Password, DbType.String, size: 100);
        parameters.Add("FirstName", dto.FirstName, DbType.String, size: 100);
        parameters.Add("LastName", dto.LastName, DbType.String, size: 100);
        parameters.Add("LicenseNumber", dto.LicenseNumber, DbType.String, size: 50);

        return QuerySingleAsync<DoctorDtoGet>("dbo.Create_Doctor", parameters, ct);
    }

    public Task<Response<DoctorDtoGet>> UpdateAsync(int doctorId, DoctorDtoPut dto, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", doctorId, DbType.Int32);
        parameters.Add("FirstName", dto.FirstName, DbType.String, size: 100);
        parameters.Add("LastName", dto.LastName, DbType.String, size: 100);
        parameters.Add("LicenseNumber", dto.LicenseNumber, DbType.String, size: 50);

        return QuerySingleAsync<DoctorDtoGet>("dbo.Update_Doctor", parameters, ct);
    }

    public Task<Response<bool>> DeleteAsync(int doctorId, CancellationToken ct = default)
    {
        var parameters = CreateParameters();
        parameters.Add("Id", doctorId, DbType.Int32);

        return ExecuteNonQueryAsync("dbo.Delete_Doctor", parameters, ct);
    }
}
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Doctor;

namespace FamilyFeet.Api.Repositories.Interface;

public interface IDoctorRepository
{
    Task<Response<IEnumerable<DoctorDtoGet>>> GetPaginatedAsync(DoctorDtoFilter filter, CancellationToken ct = default);
    Task<Response<DoctorDtoGet>> GetByIdAsync(int doctorId, CancellationToken ct = default);
    Task<Response<DoctorDtoGet>> CreateAsync(DoctorDtoPost dto, CancellationToken ct = default);
    Task<Response<DoctorDtoGet>> UpdateAsync(int doctorId, DoctorDtoPut dto, CancellationToken ct = default);
    Task<Response<bool>> DeleteAsync(int doctorId, CancellationToken ct = default);
}
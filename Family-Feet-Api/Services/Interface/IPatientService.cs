using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Patient;

namespace FamilyFeet.Api.Services.Interface;

public interface IPatientService
{
    Task<Response<IEnumerable<PatientDtoGet>>> GetPaginatedAsync(PatientDtoFilter filter, CancellationToken ct = default);
    Task<Response<PatientDtoGet>> GetByIdAsync(int patientId, CancellationToken ct = default);
    Task<Response<PatientDtoGet>> CreateAsync(PatientDtoPost dto, CancellationToken ct = default);
    Task<Response<PatientDtoGet>> UpdateAsync(int patientId, PatientDtoPut dto, CancellationToken ct = default);
    Task<Response<bool>> DeleteAsync(int patientId, CancellationToken ct = default);
}
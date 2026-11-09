using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Patient;
using FamilyFeet.Api.Repositories.Interface;
using FamilyFeet.Api.Services.Interface;

namespace FamilyFeet.Api.Services;

public sealed class PatientService : BaseService, IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository, ILogger<PatientService> logger) : base(logger)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<PatientDtoGet>>> GetPaginatedAsync(PatientDtoFilter filter, CancellationToken ct = default)
        => Log(await _repository.GetPaginatedAsync(filter, ct), nameof(GetPaginatedAsync));

    public async Task<Response<PatientDtoGet>> GetByIdAsync(int patientId, CancellationToken ct = default)
        => Log(await _repository.GetByIdAsync(patientId, ct), nameof(GetByIdAsync));

    public async Task<Response<PatientDtoGet>> CreateAsync(PatientDtoPost dto, CancellationToken ct = default)
    {
        dto.Email = dto.Email.Trim().ToLowerInvariant();
        dto.FirstName = dto.FirstName.Trim();
        dto.LastName = dto.LastName.Trim();
        dto.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();

        if (dto.BirthDate.HasValue && dto.BirthDate.Value.Date > DateTime.Today)
        {
            return Response<PatientDtoGet>.Fail(ResultCode.BadRequest, "BirthDate cannot be in the future.");
        }

        return Log(await _repository.CreateAsync(dto, ct), nameof(CreateAsync));
    }

    public async Task<Response<PatientDtoGet>> UpdateAsync(int patientId, PatientDtoPut dto, CancellationToken ct = default)
    {
        dto.FirstName = dto.FirstName.Trim();
        dto.LastName = dto.LastName.Trim();
        dto.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();

        if (dto.BirthDate.HasValue && dto.BirthDate.Value.Date > DateTime.Today)
        {
            return Response<PatientDtoGet>.Fail(ResultCode.BadRequest, "BirthDate cannot be in the future.");
        }

        return Log(await _repository.UpdateAsync(patientId, dto, ct), nameof(UpdateAsync));
    }

    public async Task<Response<bool>> DeleteAsync(int patientId, CancellationToken ct = default)
        => Log(await _repository.DeleteAsync(patientId, ct), nameof(DeleteAsync));
}
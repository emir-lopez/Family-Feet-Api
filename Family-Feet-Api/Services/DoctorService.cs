using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Doctor;
using FamilyFeet.Api.Repositories.Interface;
using FamilyFeet.Api.Services.Interface;

namespace FamilyFeet.Api.Services;

public sealed class DoctorService : BaseService, IDoctorService
{
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository, ILogger<DoctorService> logger) : base(logger)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<DoctorDtoGet>>> GetPaginatedAsync(DoctorDtoFilter filter, CancellationToken ct = default)
        => Log(await _repository.GetPaginatedAsync(filter, ct), nameof(GetPaginatedAsync));

    public async Task<Response<DoctorDtoGet>> GetByIdAsync(int doctorId, CancellationToken ct = default)
        => Log(await _repository.GetByIdAsync(doctorId, ct), nameof(GetByIdAsync));

    public async Task<Response<DoctorDtoGet>> CreateAsync(DoctorDtoPost dto, CancellationToken ct = default)
    {
        dto.Email = dto.Email.Trim().ToLowerInvariant();
        dto.FirstName = dto.FirstName.Trim();
        dto.LastName = dto.LastName.Trim();
        dto.LicenseNumber = dto.LicenseNumber.Trim();

        return Log(await _repository.CreateAsync(dto, ct), nameof(CreateAsync));
    }

    public async Task<Response<DoctorDtoGet>> UpdateAsync(int doctorId, DoctorDtoPut dto, CancellationToken ct = default)
    {
        dto.FirstName = dto.FirstName.Trim();
        dto.LastName = dto.LastName.Trim();
        dto.LicenseNumber = dto.LicenseNumber.Trim();

        return Log(await _repository.UpdateAsync(doctorId, dto, ct), nameof(UpdateAsync));
    }

    public async Task<Response<bool>> DeleteAsync(int doctorId, CancellationToken ct = default)
        => Log(await _repository.DeleteAsync(doctorId, ct), nameof(DeleteAsync));
}
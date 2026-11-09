using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Appointment;
using FamilyFeet.Api.Repositories.Interface;
using FamilyFeet.Api.Services.Interface;

namespace FamilyFeet.Api.Services;

public sealed class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IAppointmentRepository repository, ILogger<AppointmentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Response<AppointmentDtoGet>> CreateAsync(AppointmentDtoPost dto, CancellationToken ct = default)
    {
        dto.AppointmentDate = DateTimeHelper.ToUtc(dto.AppointmentDate);
        dto.Reason = string.IsNullOrWhiteSpace(dto.Reason) ? null : dto.Reason.Trim();

        if (dto.AppointmentDate <= DateTime.UtcNow)
        {
            return Response<AppointmentDtoGet>.Fail(ResultCode.BadRequest, "The appointment date must be in the future.");
        }

        var response = await _repository.CreateAsync(dto, ct);
        LogIfServerError(response, nameof(CreateAsync));
        return response;
    }

    public async Task<Response<AppointmentDtoGet>> UpdateAsync(int appointmentId, AppointmentDtoPut dto, CancellationToken ct = default)
    {
        dto.AppointmentDate = DateTimeHelper.ToUtc(dto.AppointmentDate);
        dto.Reason = string.IsNullOrWhiteSpace(dto.Reason) ? null : dto.Reason.Trim();

        if (dto.AppointmentDate <= DateTime.UtcNow)
        {
            return Response<AppointmentDtoGet>.Fail(ResultCode.BadRequest, "The appointment date must be in the future.");
        }

        var response = await _repository.UpdateAsync(appointmentId, dto, ct);
        LogIfServerError(response, nameof(UpdateAsync));
        return response;
    }

    public async Task<Response<bool>> DeleteAsync(int appointmentId, CancellationToken ct = default)
    {
        var response = await _repository.DeleteAsync(appointmentId, ct);
        LogIfServerError(response, nameof(DeleteAsync));
        return response;
    }

    public async Task<Response<AppointmentDtoGet>> GetByIdAsync(int appointmentId, CancellationToken ct = default)
    {
        var response = await _repository.GetByIdAsync(appointmentId, ct);
        LogIfServerError(response, nameof(GetByIdAsync));
        return response;
    }

    public async Task<Response<IEnumerable<AppointmentDtoGet>>> GetPaginatedAsync(AppointmentDtoFilter filter, CancellationToken ct = default)
    {
        var response = await _repository.GetPaginatedAsync(filter, ct);
        LogIfServerError(response, nameof(GetPaginatedAsync));
        return response;
    }

    private void LogIfServerError<T>(Response<T> response, string operation)
    {
        if (response.Result == ResultCode.ServerError)
        {
            _logger.LogError("Operation {Operation} failed in the database: {Message}", operation, response.Msg);
        }
    }
}

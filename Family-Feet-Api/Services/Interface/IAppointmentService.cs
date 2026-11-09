using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Appointment;

namespace FamilyFeet.Api.Services.Interface;

public interface IAppointmentService
{
    Task<Response<AppointmentDtoGet>> CreateAsync(AppointmentDtoPost dto, CancellationToken ct = default);
    Task<Response<AppointmentDtoGet>> UpdateAsync(int appointmentId, AppointmentDtoPut dto, CancellationToken ct = default);
    Task<Response<bool>> DeleteAsync(int appointmentId, CancellationToken ct = default);
    Task<Response<AppointmentDtoGet>> GetByIdAsync(int appointmentId, CancellationToken ct = default);
    Task<Response<IEnumerable<AppointmentDtoGet>>> GetPaginatedAsync(AppointmentDtoFilter filter, CancellationToken ct = default);
}

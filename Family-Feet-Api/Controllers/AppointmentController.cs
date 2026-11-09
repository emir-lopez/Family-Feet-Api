using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Appointment;
using FamilyFeet.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Controllers;

[Route("api/[controller]")]
public class AppointmentController : BaseApiController
{
    private readonly IAppointmentService _service;

    public AppointmentController(IAppointmentService service) => _service = service;

    /// <summary>Gets appointments paginated, with optional filters.</summary>
    /// <response code="200">Page of appointments. Count holds the total number of records.</response>
    /// <response code="400">Invalid pagination or filters.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<AppointmentDtoGet>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginated([FromQuery] AppointmentDtoFilter filter, CancellationToken ct)
    {
        var response = await _service.GetPaginatedAsync(filter, ct);
        return ToActionResult(response);
    }

    /// <summary>Gets one appointment by id.</summary>
    /// <response code="200">Appointment found.</response>
    /// <response code="404">Appointment does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute, Range(1, int.MaxValue, ErrorMessage = "id must be greater than 0.")] int id, CancellationToken ct)
    {
        var response = await _service.GetByIdAsync(id, ct);
        return ToActionResult(response);
    }

    /// <summary>Creates an appointment.</summary>
    /// <response code="201">Appointment created.</response>
    /// <response code="400">Validation error (e.g. past date).</response>
    /// <response code="404">Patient or doctor not found.</response>
    /// <response code="409">The doctor already has an appointment at that date and time.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] AppointmentDtoPost dto, CancellationToken ct)
    {
        var response = await _service.CreateAsync(dto, ct);

        if (response.IsSuccess && response.Data is not null)
        {
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        return ToActionResult(response);
    }

    /// <summary>Updates an appointment.</summary>
    /// <response code="200">Appointment updated.</response>
    /// <response code="404">Appointment does not exist.</response>
    /// <response code="409">The doctor already has an appointment at that date and time.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<AppointmentDtoGet>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update([FromRoute, Range(1, int.MaxValue, ErrorMessage = "id must be greater than 0.")] int id, [FromBody] AppointmentDtoPut dto, CancellationToken ct)
    {
        var response = await _service.UpdateAsync(id, dto, ct);
        return ToActionResult(response);
    }

    /// <summary>Logically deletes an appointment (IsActive = 0).</summary>
    /// <response code="200">Appointment deleted.</response>
    /// <response code="404">Appointment does not exist.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute, Range(1, int.MaxValue, ErrorMessage = "id must be greater than 0.")] int id, CancellationToken ct)
    {
        var response = await _service.DeleteAsync(id, ct);
        return ToActionResult(response);
    }
}

using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Patient;
using FamilyFeet.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Controllers;

[Route("api/[controller]")]
public class PatientController : BaseApiController
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service) => _service = service;

    /// <summary>Gets patients paginated, with optional search by name or phone.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<PatientDtoGet>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginated([FromQuery] PatientDtoFilter filter, CancellationToken ct)
        => ToActionResult(await _service.GetPaginatedAsync(filter, ct));

    /// <summary>Gets one patient by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken ct)
        => ToActionResult(await _service.GetByIdAsync(id, ct));

    /// <summary>Creates a patient and its user account.</summary>
    /// <response code="201">Patient created.</response>
    /// <response code="409">The email is already registered.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] PatientDtoPost dto, CancellationToken ct)
    {
        var response = await _service.CreateAsync(dto, ct);

        if (response.IsSuccess && response.Data is not null)
        {
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        return ToActionResult(response);
    }

    /// <summary>Updates the patient data.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<PatientDtoGet>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute, Range(1, int.MaxValue)] int id, [FromBody] PatientDtoPut dto, CancellationToken ct)
        => ToActionResult(await _service.UpdateAsync(id, dto, ct));

    /// <summary>Logically deletes a patient and deactivates its user.</summary>
    /// <response code="409">The patient has upcoming appointments.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken ct)
        => ToActionResult(await _service.DeleteAsync(id, ct));
}
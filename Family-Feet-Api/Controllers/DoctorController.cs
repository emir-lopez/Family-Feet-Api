using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Common;
using FamilyFeet.Api.Dtos.Doctor;
using FamilyFeet.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFeet.Api.Controllers;

[Route("api/[controller]")]
public class DoctorController : BaseApiController
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service) => _service = service;

    /// <summary>Gets doctors paginated, with optional search by name or license number.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<DoctorDtoGet>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginated([FromQuery] DoctorDtoFilter filter, CancellationToken ct)
        => ToActionResult(await _service.GetPaginatedAsync(filter, ct));

    /// <summary>Gets one doctor by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken ct)
        => ToActionResult(await _service.GetByIdAsync(id, ct));

    /// <summary>Creates a doctor and its user account.</summary>
    /// <response code="409">The email or the license number is already registered.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] DoctorDtoPost dto, CancellationToken ct)
    {
        var response = await _service.CreateAsync(dto, ct);

        if (response.IsSuccess && response.Data is not null)
        {
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        return ToActionResult(response);
    }

    /// <summary>Updates the doctor data.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<DoctorDtoGet>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update([FromRoute, Range(1, int.MaxValue)] int id, [FromBody] DoctorDtoPut dto, CancellationToken ct)
        => ToActionResult(await _service.UpdateAsync(id, dto, ct));

    /// <summary>Logically deletes a doctor and deactivates its user.</summary>
    /// <response code="409">The doctor has upcoming appointments.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken ct)
        => ToActionResult(await _service.DeleteAsync(id, ct));
}
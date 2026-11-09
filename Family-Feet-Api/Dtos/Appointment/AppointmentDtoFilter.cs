using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Dtos.Pagination;

namespace FamilyFeet.Api.Dtos.Appointment;

/// <summary>Query string of the paginated GET: pagination + optional filters.</summary>
public class AppointmentDtoFilter : PaginationDto
{
    /// <summary>Filter by doctor (optional).</summary>
    [Range(1, int.MaxValue, ErrorMessage = "IdDoctor must be greater than 0.")]
    public int? IdDoctor { get; set; }
}

using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Dtos.Pagination;

namespace FamilyFeet.Api.Dtos.Doctor;

public class DoctorDtoFilter : PaginationDto
{
    /// <summary>Searches by name or license number (optional).</summary>
    [StringLength(100, ErrorMessage = "Search cannot exceed 100 characters.")]
    public string? Search { get; set; }
}
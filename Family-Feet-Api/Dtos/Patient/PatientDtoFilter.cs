using System.ComponentModel.DataAnnotations;
using FamilyFeet.Api.Dtos.Pagination;

namespace FamilyFeet.Api.Dtos.Patient;

public class PatientDtoFilter : PaginationDto
{
    [StringLength(100, ErrorMessage = "Search cannot exceed 100 characters.")]
    public string? Search { get; set; }
}
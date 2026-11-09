using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Patient;

public class PatientDtoPut
{
    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(100, ErrorMessage = "FirstName cannot exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(100, ErrorMessage = "LastName cannot exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
    public string? Phone { get; set; }

    public DateTime? BirthDate { get; set; }
}
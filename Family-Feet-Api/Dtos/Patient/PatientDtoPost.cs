using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Patient;

public class PatientDtoPost
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; } = string.Empty;

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
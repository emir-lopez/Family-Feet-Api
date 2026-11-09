using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Doctor;

/// <summary>Creates a doctor AND its user account.</summary>
public class DoctorDtoPost
{
    /// <example>doctor@correo.com</example>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; } = string.Empty;

    /// <example>Emir</example>
    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(100, ErrorMessage = "FirstName cannot exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    /// <example>López</example>
    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(100, ErrorMessage = "LastName cannot exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    /// <example>1234567</example>
    [Required(ErrorMessage = "LicenseNumber is required.")]
    [StringLength(50, ErrorMessage = "LicenseNumber cannot exceed 50 characters.")]
    public string LicenseNumber { get; set; } = string.Empty;
}
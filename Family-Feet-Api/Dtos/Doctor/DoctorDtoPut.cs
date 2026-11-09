using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Doctor;

public class DoctorDtoPut
{
    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(100, ErrorMessage = "FirstName cannot exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(100, ErrorMessage = "LastName cannot exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LicenseNumber is required.")]
    [StringLength(50, ErrorMessage = "LicenseNumber cannot exceed 50 characters.")]
    public string LicenseNumber { get; set; } = string.Empty;
}
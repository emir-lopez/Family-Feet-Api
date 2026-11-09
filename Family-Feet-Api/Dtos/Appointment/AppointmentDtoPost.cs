using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Appointment;

/// <summary>Request body to create an appointment.</summary>
public class AppointmentDtoPost
{
    /// <summary>Patient identifier.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "IdPatient must be greater than 0.")]
    public int IdPatient { get; set; }

    /// <summary>Doctor identifier.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "IdDoctor must be greater than 0.")]
    public int IdDoctor { get; set; }

    /// <summary>Date and time of the appointment in UTC (ISO 8601). Must be in the future.</summary>
    /// <example>2027-01-15T10:30:00Z</example>
    [Required(ErrorMessage = "AppointmentDate is required.")]
    public DateTime AppointmentDate { get; set; }

    /// <summary>Reason for the visit (optional).</summary>
    /// <example>General check-up</example>
    [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
    public string? Reason { get; set; }
}

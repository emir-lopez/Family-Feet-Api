using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Appointment;

/// <summary>Request body to update an appointment. The identifier comes from the route.</summary>
public class AppointmentDtoPut
{
    /// <summary>New date and time of the appointment in UTC (ISO 8601). Must be in the future.</summary>
    /// <example>2027-01-16T11:00:00Z</example>
    [Required(ErrorMessage = "AppointmentDate is required.")]
    public DateTime AppointmentDate { get; set; }

    /// <summary>Reason for the visit (optional).</summary>
    /// <example>Follow-up consultation</example>
    [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
    public string? Reason { get; set; }
}

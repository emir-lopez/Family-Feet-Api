namespace FamilyFeet.Api.Dtos.Appointment;

/// <summary>Appointment returned by the API.</summary>
public class AppointmentDtoGet
{
    public int Id { get; set; }
    public int IdPatient { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int IdDoctor { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Reason { get; set; }
}

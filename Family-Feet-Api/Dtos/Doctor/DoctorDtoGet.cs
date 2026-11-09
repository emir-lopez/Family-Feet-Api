namespace FamilyFeet.Api.Dtos.Doctor;

public class DoctorDtoGet
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
}
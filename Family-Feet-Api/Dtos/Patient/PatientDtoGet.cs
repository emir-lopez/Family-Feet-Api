namespace FamilyFeet.Api.Dtos.Patient;
public class PatientDtoGet
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
}
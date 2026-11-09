namespace FamilyFeet.Api.Dtos.User;

public class UserDtoGet
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
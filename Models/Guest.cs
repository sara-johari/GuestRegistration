namespace GuestRegistration.Models;

public class Guest
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int PhoneNumber { get; set; }
}
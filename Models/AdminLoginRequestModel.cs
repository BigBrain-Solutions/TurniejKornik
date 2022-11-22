namespace KornikTournament.Models;

public class AdminLoginRequestModel
{
    public required string Nickname { get; set; }
    public required string Password { get; set; }
}
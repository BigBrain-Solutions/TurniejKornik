namespace KornikTournament.Models;

public class TournamentSettings
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Ended { get; set; }

    public List<Participant> Organizers { get; set; } = null!;
}
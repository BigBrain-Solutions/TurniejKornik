namespace KornikTournament.Models;

public class Announcement
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? TimeAdded { get; set; }
}
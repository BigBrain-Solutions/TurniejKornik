﻿namespace KornikTournament.Models;

public class Announcements
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}
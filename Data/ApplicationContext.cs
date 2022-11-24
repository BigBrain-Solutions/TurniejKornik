using KornikTournament.Models;
using Microsoft.EntityFrameworkCore;

namespace KornikTournament.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participant>(eb =>
        {
            eb.HasOne(x => x.Team).WithMany(x => x.Participants).OnDelete(DeleteBehavior.ClientCascade);
        });
        
        modelBuilder.Entity<Team>(eb =>
        {
            eb.HasMany(x => x.Participants).WithOne(x => x.Team).OnDelete(DeleteBehavior.ClientCascade);
        });
    }

    public DbSet<Participant> Participants { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
}
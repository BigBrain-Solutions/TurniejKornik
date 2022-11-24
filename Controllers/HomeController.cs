using System.Diagnostics;
using KornikTournament.Data;
using KornikTournament.Enums;
using Microsoft.AspNetCore.Mvc;
using KornikTournament.Models;
using Microsoft.EntityFrameworkCore;

namespace KornikTournament.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["Announcements"] = _context.Announcements.OrderByDescending(x => x.TimeAdded).ToList();
        return View();
    }
    
    [Route("Contact")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost("Announcement")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteAnnouncement(int id)
    {
        var announcement = _context.Announcements.First(x => x.Id == id);

        _context.Announcements.Remove(announcement);

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    
    [Route("FreeAgents")]
    public IActionResult FreeAgents(ErrorTypes? error)
    {
        ViewData["agents"] = _context.Participants.Include(x => x.Team).Where(x => x.Team == null).ToList();
        ViewData["error"] = error;
        return View();
    }
    
    [HttpPost("FreeAgents")]
    public async Task<IActionResult> FreeAgentsConfirmed(AddParticipant participant)
    {
        if (ModelState.IsValid)
        {
            if (_context.Participants.Any(x => x.Nickname == participant.Nickname))
            {
                return RedirectToAction(nameof(FreeAgents), new {error = ErrorTypes.AgentIsOnTheList});
            }
            
            _context.Participants.Add(new Participant
            {
                Id = Guid.NewGuid(),
                Name = participant.Name,
                Surname = participant.Surname,
                Nickname = participant.Nickname,
                Class = participant.Class,
                IsAdmin = false,
                Leader = false,
                Roles = participant.Role,
                Team = null!
            });

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(FreeAgents));
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
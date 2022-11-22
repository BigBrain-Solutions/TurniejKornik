using System.Diagnostics;
using System.Security.Claims;
using KornikTournament.Data;
using KornikTournament.Enums;
using KornikTournament.Helpers;
using KornikTournament.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KornikTournament.Controllers;

[Route("[controller]")]
public class TeamController : Controller
{
    private readonly ApplicationContext _context;

    public TeamController(ApplicationContext context)
    {
        _context = context;
    }

    public IActionResult Index([FromQuery] Guid id)
    {
        ViewData["Team"] = _context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == id);
        return View();
    }
    
    
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateTeam(RegisterTeamModel model)
    {
        if (ModelState.IsValid)
        {
            var any = _context.Teams.Any(x => x.Tag == model.Tag);

            if (any)
            {
                return RedirectToAction(nameof(Create));
            }
            
            var leader = new Participant
            {
                Id = Guid.NewGuid(),
                Name = model.LeaderName,
                Surname = model.LeaderSurname,
                Nickname = model.LeaderNickname,
                Class = model.LeaderClass,
                PasswordHash = model.Password,
                Leader = true,
                Roles = Enum.Parse<ERole>(model.Role)
            };

            _context.Teams.Add(new Team
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Tag = model.Tag,
                Participants = new List<Participant> {leader}
            });
            
            var success = Register(model.LeaderNickname, leader);

            if (!success.success)
            {
                return RedirectToAction(nameof(Create));
            }
            
            _context.SaveChanges();

            return RedirectToAction("Login", "Team");
        }
        
        return RedirectToAction(nameof(Create));
    }

    private (bool success, string content) Register(string nickname, Participant participant)
    {
        if (_context.Participants.Any(x => x.Nickname == nickname))
            return (false, "Username not available");
        
        participant.ProvideSaltAndHash();

        _context.Add(participant);
        _context.SaveChanges();

        return (true, string.Empty);
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public IActionResult LoginConfirmed(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var participant = _context.Participants.Include(x => x.Team).FirstOrDefault(x => x.Nickname == loginModel.Nickname);

            if (participant is null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (!TryLogin(loginModel.Nickname, loginModel.Password))
            {
                return RedirectToAction(nameof(Login));
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, loginModel.Nickname),
                new (ClaimTypes.Sid, participant.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
            
            if (participant.Leader)
            {
                return RedirectToAction(nameof(Index), "Team", new {Id = participant.Team.Id});
            }
        }

        return RedirectToAction(nameof(Login));
    }
    
    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    public IActionResult AddToTeam(AddParticipant participant)
    {
        var team = _context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == participant.TeamId);

        if (team!.Participants.Count >= 5)
        {
            return RedirectToAction(nameof(Index), new {id = participant.TeamId});
        }

        if (team.Participants.Any(x => x.Roles == participant.Role))
        {
            return RedirectToAction(nameof(Index), new {id = participant.TeamId});
        }
        
        _context.Participants.Add(new Participant
        {
            Id = Guid.NewGuid(),
            Name = participant.Name,
            Surname = participant.Surname,
            Nickname = participant.Nickname,
            Roles = participant.Role,
            Class = participant.Class,
            Team = team
        });

        _context.SaveChanges();
        
        return RedirectToAction(nameof(Index), new {id = participant.TeamId});
    }
    
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteParticipant(Guid id, Guid teamId)
    {
        var p = _context.Participants.FirstOrDefault(x => x.Id == id);
        _context.Participants.Remove(p!);

        _context.SaveChanges();
        
        return RedirectToAction(nameof(Index), new {id = teamId});
    }
    
    private bool TryLogin(string nickname, string password)
    {
        var participant = _context.Participants.SingleOrDefault(p => p.Nickname == nickname);

        if (participant == null) return false;

        if (participant.PasswordHash != AuthenticationHelper.GenerateHash(password, participant.Salt))
            return false;

        return true;
    }
}
using System.Security.Claims;
using KornikTournament.Data;
using KornikTournament.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KornikTournament.Controllers;

[Route("[controller]")]
public class AdminController : Controller
{
    private readonly ApplicationContext _context;
    private readonly Settings _settings;

    public AdminController(ApplicationContext context, Settings settings)
    {
        _context = context;
        _settings = settings;
    }
    
    public IActionResult Index()
    {
        ViewData["Teams"] = _context.Teams.Include(x => x.Participants).ToList();
        return View();
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public IActionResult LoginConfirm(AdminLoginRequestModel model)
    {

        if (ModelState.IsValid)
        {
            if (model.Password != _settings.AdminPass)
            {
                return RedirectToAction(nameof(Login));
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
            
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Login));
    }
    
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteTeam(Guid id)
    {
        var team = _context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == id);

        _context.Teams.Remove(team);

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("CreateAnnouncements")]
    public IActionResult AddAnnouncements(Announcement announcement)
    {
        if (ModelState.IsValid)
        {
            announcement.TimeAdded = DateTime.Now.ToUniversalTime();
            _context.Announcements.Add(announcement);

            _context.SaveChanges();   
        }

        return RedirectToAction(nameof(Index));
    }
}
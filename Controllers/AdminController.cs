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

    public AdminController(ApplicationContext context)
    {
        _context = context;
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
            var user = _context.Participants.FirstOrDefault(x => x.Nickname == model.Nickname);

            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (model.Password != "d59cf3a97342b79f5c88d7d00be6bc91" || !user.IsAdmin)
            {
                return RedirectToAction(nameof(Login));
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, model.Nickname),
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
    
    
}
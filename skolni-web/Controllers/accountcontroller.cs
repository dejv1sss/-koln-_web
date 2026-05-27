using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skolni_web.Data;
using skolni_web.Models;

namespace skolni_web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Zkontroluj jestli email už existuje
            var existuje = await _db.Users.AnyAsync(u => u.Email == model.Email);
            if (existuje)
            {
                ModelState.AddModelError("Email", "Tento email je již zaregistrován");
                return View(model);
            }

            var user = new User
            {
                Jmeno = model.Jmeno,
                Email = model.Email,
                HesloHash = BCrypt.Net.BCrypt.HashPassword(model.Heslo),
                Role = model.Role,
                DatumRegistrace = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Ulož do session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserJmeno", user.Jmeno);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Dashboard", "Home");
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Heslo, user.HesloHash))
            {
                ModelState.AddModelError("", "Nesprávný email nebo heslo");
                return View(model);
            }

            // Ulož do session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserJmeno", user.Jmeno);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Dashboard", "Home");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
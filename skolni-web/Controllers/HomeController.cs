using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skolni_web.Data;
using skolni_web.Models;

namespace skolni_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Kontakt()
        {
            return View();
        }

        // Dashboard - přesměruje podle role
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role == null)
                return RedirectToAction("Login", "Account");

            if (role == "ucitel")
                return RedirectToAction("DashboardUcitel");
            else
                return RedirectToAction("DashboardZak");
        }

        // =====================
        // DASHBOARD ŽÁKA
        // =====================
        public async Task<IActionResult> DashboardZak()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "zak")
                return RedirectToAction("Login", "Account");

            var tahaky = await _db.Tahaky
                .Include(t => t.User)
                .OrderByDescending(t => t.DatumPridani)
                .ToListAsync();

            return View(tahaky);
        }

        // POST: Přidat tahák
        [HttpPost]
        public async Task<IActionResult> PridatTahak(string nazev, string obsah)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (role != "zak" || userId == null)
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrWhiteSpace(nazev) && !string.IsNullOrWhiteSpace(obsah))
            {
                _db.Tahaky.Add(new Tahak
                {
                    Nazev = nazev,
                    Obsah = obsah,
                    UserId = userId.Value,
                    DatumPridani = DateTime.Now
                });
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("DashboardZak");
        }

        // POST: Smazat tahák
        [HttpPost]
        public async Task<IActionResult> SmazatTahak(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var tahak = await _db.Tahaky.FindAsync(id);

            if (tahak != null && tahak.UserId == userId)
            {
                _db.Tahaky.Remove(tahak);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("DashboardZak");
        }

        // =====================
        // DASHBOARD UČITELE
        // =====================
        public async Task<IActionResult> DashboardUcitel()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "ucitel")
                return RedirectToAction("Login", "Account");

            var hodiny = await _db.OdpadnuteHodiny
                .Include(h => h.User)
                .OrderByDescending(h => h.Datum)
                .ToListAsync();

            return View(hodiny);
        }

        // POST: Přidat odpadnutou hodinu
        [HttpPost]
        public async Task<IActionResult> PridatHodinu(string predmet, string trida, DateTime datum, string poznamka)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (role != "ucitel" || userId == null)
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrWhiteSpace(predmet) && !string.IsNullOrWhiteSpace(trida))
            {
                _db.OdpadnuteHodiny.Add(new OdpadnutaHodina
                {
                    Predmet = predmet,
                    Trida = trida,
                    Datum = datum,
                    Poznamka = poznamka ?? "",
                    UserId = userId.Value
                });
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("DashboardUcitel");
        }

        // POST: Smazat hodinu
        [HttpPost]
        public async Task<IActionResult> SmazatHodinu(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var hodina = await _db.OdpadnuteHodiny.FindAsync(id);

            if (hodina != null && hodina.UserId == userId)
            {
                _db.OdpadnuteHodiny.Remove(hodina);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("DashboardUcitel");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Streamify.Models;

namespace Streamify.Controllers
{
    public class AdminController : Controller
    {
        private readonly Database _database;

        public AdminController(Database database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Query()
        {
            return View();
        }

        [HttpGet("/Query/Amministratore")]
        public IActionResult Amministratore()
        {
            ViewBag.RisultatoPrimaQuery = _database.PrimaQuery() ?? new List<dynamic>();
            ViewBag.RisultatoSecondaQuery = _database.SecondaQuery() ?? new List<dynamic>();
            ViewBag.RisultatoTerzaQuery = _database.TerzaQuery() ?? new List<dynamic>();
            ViewBag.RisultatoQuartaQuery = _database.QuartaQuery() ?? new List<dynamic>();
            ViewBag.RisultatoQuintaQuery = _database.QuintaQuery(5) ?? new List<dynamic>();

            return View();
        }

        [HttpGet("/Query/Utente")]
        public IActionResult Utente(int? userId, DateTime? datainizio, DateTime? datafine)
        {
            InizializzaViewBag();

            if (!userId.HasValue)
            {
                ViewBag.Error = "Specifica un ID utente";
                return View();
            }

            if (!datainizio.HasValue || !datafine.HasValue)
            {
                
                datainizio ??= new DateTime(2000, 1, 1); 
                datafine ??= DateTime.UtcNow;           
            }

            if (datainizio.Value < new DateTime(1753, 1, 1) || datafine.Value < new DateTime(1753, 1, 1))
            {
                ModelState.AddModelError("DateRange", "Le date devono essere uguali o successive al 01/01/1753.");
                return View();
            }

            CaricaEntrambeLeQuery(userId.Value, datainizio.Value, datafine.Value);
            return View();
        }

        private void InizializzaViewBag()
        {
            ViewBag.RisultatoPrimaQuery = new List<dynamic>();
            ViewBag.RisultatoSecondaQuery = new List<dynamic>();
            ViewBag.Error = null;
            ViewBag.UserId = null;
            ViewBag.StartDate = null;
            ViewBag.EndDate = null;
        }


        private void CaricaEntrambeLeQuery(int userId, DateTime datainizio, DateTime datafine)
        {
            ViewBag.RisultatoPrimaQuery = _database.PrimaQueryUtente(userId) ?? new List<dynamic>();

            var secondaQueryRisultati = _database.OttieniCronologiaConRange(userId, datainizio, datafine);

            ViewBag.RisultatoSecondaQuery = secondaQueryRisultati ?? new List<dynamic>();
            ViewBag.UserId = userId;
            ViewBag.StartDate = datainizio;
            ViewBag.EndDate = datafine;
        }
    }
}

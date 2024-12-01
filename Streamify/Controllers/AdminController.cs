using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        public IActionResult Utente()
        {
            var risultatoPrimaQuery = _database.PrimaQueryUtente(5);
            var risultatoSecondaQuery = _database.OttieniCronologiaPerUtente(5);

            ViewBag.RisultatoPrimaQuery = risultatoPrimaQuery ?? new List<dynamic>();
            ViewBag.RisultatoSecondaQuery = risultatoSecondaQuery ?? new List<dynamic>();

            return View();
        }
    }
}

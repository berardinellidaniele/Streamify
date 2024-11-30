using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Streamify.Controllers
{
    public class SettingsController : Controller
    {

        private readonly Database _database;

        public SettingsController(Database database, IConfiguration configuration)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult Dati()
        {
            var email_utente = HttpContext.Session.GetString("EmailUtente");

            if (string.IsNullOrEmpty(email_utente))
            {
                return RedirectToAction("Login", "Account");
            }

            var utente = _database.OttieniUtenteDaEmail(email_utente);

            ViewBag.Nome = utente?.Nome;
            ViewBag.Cognome = utente?.Cognome;
            ViewBag.Email = utente?.Email;
            ViewBag.DataNascita = utente?.Data_Nascita.ToString("dd/MM/yyyy");
            ViewBag.DataIscrizione = utente?.Data_Iscrizione.ToString("dd/MM/yyyy");

            return View();
        }

        [HttpGet]
        public IActionResult Opzioni()
        {
            var email_utente = HttpContext.Session.GetString("EmailUtente");

            if (string.IsNullOrEmpty(email_utente))
            {
                return RedirectToAction("Login", "Account");
            }

            var (nome, cognome) = _database.NomeCognome(email_utente);

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cognome))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Nome = nome;
            ViewBag.Cognome = cognome;

            return View();
        }
    }
}

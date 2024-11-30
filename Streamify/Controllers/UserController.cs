using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Streamify.Models;

namespace Streamify.Controllers
{
    public class UserController : Controller
    {

        private readonly Database _database;

        public UserController(Database database)
        {
            _database = database;
        }

        public IActionResult User()
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

        public IActionResult Preferiti()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();
            var id_utente = int.Parse(HttpContext.Session.GetString("IdUtente"));

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenerePreferiti(genere, id_utente, 0, 30);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            ViewBag.IdUtente = id_utente;
            return View();
        }

        [HttpGet]
        public IActionResult CaricaPiuContenuti(string genere, int offset, int limit)
        {
            var id_utente = int.Parse(HttpContext.Session.GetString("IdUtente"));

            var contenuti = _database.GetContenutiPerGenerePreferiti(genere, id_utente, offset, limit);
            return PartialView("_ContenutiPartial", contenuti);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using Streamify.Models;
using Streamify.ViewModels;
using System.Text.RegularExpressions;


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

        [HttpGet]
        public IActionResult Cronologia()
        {
            var email_utente = HttpContext.Session.GetString("EmailUtente");

            if (string.IsNullOrEmpty(email_utente))
            {
                return RedirectToAction("Login", "Account");
            }

            var utente = _database.OttieniUtenteDaEmail(email_utente);
            if (utente == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cronologia = _database.OttieniCronologiaPerUtente(utente.ID_Utente);
            ViewBag.Cronologia = cronologia;

            return View();
        }


        [HttpPost]
        public JsonResult SalvaDati([FromBody] Dictionary<string, string> datimodificati)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            var emaill = HttpContext.Session.GetString("EmailUtente");
            if (string.IsNullOrEmpty(emaill))
            {
                return Json(new { success = false, message = "utente non autenticato" });
            }

            var utente = _database.OttieniUtenteDaEmail(emaill);

            if (datimodificati.ContainsKey("Nome"))
            {
                string nome = datimodificati["Nome"].Trim();
                if (nome.Length < 2 || !nome.All(Char.IsLetter))
                {
                    return Json(new { success = false, message = "il nome deve contenere almeno 2 lettere e solo caratteri alfabetici" });
                }
                utente.Nome = nome;
            }

            if (datimodificati.ContainsKey("Cognome"))
            {
                string cognome = datimodificati["Cognome"].Trim();
                if (cognome.Length < 2 || !cognome.All(Char.IsLetter))
                {
                    return Json(new { success = false, message = "il cognome deve contenere almeno 2 lettere e solo caratteri alfabetici" });
                }
                utente.Cognome = cognome;
            }
            if (datimodificati.ContainsKey("Email"))
            {
                string email = datimodificati["Email"].Trim();
                if (!Regex.IsMatch(email, pattern))
                {
                    return Json(new { success = false, message = "l'indirizzo email non è valido." });
                }

                var emailEsistente = _database.OttieniUtenteDaEmail(email);
                if (emailEsistente != null && emailEsistente.ID_Utente != utente.ID_Utente)
                {
                    return Json(new { success = false, message = "email già in uso" });
                }
                utente.Email = email;
            }

            var aggiornato = _database.AggiornaUtente(utente);
            return Json(new { success = aggiornato });
        }

        [HttpGet]
        public JsonResult Cambiastatovisione(int id_cronologia)
        {
            var query = _database.Cambiastatovisione(id_cronologia);
            if (query == false)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

    }
}

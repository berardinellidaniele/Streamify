using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using Streamify.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace Streamify.Controllers
{
    public class AccountController : Controller
    {
        private readonly Database _database;

        public AccountController(Database database)
        {
            _database = database;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(loginviewmodel modelloLogin)
        {
            if (ModelState.IsValid)
            {
                var passwordHashata = HashPassword(modelloLogin.Password);
                if (_database.ValidazioneUtente(modelloLogin.Email, passwordHashata))
                {
                    var utente = _database.OttieniUtenteDaEmail(modelloLogin.Email);
                    HttpContext.Session.SetString("EmailUtente", modelloLogin.Email);
                    HttpContext.Session.SetString("NomeUtente", utente.Nome);
                    HttpContext.Session.SetString("IdUtente", utente.ID_Utente.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email o password non corretti");
                }
            }
            return View(modelloLogin);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(registerviewmodel modelloRegistrazione)
        {
            if (ModelState.IsValid)
            {
                var passwordHashata = HashPassword(modelloRegistrazione.Password);

                var utente = new Utente
                {
                    Nome = modelloRegistrazione.Nome,
                    Cognome = modelloRegistrazione.Cognome,
                    Email = modelloRegistrazione.Email,
                    Data_Iscrizione = DateTime.UtcNow,
                    Data_Nascita = modelloRegistrazione.Data_Nascita
                };

                if (_database.CreaUtente(utente, passwordHashata))
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Errore nella creazione dell'account");
                }
            }
            return View(modelloRegistrazione);
        }

        public IActionResult ChangePassword(string email)
        {
            return View(new CambiaPasswordViewModel { Email = email });
        }

        [HttpPost]
        public IActionResult ChangePassword(CambiaPasswordViewModel modelloCambiaPassword)
        {
            if (ModelState.IsValid)
            {
                var passwordHashata = HashPassword(modelloCambiaPassword.NuovaPassword);
                if (_database.ModificaPassword(modelloCambiaPassword.Email, passwordHashata))
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Errore durante l'aggiornamento della password");
                }
            }
            return View(modelloCambiaPassword);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

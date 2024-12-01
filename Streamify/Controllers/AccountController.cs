using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using Streamify.ViewModels;
using System.Runtime.CompilerServices;
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
                if (modelloLogin.Email.ToLower() == "admin@admin.com")
                {
                    var admin = _database.AdminDaEmail(modelloLogin.Email);
                    if (admin != null && admin.Password.Trim() == modelloLogin.Password.Trim()) 
                    {
                        HttpContext.Session.SetString("EmailUtente", admin.Email);
                        HttpContext.Session.SetString("NomeUtente", admin.Nome);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Credenziali admin non valide");
                        return View(modelloLogin);
                    }
                }

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


        [HttpPost]
        public JsonResult CambiaPassword(CambiaPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return Json(new { success = false, message = "I dati inseriti non sono validi.", errors });
            }

            var emailUtente = HttpContext.Session.GetString("EmailUtente");
            if (string.IsNullOrEmpty(emailUtente))
            {
                return Json(new { success = false, message = "Utente non autenticato." });
            }

            var utente = _database.OttieniUtenteDaEmail(emailUtente);
            if (utente == null)
            {
                return Json(new { success = false, message = "Utente non trovato." });
            }

            var vecchiaPasswordHash = HashPassword(model.VecchiaPassword);
            if (utente.Password != vecchiaPasswordHash)
            {
                return Json(new { success = false, message = "La vecchia password non è corretta." });
            }

            var nuovaPasswordHash = HashPassword(model.NuovaPassword);
            if (utente.Password == nuovaPasswordHash)
            {
                return Json(new { success = false, message = "La nuova password non può essere uguale a quella attuale." });
            }

            if (model.NuovaPassword != model.ConfermaNuovaPassword)
            {
                return Json(new { success = false, message = "Le nuove password non corrispondono." });
            }

            try
            {
                var successo = _database.ModificaPassword(emailUtente, nuovaPasswordHash);

                if (successo)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Errore nel salvataggio della nuova password nel database." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Errore: {ex.Message}" });
            }
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

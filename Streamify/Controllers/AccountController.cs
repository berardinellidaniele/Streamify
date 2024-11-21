using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using Streamify.ViewModels;

namespace UsersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Utente> signInManager;
        private readonly UserManager<Utente> userManager;

        public AccountController(SignInManager<Utente> signinmanager, UserManager<Utente> usermanager)
        {
            this.signInManager = signinmanager;
            this.userManager = usermanager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(loginviewmodel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "password o email non corretti");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(registerviewmodel model)
        {
            if (ModelState.IsValid)
            {
                Utente users = new Utente
                {
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    Data_Nascita = model.Data_Nascita,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerificaEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "qualche errore strano deicsamente lol");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Cambia Password", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Verifica Email", "Account");
            }
            return View(new CambiaPasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(CambiaPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NuovaPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                { 
                    ModelState.AddModelError("", "Email non trovata");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Qualche errore strano decisamente");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
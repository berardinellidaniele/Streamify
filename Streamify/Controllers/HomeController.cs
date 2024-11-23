using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using System.Collections.Generic;

namespace Streamify.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database _database;

        public HomeController(Database database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, 0, 10);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        public IActionResult LoadMoreContent(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, offset, limit);

            return PartialView("_ContenutiPartial", contenuti);
        }
    }
}

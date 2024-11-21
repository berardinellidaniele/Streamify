using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using System.Collections.Generic;

namespace Streamify.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Database _database;

        public DashboardController(Database database)
        {
            _database = database;
        }

        // Carica la dashboard con i generi e i contenuti iniziali
        public IActionResult Dashboard()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = new List<string> { "Azione", "Commedia", "Dramma", "Fantasy", "Thriller" };

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, 0, 10); // Offset iniziale per il primo caricamento
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        // Carica i contenuti per un genere specifico
        public IActionResult GetContenuti(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, offset, limit);
            return PartialView("_Contenuti", contenuti);
        }
    }
}

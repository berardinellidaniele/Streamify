using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Streamify.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Streamify.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database _database;
        private readonly string _youtubeApiKey;

        public HomeController(Database database, IConfiguration configuration)
        {
            _database = database;
            _youtubeApiKey = configuration["Youtube_API:API_KEY"];
        }

        public IActionResult Index()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, 0, 30);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        public IActionResult Film()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, "movie", 0, 30);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        public IActionResult SerieTV()
        {
            var contenutiPerGenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, "tvSeries", 0, 30);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        [HttpGet]
        public IActionResult Generi()
        {
            var contenutipergenere = new Dictionary<string, List<Contenuto>>();
            var generi = _database.GetGeneriUnici();

            foreach (var genere in generi)
            {
                var contenuti = _database.GetContenutiPerGenere(genere, 0, 30);
                contenutipergenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutipergenere;
            return View();
        }


        [HttpGet]
        public IActionResult MostraGenere(string genere)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, 0, 30);

            ViewBag.Contenuti = contenuti;
            ViewBag.Genere = genere;

            return View();
        }

        [HttpGet]
        public IActionResult CercaContenutoOGenere(string query)
        {
            var contenuti = _database.CercaContenutoOGeneri(query);

            ViewBag.Contenuti = contenuti;
            ViewBag.Query = query;

            return View();
        }

        [HttpGet]
        public IActionResult CaricaPiuContenuti(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, offset, limit);
            return PartialView("_ContenutiPartial", contenuti);
        }

        [HttpGet]
        public IActionResult CaricaPiuContenutiFilm(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, "movie", offset, limit);
            return PartialView("_ContenutiPartial", contenuti);
        }

        [HttpGet]
        public IActionResult CaricaPiuContenutiSerieTV(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, "tvSeries", offset, limit);
            return PartialView("_ContenutiPartial", contenuti);
        }

        [HttpGet]
        public IActionResult Dettagli(int id)
        {
            var contenuto = _database.GetContenuto(id);
            if (contenuto == null)
            {
                return NotFound();
            }

            return View(contenuto);
        }

        [HttpGet]
        public JsonResult GetContenutoDettagli(int id)
        {
            var contenuto = _database.GetContenuto(id);
            if (contenuto == null)
            {
                return Json(new { success = false, message = "Contenuto non trovato" });
            }

            return Json(new
            {
                success = true,
                nome = contenuto.Nome,
                descrizione = contenuto.Descrizione,
                trailerKeyword = contenuto.Nome
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetTrailerUrl(string trailerKeyword)
        {
            var query = Uri.EscapeDataString(trailerKeyword + " official" + " trailer");

            var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={query}&type=video&autoplay=1&vq=hd1080&key={_youtubeApiKey}";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var jsonResponse = JsonConvert.DeserializeObject<YouTubeSearchResponse>(response);

            if (jsonResponse.Items != null && jsonResponse.Items.Count > 0)
            {
                var videoId = jsonResponse.Items[0].Id.VideoId;
                var videoUrl = $"https://www.youtube.com/embed/{videoId}";
                return Json(new { success = true, trailerUrl = videoUrl });
            }

            return Json(new { success = false, trailerUrl = string.Empty });
        }

        [HttpGet]
        public JsonResult Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return Json(new List<object>());
            }

            var risultati = _database.CercaContenutoOGeneri(search);
           
            var response = risultati.Select(r => new
            {
                id = r.ID_Contenuto,
                nome = r.Nome,
                descrizione = r.Descrizione, 
                locandina = string.IsNullOrEmpty(r.Locandina) || !Uri.IsWellFormedUriString(r.Locandina, UriKind.Absolute)
                ? Url.Content("~/images/locandina_film_default.jpg")
                : r.Locandina
            }).ToList();

            return Json(response);
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

    public class YouTubeSearchResponse
    {
        public List<YouTubeSearchItem> Items { get; set; }
    }

    public class YouTubeSearchItem
    {
        public YouTubeSearchId Id { get; set; }
    }

    public class YouTubeSearchId
    {   
        public string VideoId { get; set; }
    }
}

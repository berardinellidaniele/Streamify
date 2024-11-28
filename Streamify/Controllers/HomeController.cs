using Microsoft.AspNetCore.Mvc;
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
        private readonly string _youtubeApiKey = "AIzaSyAvdS11fj6_aHedKM7mU9XqqjYtpTmZ_ek";

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
                var contenuti = _database.GetContenutiPerGenere(genere, 0, 30);
                contenutiPerGenere[genere] = contenuti;
            }

            ViewBag.ContenutiPerGenere = contenutiPerGenere;
            return View();
        }

        [HttpGet]
        public IActionResult CaricaPiuContenuti(string genere, int offset, int limit)
        {
            var contenuti = _database.GetContenutiPerGenere(genere, offset, limit);
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
            var query = Uri.EscapeDataString(trailerKeyword + " trailer");

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

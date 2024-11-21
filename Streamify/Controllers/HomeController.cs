using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streamify.Models;
using System.Diagnostics;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace UsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database _database;
        private readonly IDbConnection _dbConnection;

        public HomeController(Database database, IDbConnection dbConnection, ILogger<HomeController> logger)
        {
            _logger = logger;
            _database = database;
            _dbConnection = dbConnection; 
        }

        public IActionResult Index()
        {
            var contenuti = _dbConnection.Query<Contenuto>("SELECT TOP 5 * FROM Contenuto").ToList();

            foreach (var contenuto in contenuti)
            {
                Console.WriteLine($"Nome: {contenuto.Nome}, Tipo: {contenuto.Tipo}, Rating: {contenuto.Rating}");
            }

            return View(contenuti);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

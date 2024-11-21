using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamify.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Streamify.Models
{
    public class Database
    {
        private readonly string _conn;
        private readonly SqlConnection _db;

        public Database(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("Default");
            _db = new SqlConnection(_conn);
        }

        // Ottiene i contenuti per un genere specifico
        public List<Contenuto> GetContenutiPerGenere(string genere, int offset, int limit)
        {
            var query = "SELECT * FROM Contenuto WHERE Genere = @Genere ORDER BY ID_Contenuto OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            var parameters = new { Genere = genere, Offset = offset, Limit = limit };
            return _db.Query<Contenuto>(query, parameters).AsList();
        }

    }
}

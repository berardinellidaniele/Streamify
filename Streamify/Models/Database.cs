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
    }
}

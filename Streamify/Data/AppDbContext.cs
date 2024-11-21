using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Streamify.Models;
using Microsoft.EntityFrameworkCore;

namespace Streamify.Data
{
    public class AppDbContext : IdentityDbContext<Utente>
    {
        public AppDbContext(DbContextOptions opzioni) : base(opzioni)
        {
        }
    }
} 

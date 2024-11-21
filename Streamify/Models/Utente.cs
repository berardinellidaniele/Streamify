using Microsoft.AspNetCore.Identity;

namespace Streamify.Models
{
    public class Utente : IdentityUser
    {
        public int ID_Utente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public DateTime Data_Iscrizione { get; set; }
        public DateTime Data_Nascita { get; set; }
    }
}

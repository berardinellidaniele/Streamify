namespace Streamify.Models
{
    public class Utente
    {
        public int ID_Utente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime Data_Iscrizione { get; set; }
        public DateTime Data_Nascita { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}

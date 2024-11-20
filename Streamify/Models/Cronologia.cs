namespace Streamify.Models
{
    public class Cronologia
    {
        public int ID_Cronologia { get; set; }
        public int ID_Utente { get; set; }
        public int ID_Contenuto { get; set; }
        public DateTime Data_Inizio { get; set; }
        public string Stato { get; set; } = string.Empty;
    }
}

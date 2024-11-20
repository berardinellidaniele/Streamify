namespace Streamify.Models
{
    public class Contenuto
    {
        public int ID_Contenuto { get; set; }
        public int ID_Amministratore { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime Data_Rilascio { get; set; }
        public string Genere { get; set; } = string.Empty;
        public string Locandina { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public float Rating { get; set; }
        public int N_Episodi { get; set; }
        public int Durata { get; set; }
    }
}

namespace Streamify.Models
{
    public class Preferenza
    {
        public int ID_Preferenza { get; set; }
        public int ID_Utente { get; set; }
        public int ID_Contenuto { get; set; }
        public int Voto { get; set; }
    }

}

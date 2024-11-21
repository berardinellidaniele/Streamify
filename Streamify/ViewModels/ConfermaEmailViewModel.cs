using System.ComponentModel.DataAnnotations;

namespace Streamify.ViewModels
{
    public class ConfermaEmailViewModel
    {
        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il codice di conferma è obbligatorio")]
        public string Codice { get; set; }
    }
}

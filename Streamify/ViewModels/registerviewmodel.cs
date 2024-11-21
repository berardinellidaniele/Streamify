using System.ComponentModel.DataAnnotations;


namespace Streamify.ViewModels
{
    public class registerviewmodel
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "La data di nascita è obbligatoria")]
        [DataType(DataType.Date, ErrorMessage = "Formato data non valida")]
        
        public DateTime Data_Nascita { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "La password deve avere almeno {2} caratteri")]

        public string Password { get; set; }

        [Required(ErrorMessage = "La conferma della password è obbligatoria")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La password e la conferma non corrispondono")]
        public string ConfermaPassword { get; set; }

    }
}

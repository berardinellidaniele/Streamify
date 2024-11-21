using System.ComponentModel.DataAnnotations;

namespace Streamify.ViewModels
{
    public class ReimpostaPasswordViewModel
    {
        [Required(ErrorMessage = "L'email è richiesta")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La nuova password è obbligatoria")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "La password deve avere almeno {2} caratteri")]
        public string NuovaPassword { get; set; }

        [Required(ErrorMessage = "Confermare la password")]
        [DataType(DataType.Password)]
        [Compare("NuovaPassword", ErrorMessage = "Le password non corrispondono")]
        public string ConfermaNuovaPassword { get; set; }

        public string Token { get; set; } 
    }
}

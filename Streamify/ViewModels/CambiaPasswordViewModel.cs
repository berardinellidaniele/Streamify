using System.ComponentModel.DataAnnotations;

namespace Streamify.ViewModels
{
    public class CambiaPasswordViewModel
    {
        [Required(ErrorMessage = "L'email è richiesta")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La password è richiesta")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "La password deve avere almeno {2} caratteri")]
        [DataType(DataType.Password)]
        [Display(Name = "Nuova Password")]
        public string NuovaPassword { get; set; }

        [Required(ErrorMessage = "Confermare la password")]
        [DataType(DataType.Password)]
        [Display(Name = "Conferma la nuova password")]
        [Compare("ConfermaNuovaPassword", ErrorMessage = "Le password non corrispondono")]
        public string ConfermaNuovaPassword { get; set; }
    }
}

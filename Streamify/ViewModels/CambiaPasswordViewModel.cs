using System.ComponentModel.DataAnnotations;

namespace Streamify.ViewModels
{
    public class CambiaPasswordViewModel
    {
        [Required(ErrorMessage = "La vecchia password è obbligatoria.")]
        [DataType(DataType.Password)]
        public string VecchiaPassword { get; set; }

        [Required(ErrorMessage = "La nuova password è obbligatoria.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "La password deve essere lunga almeno 8 caratteri.")]
        [DataType(DataType.Password)]
        public string NuovaPassword { get; set; }

        [Required(ErrorMessage = "Confermare la nuova password.")]
        [DataType(DataType.Password)]
        [Compare("NuovaPassword", ErrorMessage = "Le password non corrispondono.")]
        public string ConfermaNuovaPassword { get; set; }
    }


}

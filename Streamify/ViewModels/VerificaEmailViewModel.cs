using System.ComponentModel.DataAnnotations;

namespace Streamify.ViewModels
{
    public class VerificaEmailViewModel
    {
        [Required(ErrorMessage = "L'email è richiesta")]
        [EmailAddress]
        public string Email { get; set; }
    }
}

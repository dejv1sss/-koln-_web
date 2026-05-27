using System.ComponentModel.DataAnnotations;

namespace skolni_web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Neplatný email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Heslo je povinné")]
        public string Heslo { get; set; } = "";
    }
}
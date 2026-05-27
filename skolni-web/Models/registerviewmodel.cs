using System.ComponentModel.DataAnnotations;

namespace skolni_web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Jméno je povinné")]
        public string Jmeno { get; set; } = "";

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Neplatný email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Heslo je povinné")]
        [MinLength(6, ErrorMessage = "Heslo musí mít alespoň 6 znaků")]
        public string Heslo { get; set; } = "";

        [Required(ErrorMessage = "Potvrď heslo")]
        [Compare("Heslo", ErrorMessage = "Hesla se neshodují")]
        public string HesloZnovu { get; set; } = "";

        [Required(ErrorMessage = "Vyber roli")]
        public string Role { get; set; } = ""; // "zak" nebo "ucitel"
    }
}
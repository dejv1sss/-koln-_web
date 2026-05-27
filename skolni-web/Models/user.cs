namespace skolni_web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Jmeno { get; set; } = "";
        public string Email { get; set; } = "";
        public string HesloHash { get; set; } = "";
        public string Role { get; set; } = ""; // "zak" nebo "ucitel"
        public DateTime DatumRegistrace { get; set; } = DateTime.Now;
    }
}
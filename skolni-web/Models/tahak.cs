namespace skolni_web.Models
{
    public class Tahak
    {
        public int Id { get; set; }
        public string Nazev { get; set; } = "";
        public string Obsah { get; set; } = "";
        public DateTime DatumPridani { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
namespace skolni_web.Models
{
    public class OdpadnutaHodina
    {
        public int Id { get; set; }
        public string Predmet { get; set; } = "";
        public string Trida { get; set; } = "";
        public DateTime Datum { get; set; }
        public string Poznamka { get; set; } = "";
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
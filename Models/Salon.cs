namespace Web_Odev.Models
{
    public class Salon
    {
        public int? ID { get; set; } // Birincil anahtar
        public int? SalonID { get; set; }
        public string? Isim { get; set; }

        public string? CalismaSaatleri { get; set; } // Birincil anahtar
    }
}

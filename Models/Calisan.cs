using System.Collections.Generic; // Bu satırı ekleyin

namespace Web_Odev.Models
{
    public class Calisan
    {
        public int? ID { get; set; } // Birincil anahtar
        public string? Isim { get; set; }
        public string? UzmanlikAlanlari { get; set; }
        public string? MusaitlikSaatleri { get; set; }
        public int? SalonID { get; set; }
        public Salon? Salon { get; set; }
        public ICollection<Randevu>? Randevular { get; set; }
    }
}


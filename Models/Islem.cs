namespace Web_Odev.Models
{
    public class Islem
    {
        public int? ID { get; set; } // Birincil anahtar
        public string? Ad { get; set; }
        public int? Sure { get; set; } // Süre (dakika cinsinden)
        public decimal? Ucret { get; set; } // Ücret (örneğin: 100.50)
    }

}

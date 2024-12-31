using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Calisan
    {
        [Key]
        public int? ID { get; set; } // Birincil anahtar

        [Required]
        [MaxLength(100)]
        [Display(Name = "Çalışan İsmi")]
        public string? Isim { get; set; }

        [Display(Name = "Uzmanlık Alanları")]
        public string? UzmanlikAlanlari { get; set; } // Örn: Saç Kesimi, Boyama

        [Display(Name = "Uzmanlık Süreleri")]
        public string? UzmanlikSureleri { get; set; } // Örn: 10, 30 (dakika cinsinden süreler)

        [Display(Name = "Uzmanlık Fiyatlari")]
        public string? UzmanlikFiyatlari { get; set; }  // Yeni eklenen alan

        [Display(Name = "Müsaitlik Saatleri")]
        public string? MusaitlikSaatleri { get; set; } // Örn: 09:00-12:00, 13:00-17:00

        public Salon? Salon { get; set; } // İlgili salon

        public List<Randevu>? Randevular { get; set; } // Çalışanın randevuları
    }
}

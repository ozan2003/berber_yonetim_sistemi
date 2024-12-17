using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Randevu
    {
        [Key]
        public int ID { get; set; } // Birincil anahtar

        [Required]
        [Display(Name = "Ad Soyad")]
        public string? AdSoyad { get; set; } // Kullanıcı adı

        [Required]
        [Display(Name = "Yapılacak İşlem")]
        public string? Islem { get; set; } // İşlem (ör: Saç Kesimi, Boyama)

        [Required]
        [Display(Name = "Randevu Tarihi ve Saati")]
        public DateTime TarihSaat { get; set; } // Randevu zamanı

        [Required]
        [Display(Name = "Çalışan")]
        public int CalisanId { get; set; } // Çalışan ID
        public Calisan? Calisan { get; set; } // Çalışan bilgisi
        public string OnayDurumu { get; set; } = "OnayBekliyor"; // Default değer
    }
}

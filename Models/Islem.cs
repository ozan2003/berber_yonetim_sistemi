using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Islem
    {
        [Key]
        public int ID { get; set; } // Birincil anahtar

        [Required]
        [MaxLength(100)]
        [Display(Name = "İşlem Adı")]
        public string Ad { get; set; } = "İşlem";

        [Display(Name = "Süre (dakika)")]
        public int Sure { get; set; } // Süre (dakika cinsinden)

        [Display(Name = "Ücret")]
        public decimal? Ucret { get; set; } // Ücret (örneğin: 100.50)
    }
}

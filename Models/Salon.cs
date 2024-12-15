using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Salon
    {
        [Key]
        public int ID { get; set; } // Birincil anahtar
        //public int? SalonID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Salon İsmi")]
        public string? Isim { get; set; }

        public string? CalismaSaatleri { get; set; } // Birincil anahtar
    }
}

using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Kullanici
    {
        [Key]
        public int ID { get; set; } // Birincil anahtar

        [Required]
        [MaxLength(100)]
        [Display(Name = "Kullanıcı İsmi")]
        public string? Isim { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Şifre { get; set; }

        public string? Email { get; set; }
        public string Rol { get; set; } = "User"; // Varsayılan rol
    }
}

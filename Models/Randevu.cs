using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Randevu
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Tarih ve Saat")]
        public DateTime TarihSaat { get; set; }
        //public int IslemID { get; set; }

        public Islem? Islem { get; set; }
        //public int CalisanID { get; set; }
        public Calisan? Calisan { get; set; }
        //public int KullaniciID { get; set; }
        public Kullanici? Kullanici { get; set; }
    }
}

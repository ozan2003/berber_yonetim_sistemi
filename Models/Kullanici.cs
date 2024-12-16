using System.ComponentModel.DataAnnotations;

namespace Web_Odev.Models
{
    public class Kullanici
    {
        
            public int ID { get; set; }

            [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
            public string Isim { get; set; }

            [Required(ErrorMessage = "E-posta boş bırakılamaz.")]
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
            [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
            public string Şifre { get; set; }
        public string Rol { get; set; } = "User"; // Varsayılan rol
}
    }


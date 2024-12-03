using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Odev.Data;
using Web_Odev.Models;

namespace Web_Odev.Controllers
{
    [Route("[controller]")]
    public class RandevuController : Controller
    {
        private readonly AppDbContext _context;

        public RandevuController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Randevu Listeleme
        public IActionResult Al()
        {
            // Eğer hiç randevu yoksa örnek veriler oluştur
            if (!_context.Randevular.Any())
            {
                // Önce gerekli ilişkili verileri kontrol edin ve ekleyin
                if (!_context.Islemler.Any())
                {
                    _context.Islemler.Add(new Islem
                    {
                        Ad = "Saç Kesimi",
                        Sure = 30,
                        Ucret = 100
                    });
                }

                if (!_context.Calisanlar.Any())
                {
                    _context.Calisanlar.Add(new Calisan
                    {
                        Isim = "Ahmet Yılmaz",
                        UzmanlikAlanlari = "Saç Kesimi"
                    });
                }

                if (!_context.Kullanicilar.Any())
                {
                    _context.Kullanicilar.Add(new Kullanici
                    {
                        Isim = "Mehmet Demir"
                    });
                }

                _context.SaveChanges();

                // Örnek randevu oluştur
                var randevu = new Randevu
                {
                    TarihSaat = DateTime.Now.AddDays(1),
                    IslemID = _context.Islemler.First().ID ?? 0,
                    CalisanID = _context.Calisanlar.First().ID ?? 0,
                    KullaniciID = _context.Kullanicilar.First().ID ?? 0
                };

                _context.Randevular.Add(randevu);
                _context.SaveChanges();
            }

            var randevular = _context.Randevular?
                .Include(r => r.Islem)
                .Include(r => r.Calisan)
                .Include(r => r.Kullanici)
                .ToList() ?? new List<Randevu>();

            return View(randevular);
        }

        // Yeni Randevu Alma
        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Islemler = _context.Islemler?.ToList() ?? new List<Islem>();
            ViewBag.Calisanlar = _context.Calisanlar?.ToList() ?? new List<Calisan>();
            ViewBag.Kullanicilar = _context.Kullanicilar?.ToList() ?? new List<Kullanici>();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                _context.Randevular?.Add(randevu);
                _context.SaveChanges();
                return RedirectToAction("Al"); // Index yerine Al metoduna yönlendir
            }
            return View(randevu);
        }
    }
}
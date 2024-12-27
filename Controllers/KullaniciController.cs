using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Odev.Data;
using Web_Odev.Models;

namespace Web_Odev.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly AppDbContext _context;

        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Kullanicis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kullanicilar.ToListAsync());
        }

        // GET: Kullanicis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisanlar.FirstOrDefaultAsync(c => c.ID == id);

            if (calisan == null)
            {
                return NotFound();
            }

            // Uzmanlık Alanları ve Süreleri İşleme
            var uzmanlikAlanlari = !string.IsNullOrEmpty(calisan.UzmanlikAlanlari)
                ? calisan.UzmanlikAlanlari.Split(',').Select(u => u.Trim()).ToList()
                : new List<string>();

            var uzmanlikSureleri = !string.IsNullOrEmpty(calisan.UzmanlikSureleri)
                ? calisan.UzmanlikSureleri.Split(',').Select(s => s.Trim()).ToList()
                : new List<string>();

            // Log kontrolü
            Console.WriteLine("Uzmanlık Alanları: " + string.Join(", ", uzmanlikAlanlari));
            Console.WriteLine("Uzmanlık Süreleri: " + string.Join(", ", uzmanlikSureleri));

            // ViewBag ile verileri View'e gönder
            ViewBag.UzmanlikAlanlari = uzmanlikAlanlari;
            ViewBag.UzmanlikSureleri = uzmanlikSureleri;

            ViewBag.MusaitlikSaatleri = !string.IsNullOrEmpty(calisan.MusaitlikSaatleri)
                ? calisan.MusaitlikSaatleri
                : "Müsaitlik saatleri bulunmuyor.";

            return View(calisan);
        }

        // GET: Kullanicis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kullanicis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Isim,Şifre")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kullanici);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // GET: Kullanicis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return View(kullanici);
        }

        // POST: Kullanicis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Isim,Şifre")] Kullanici kullanici)
        {
            if (id != kullanici.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kullanici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KullaniciExists(kullanici.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // GET: Kullanicis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(m => m.ID == id);
            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // POST: Kullanicis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KullaniciExists(int id)
        {
            return _context.Kullanicilar.Any(e => e.ID == id);
        }

        /// <summary>
        /// Yeni bir kullanıcı kaydı oluşturur.
        /// </summary>
        /// <param name="kullanici">Kullanıcı bilgilerini içeren model.</param>
        /// <returns>Başarılı ise Login sayfasına yönlendirir, aksi halde formu tekrar gösterir.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Kullanici kullanici)
        {
            // Doğrulama
            if (!ModelState.IsValid)
            {
                // Hata mesajlarını loglayın
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Hatası: {error.ErrorMessage}");
                }

                return View(kullanici); // Hatalı durumda formu doldurarak geri döndür
            }

            // Aynı email kontrolü
            var existingUser = await _context.Kullanicilar.FirstOrDefaultAsync(u =>
                u.Email == kullanici.Email
            );

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email adresi zaten kullanılmış.");
                return View(kullanici); // Hatalı durumda formu doldurarak geri döndür
            }

            // Şifreyi hashleyerek kaydet
            var passwordHasher = new PasswordHasher<string>();
            kullanici.Şifre = passwordHasher.HashPassword(null, kullanici.Şifre);

            // Yeni kullanıcı ekleme
            kullanici.Rol = "User"; // Varsayılan rol
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}

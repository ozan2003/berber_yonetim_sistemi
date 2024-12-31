using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Odev.Data;
using Web_Odev.Models;

namespace Web_Odev.Controllers
{
    public class CalisansController : Controller
    {
        private readonly AppDbContext _context;

        public CalisansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Calisans
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Calisanlar.ToListAsync());
        }

        // GET: Calisans/Details/5
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
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

        public IActionResult CalisanList()
        {
            return View(_context.Calisanlar.ToList());
        }

        // GET: Calisans/Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(calisan); // Yeni çalışan ekleniyor
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Çalışan listesine yönlendir
            }
            return View(calisan); // Model hatalıysa tekrar aynı sayfa gösterilir
        }

        // GET: Calisans/Edit/5
        // GET: Edit
        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null) return NotFound();
            return View(calisan);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Isim,UzmanlikAlanlari,UzmanlikSureleri,MusaitlikSaatleri,UzmanlikFiyatlari")] Calisan calisan)
        {
            if (id != calisan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCalisan = await _context.Calisanlar.FindAsync(id);
                    if (existingCalisan == null)
                    {
                        return NotFound();
                    }

                    existingCalisan.Isim = calisan.Isim;
                    existingCalisan.UzmanlikAlanlari = calisan.UzmanlikAlanlari;
                    existingCalisan.UzmanlikSureleri = calisan.UzmanlikSureleri;
                    existingCalisan.MusaitlikSaatleri = calisan.MusaitlikSaatleri;
                    existingCalisan.UzmanlikFiyatlari = calisan.UzmanlikFiyatlari;

                    _context.Update(existingCalisan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalisanExists(calisan.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(calisan);
        }





        // GET: Calisans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisanlar
                .FirstOrDefaultAsync(m => m.ID == id);
            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        // POST: Calisans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan != null)
            {
                _context.Calisanlar.Remove(calisan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalisanExists(int? id)
        {
            return _context.Calisanlar.Any(e => e.ID == id);
        }
    }
}

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
    public class RandevusController : Controller
    {
        private readonly AppDbContext _context;

        public RandevusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Randevus
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Calisan) // Çalışan bilgilerini de getiriyoruz
                .ToListAsync();
            return View(randevular);
        }



        // GET: Randevus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular
                .FirstOrDefaultAsync(m => m.ID == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // GET: Randevu/Create
        [HttpGet]
        [HttpGet]
        public IActionResult Create()
        {
            // Çalışanları Dropdown için hazırlıyoruz
            var calisanlar = _context.Calisanlar
                .Select(c => new SelectListItem
                {
                    Value = c.ID.ToString(),
                    Text = $"{c.Isim} ({c.UzmanlikAlanlari})"
                }).ToList();

            ViewBag.Calisanlar = calisanlar; // View'a gönderiyoruz
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu, string SelectedUzmanlik)
        {
            // Çalışanı veritabanından getir
            var calisan = await _context.Calisanlar.FindAsync(randevu.CalisanId);

            if (calisan == null)
            {
                ModelState.AddModelError("CalisanId", "Geçersiz çalışan seçimi.");
                return View(randevu);
            }

            // Uzmanlık alanlarını ve sürelerini ayır
            var uzmanlikAlanlari = calisan.UzmanlikAlanlari?.Split(',').Select(x => x.Trim()).ToList() ?? new List<string>();
            var uzmanlikSureleri = calisan.UzmanlikSureleri?.Split(',').Select(x => int.Parse(x.Trim())).ToList() ?? new List<int>();

            // Seçilen uzmanlık alanı ve süresini belirle
            int index = uzmanlikAlanlari.IndexOf(SelectedUzmanlik);
            if (index < 0 || index >= uzmanlikSureleri.Count)
            {
                ModelState.AddModelError("Islem", "Geçersiz uzmanlık alanı seçimi.");
                return View(randevu);
            }

            int sure = uzmanlikSureleri[index];
            var randevuBaslangic = randevu.TarihSaat;
            var randevuBitis = randevu.TarihSaat.AddMinutes(sure);

            // Çalışan için çakışma kontrolü
            var calisanRandevular = _context.Randevular
                .Where(r => r.CalisanId == randevu.CalisanId && r.TarihSaat < randevuBitis && r.TarihSaat.AddMinutes(sure) > randevuBaslangic)
                .ToList();

            if (calisanRandevular.Any())
            {
                ModelState.AddModelError("TarihSaat", "Bu çalışanın bu saatlerde başka bir randevusu mevcut.");
                return View(randevu);
            }

            // Genel çakışma kontrolü
            var genelCakisiyorMu = _context.Randevular
                .Any(r => r.TarihSaat < randevuBitis && r.TarihSaat.AddMinutes(sure) > randevuBaslangic);

            if (genelCakisiyorMu)
            {
                ModelState.AddModelError("TarihSaat", "Bu saatlerde başka bir randevu mevcut.");
                return View(randevu);
            }

            // Randevu kaydet
            randevu.Islem = SelectedUzmanlik;
            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);

            if (randevu == null)
            {
                return NotFound();
            }

            // Randevu durumunu güncelle
            randevu.OnayDurumu = "Onaylandi";
            _context.Update(randevu);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Randevu başarıyla onaylandı!";
            return RedirectToAction(nameof(Index));
        }




        // POST: Randevus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TarihSaat")] Randevu randevu)
        {
            if (id != randevu.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.ID))
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
            return View(randevu);
        }



        // POST: Randevus/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
            return _context.Randevular.Any(e => e.ID == id);
        }

        [HttpGet]
        public IActionResult GetUzmanlikAlanlari(int calisanId)
        {
            var calisan = _context.Calisanlar.Find(calisanId);
            if (calisan == null || string.IsNullOrEmpty(calisan.UzmanlikAlanlari))
            {
                return Json(new List<string>());
            }

            var uzmanlikAlanlari = calisan.UzmanlikAlanlari.Split(',').Select(u => u.Trim()).ToList();
            return Json(uzmanlikAlanlari);
        }



    }
}

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

            var calisan = await _context.Calisanlar
                .FirstOrDefaultAsync(m => m.ID == id);

            if (calisan == null)
            {
                return NotFound();
            }

            // ViewBag'e müsaitlik saatleri ve uzmanlık alanlarını ekleyin
            ViewBag.MusaitlikSaatleri = calisan.MusaitlikSaatleri;
            ViewBag.UzmanlikAlanlari = calisan.UzmanlikAlanlari;

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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Isim,UzmanlikAlanlari,MusaitlikSaatleri")] Calisan calisan)
        {
            if (id != calisan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Veritabanından mevcut çalışanı al
                    var existingCalisan = await _context.Calisanlar.FindAsync(id);

                    if (existingCalisan == null)
                    {
                        return NotFound();
                    }

                    // Varolan çalışanın özelliklerini güncelle
                    existingCalisan.Isim = calisan.Isim;
                    existingCalisan.UzmanlikAlanlari = calisan.UzmanlikAlanlari;
                    existingCalisan.MusaitlikSaatleri = calisan.MusaitlikSaatleri;

                    _context.Update(existingCalisan);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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

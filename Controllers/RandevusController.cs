using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Web_Odev.Data;
using Web_Odev.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
        // GET: Randevus
[Authorize]
[HttpGet]
public async Task<IActionResult> Index()
{
    var userEmail = User.Identity.Name;
    Console.WriteLine($"1. Kullanıcı Email: {userEmail}");

    // Admin kontrolü
    if (User.IsInRole("Admin"))
    {
        var adminRandevular = await _context.Randevular
            .Include(r => r.Calisan)
            .ToListAsync();
        return View(adminRandevular);
    }

    // Kullanıcı bilgilerini al
    var kullanici = await _context.Kullanicilar
        .FirstOrDefaultAsync(k => k.Email == userEmail);

    if (kullanici == null)
    {
        Console.WriteLine("Kullanıcı bulunamadı!");
        return View(new List<Randevu>());
    }

    Console.WriteLine($"2. Kullanıcı bulundu: {kullanici.Isim}");

    // Kullanıcının randevularını getir
    var randevular = await _context.Randevular
        .Include(r => r.Calisan)
        .Where(r => r.AdSoyad == kullanici.Isim)
        .ToListAsync();

    Console.WriteLine($"3. Bulunan randevu sayısı: {randevular.Count}");
    foreach (var r in randevular)
    {
        Console.WriteLine($"Randevu: {r.ID} - {r.AdSoyad} - {r.Islem} - {r.TarihSaat}");
    }

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
        public async Task<IActionResult> Create(Randevu randevu, string SelectedUzmanlik, IFormFile PhotoFile)
        {
            var userEmail = User.Identity.Name;
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == userEmail);

            if (kullanici == null)
            {
                ModelState.AddModelError("", "Kullanıcı bilgileri bulunamadı.");
                return View(randevu);
            }

            randevu.AdSoyad = kullanici.Isim;

            // ÇALIŞAN & UZMANLIK KONTROLLERİ
            var calisan = await _context.Calisanlar.FindAsync(randevu.CalisanId);
            if (calisan == null)
            {
                ModelState.AddModelError("CalisanId", "Geçersiz çalışan seçimi.");
                return View(randevu);
            }

            // Müsaitlik saatleri kontrolü
            var musaitlikSaatleri = calisan.MusaitlikSaatleri?.Split(',')
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

            bool saatUygun = false;
            foreach (var musaitlikAraligi in musaitlikSaatleri)
            {
                var saatler = musaitlikAraligi.Split('-');
                if (saatler.Length == 2)
                {
                    if (DateTime.TryParse(saatler[0], out DateTime baslangic) && 
                        DateTime.TryParse(saatler[1], out DateTime bitis))
                    {
                        var randevuSaati = randevu.TarihSaat.TimeOfDay;
                        var musaitlikBaslangic = baslangic.TimeOfDay;
                        var musaitlikBitis = bitis.TimeOfDay;

                        if (randevuSaati >= musaitlikBaslangic && randevuSaati <= musaitlikBitis)
                        {
                            saatUygun = true;
                            break;
                        }
                    }
                }
            }

            if (!saatUygun)
            {
                ModelState.AddModelError("TarihSaat", "Seçilen saat çalışanın müsait olduğu saatler arasında değil.");
                return View(randevu);
            }

            // Diğer kontroller
            var uzmanlikAlanlari = calisan.UzmanlikAlanlari?.Split(',')
                .Select(x => x.Trim()).ToList() ?? new List<string>();
            var uzmanlikSureleri = calisan.UzmanlikSureleri?.Split(',')
                .Select(x => int.Parse(x.Trim())).ToList() ?? new List<int>();

            int index = uzmanlikAlanlari.IndexOf(SelectedUzmanlik);
            if (index < 0 || index >= uzmanlikSureleri.Count)
            {
                ModelState.AddModelError("Islem", "Geçersiz uzmanlık alanı seçimi.");
                return View(randevu);
            }

            int sure = uzmanlikSureleri[index];
            var randevuBaslangic = randevu.TarihSaat;
            var randevuBitis = randevu.TarihSaat.AddMinutes(sure);

            var calisanRandevular = _context.Randevular
                .Where(r => r.CalisanId == randevu.CalisanId
                    && r.TarihSaat < randevuBitis
                    && r.TarihSaat.AddMinutes(sure) > randevuBaslangic)
                .ToList();

            if (calisanRandevular.Any())
            {
                ModelState.AddModelError("TarihSaat", "Bu çalışanın bu saatlerde başka bir randevusu mevcut.");
                return View(randevu);
            }

            var genelCakisiyorMu = _context.Randevular
                .Any(r => r.TarihSaat < randevuBitis
                    && r.TarihSaat.AddMinutes(sure) > randevuBaslangic);
            if (genelCakisiyorMu)
            {
                ModelState.AddModelError("TarihSaat", "Bu saatlerde başka bir randevu mevcut.");
                return View(randevu);
            }

            randevu.Islem = SelectedUzmanlik;
            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();
            Console.WriteLine("5. Create - Randevu kaydedildi");

            // FOTOĞRAF İŞLEMLERİ
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                try
                {
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(wwwRootPath))
                        Directory.CreateDirectory(wwwRootPath);

                    var originalFileName = Path.GetFileName(PhotoFile.FileName);
                    var localFilePath = Path.Combine(wwwRootPath, originalFileName);

                    // Save the uploaded file
                    using (var stream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(stream);
                    }

                    // Create the API request
                    using var client = new HttpClient();
                    using var request = new HttpRequestMessage(HttpMethod.Post,
                        "https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle");

                    request.Headers.Add("X-RapidAPI-Key", "4153a9853cmsh529d90af3366380p151f08jsn156dcfd4c127");
                    request.Headers.Add("X-RapidAPI-Host", "hairstyle-changer.p.rapidapi.com");

                    // Create multipart form content
                    var multipartContent = new MultipartFormDataContent();

                    // Read file as bytes and create content
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(localFilePath);
                    var imageContent = new ByteArrayContent(imageBytes);
                     imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    // Important: The API expects these exact form field names
                    multipartContent.Add(imageContent, "image_target", originalFileName);
                    multipartContent.Add(new StringContent("801"), "hair_type");

                    request.Content = multipartContent;

                    // Send request and process response
                    using (var response = await client.SendAsync(request))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError("", $"API Hatası: {response.StatusCode} - {errorContent}");
                            return View(randevu);
                        }

                        var body = await response.Content.ReadAsStringAsync();
                        dynamic values = JsonConvert.DeserializeObject(body);
                        string base64Image = values.data.image;

                        // Save the processed image
                        byte[] processedImageBytes = Convert.FromBase64String(base64Image);
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                        var newFileName = fileNameWithoutExt + "_processed.jpg";
                        var newFilePath = Path.Combine(wwwRootPath, newFileName);

                        await System.IO.File.WriteAllBytesAsync(newFilePath, processedImageBytes);
                        ViewBag.image = newFileName;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"İşlem sırasında hata: {ex.Message}");
                    return View(randevu);
                }
            }

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
                return Json(new List<object>());
            }

            var uzmanlikAlanlari = calisan.UzmanlikAlanlari.Split(',').Select(u => u.Trim()).ToList();
            var uzmanlikFiyatlari = calisan.UzmanlikFiyatlari?.Split(',').Select(f => f.Trim()).ToList() ?? new List<string>();

            var result = uzmanlikAlanlari.Select((alan, index) => new
            {
                alan = alan,
                fiyat = index < uzmanlikFiyatlari.Count ? uzmanlikFiyatlari[index] : "0"
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> PreviewPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return BadRequest("Fotoğraf bulunamadı veya boş.");
            }

            try
            {
                // 1) Dosyayı geçici olarak kaydet
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                    Directory.CreateDirectory(wwwRootPath);

                var fileName = Path.GetFileName(photo.FileName);
                var localPath = Path.Combine(wwwRootPath, fileName);
                using (var fs = new FileStream(localPath, FileMode.Create))
                {
                    await photo.CopyToAsync(fs);
                }

                // 2) AI servisine gönder
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle");

                request.Headers.Add("X-RapidAPI-Key", "4153a9853cmsh529d90af3366380p151f08jsn156dcfd4c127");
                request.Headers.Add("X-RapidAPI-Host", "hairstyle-changer.p.rapidapi.com");

                var multipartContent = new MultipartFormDataContent();

                // Fotoğrafı okuyoruz
                var fileBytes = await System.IO.File.ReadAllBytesAsync(localPath);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                // "image_target" ve "hair_type" API'ye özgü parametreler
                multipartContent.Add(fileContent, "image_target", fileName);
                multipartContent.Add(new StringContent("801"), "hair_type");

                request.Content = multipartContent;

                using var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest($"API Hatası: {response.StatusCode} - {errorContent}");
                }

                var body = await response.Content.ReadAsStringAsync();
                dynamic values = JsonConvert.DeserializeObject(body);
                string base64Image = values.data.image;

                // 3) Base64'i JSON olarak dön
                return Json(new { base64Image });
            }
            catch (Exception ex)
            {
                return BadRequest("Hata: " + ex.Message);
            }
        }



    }
}

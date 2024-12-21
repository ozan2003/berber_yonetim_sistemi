using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Odev.Data;
using Web_Odev.Models;

[Route("api/[controller]")]
[ApiController]
public class RandevuApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public RandevuApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/RandevuApi
    [HttpGet]
    public IActionResult GetRandevular()
    {
        var randevular = _context
            .Randevular.Where(r => r.OnayDurumu == "Onaylandi") // Onaylanmış randevuları getir
            .Select(r => new
            {
                r.ID,
                r.AdSoyad,
                r.TarihSaat,
                Calisan = r.Calisan.Isim,
            })
            .ToList();

        return Ok(randevular); // JSON formatında geri döner
    }

    // POST: api/RandevuApi/Onayla/5
    [HttpPost("Onayla/{id}")]
    public async Task<IActionResult> OnaylaRandevu(int id)
    {
        var randevu = await _context.Randevular.FindAsync(id);
        if (randevu == null)
        {
            return NotFound();
        }

        randevu.OnayDurumu = "Onaylandi"; // Randevuyu onayla
        await _context.SaveChangesAsync();
        return Ok(new { message = "Randevu onaylandı!" });
    }

    // GET: api/RandevuApi/UygunCalisanlar
    [HttpGet("UygunCalisanlar")]
    public IActionResult GetUygunCalisanlar([FromQuery] DateTime tarihSaat)
    {
        var uygunCalisanlar = _context
            .Calisanlar.Where(c =>
                !_context.Randevular.Any(r => r.CalisanId == c.ID && r.TarihSaat == tarihSaat)
            ) // Randevusu olmayan çalışanlar
            .Select(c => new { c.ID, c.Isim })
            .ToList();

        return Ok(uygunCalisanlar);
    }
}

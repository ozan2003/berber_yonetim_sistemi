using Microsoft.AspNetCore.Mvc;
using Web_Odev.Data;
using Web_Odev.Models;
using System.Linq;

namespace Web_Odev.Controllers
{
    public class SalonController : Controller
    {
        private readonly AppDbContext _context;

        public SalonController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var salonlar = _context.Salonlar?.ToList() ?? new List<Salon>();
            return View(salonlar);
        }
    }
}


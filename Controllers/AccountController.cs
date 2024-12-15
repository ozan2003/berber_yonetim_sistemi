using Microsoft.AspNetCore.Mvc;
using Web_Odev.Data;
using Web_Odev.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Web_Odev.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            // Aynı email kontrolü
            var existingUser = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email adresi zaten kullanılmış.");
                return View();
            }

            // Yeni kullanıcı ekleme
            var newUser = new Kullanici
            {
                Isim = username,
                Email = email,
                Şifre = password, // Şifreleme eklenebilir
                Rol = "User"
            };

            _context.Kullanicilar.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
        


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            var user = _context.Kullanicilar.FirstOrDefault(k => k.Email == email && k.Şifre == sifre);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Isim),
                    new Claim(ClaimTypes.Role, user.Rol)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
            return View();
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}

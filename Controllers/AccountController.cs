using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Şifre hashing için gerekli
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Odev.Data;
using Web_Odev.Models;

namespace Web_Odev.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<string> _passwordHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<string>();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Hata Mesajı: {error.ErrorMessage}");
                }
                return View(kullanici);
            }

            // Aynı email kontrolü
            var existingUser = await _context.Kullanicilar.FirstOrDefaultAsync(u =>
                u.Email == kullanici.Email
            );

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email adresi zaten kullanılmış.");
                return View(kullanici);
            }

            // Şifre hashleme
            var passwordHasher = new PasswordHasher<string>();
            kullanici.Şifre = passwordHasher.HashPassword(null, kullanici.Şifre);

            // Yeni kullanıcı ekleme
            kullanici.Rol = "User"; // Varsayılan rol
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcı giriş işlemini gerçekleştirir.
        /// </summary>
        /// <returns>Giriş başarılıysa ana sayfaya yönlendirir, aksi halde giriş sayfasına geri döner.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            var user = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == email);

            if (user != null)
            {
                // Şifre doğrulama
                var verificationResult = _passwordHasher.VerifyHashedPassword(
                    null,
                    user.Şifre,
                    sifre
                );
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    // Kullanıcı girişini yönet
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Rol),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

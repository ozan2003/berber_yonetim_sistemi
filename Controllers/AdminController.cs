using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web_Odev.Controllers
{
    public class AdminController : Controller
    {
        // Admin giriş sayfasını göster
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Giriş işlemi
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Sabit admin kullanıcı bilgileri (projede database ile kontrol edilmeli)
            const string AdminUsername = "G221210093@ogr.sakarya.edu.tr";
            const string AdminPassword = "sau";

            if (username == AdminUsername && password == AdminPassword)
            {
                // Claim oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Oturum kalıcılığını ayarla
                    IsPersistent = true,
                    // Oturum süresini belirle (örn. 2 saat)
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                };

                // Kullanıcıyı oturum aç
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Admin dashboard'a yönlendir
                return RedirectToAction("Dashboard", "Admin");
            }

            // Giriş başarısızsa hata mesajı göster
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            return View();
        }

        // Çıkış işlemi
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Admin");
        }

        // Sadece Admin rolü olan kullanıcılar erişebilir
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        // Yetkisiz erişim sayfası
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
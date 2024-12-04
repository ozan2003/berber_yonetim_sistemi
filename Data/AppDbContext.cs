using Microsoft.EntityFrameworkCore;
using Web_Odev.Models;

namespace Web_Odev.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salon>? Salonlar { get; set; }
        public DbSet<Calisan>? Calisanlar { get; set; }
        public DbSet<Islem>? Islemler { get; set; }
        public DbSet<Randevu>? Randevular { get; set; }
        public DbSet<Kullanici>? Kullanicilar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salon>().HasData(
                new Salon {ID = 1}
            );

            modelBuilder.Entity<Islem>().HasData(
                new Islem {ID = 1}
            );
        }


    }

}


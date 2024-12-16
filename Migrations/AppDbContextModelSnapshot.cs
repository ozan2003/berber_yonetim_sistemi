﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web_Odev.Data;

#nullable disable

namespace Web_Odev.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Web_Odev.Models.Calisan", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Isim")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MusaitlikSaatleri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SalonID")
                        .HasColumnType("int");

                    b.Property<string>("UzmanlikAlanlari")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("SalonID");

                    b.ToTable("Calisanlar");
                });

            modelBuilder.Entity("Web_Odev.Models.Islem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Sure")
                        .HasColumnType("int");

                    b.Property<decimal?>("Ucret")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID");

                    b.ToTable("Islemler");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Ad = "İşlem",
                            Sure = 0
                        });
                });

            modelBuilder.Entity("Web_Odev.Models.Kullanici", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Isim")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Şifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("Web_Odev.Models.Randevu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AdSoyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CalisanId")
                        .HasColumnType("int");

                    b.Property<string>("Islem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TarihSaat")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CalisanId");

                    b.ToTable("Randevular");
                });

            modelBuilder.Entity("Web_Odev.Models.Salon", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CalismaSaatleri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Isim")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.ToTable("Salonlar");

                    b.HasData(
                        new
                        {
                            ID = 1
                        });
                });

            modelBuilder.Entity("Web_Odev.Models.Calisan", b =>
                {
                    b.HasOne("Web_Odev.Models.Salon", "Salon")
                        .WithMany()
                        .HasForeignKey("SalonID");

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("Web_Odev.Models.Randevu", b =>
                {
                    b.HasOne("Web_Odev.Models.Calisan", "Calisan")
                        .WithMany("Randevular")
                        .HasForeignKey("CalisanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calisan");
                });

            modelBuilder.Entity("Web_Odev.Models.Calisan", b =>
                {
                    b.Navigation("Randevular");
                });
#pragma warning restore 612, 618
        }
    }
}

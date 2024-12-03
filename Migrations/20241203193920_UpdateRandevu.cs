using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Odev.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRandevu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "saat",
                table: "Randevular");

            migrationBuilder.AlterColumn<int>(
                name: "CalisanID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IslemID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KullaniciID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarihSaat",
                table: "Randevular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Salonlar",
                keyColumn: "ID",
                keyValue: 1,
                column: "CalismaSaatleri",
                value: " 09:00 - 18:00");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_IslemID",
                table: "Randevular",
                column: "IslemID");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_KullaniciID",
                table: "Randevular",
                column: "KullaniciID");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanID",
                table: "Randevular",
                column: "CalisanID",
                principalTable: "Calisanlar",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Islemler_IslemID",
                table: "Randevular",
                column: "IslemID",
                principalTable: "Islemler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Kullanicilar_KullaniciID",
                table: "Randevular",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanID",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Islemler_IslemID",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Kullanicilar_KullaniciID",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_IslemID",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_KullaniciID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "IslemID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "KullaniciID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "TarihSaat",
                table: "Randevular");

            migrationBuilder.AlterColumn<int>(
                name: "CalisanID",
                table: "Randevular",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "saat",
                table: "Randevular",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Salonlar",
                keyColumn: "ID",
                keyValue: 1,
                column: "CalismaSaatleri",
                value: "09:00 - 18:00");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanID",
                table: "Randevular",
                column: "CalisanID",
                principalTable: "Calisanlar",
                principalColumn: "ID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Odev.Migrations
{
    /// <inheritdoc />
    public partial class YeniMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalismaSaatleri",
                table: "Salonlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isim",
                table: "Salonlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "ID", "Ad", "Sure", "Ucret" },
                values: new object[] { 1, "Saç Kesimi", 30, 50m });

            migrationBuilder.InsertData(
                table: "Salonlar",
                columns: new[] { "ID", "CalismaSaatleri", "Isim", "SalonID" },
                values: new object[] { 1, "09:00 - 18:00", "Örnek Salon", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Salonlar",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CalismaSaatleri",
                table: "Salonlar");

            migrationBuilder.DropColumn(
                name: "Isim",
                table: "Salonlar");
        }
    }
}

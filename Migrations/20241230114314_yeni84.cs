using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Odev.Migrations
{
    /// <inheritdoc />
    public partial class yeni84 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UzmanlikFiyatlari",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UzmanlikFiyatlari",
                table: "Calisanlar");
        }
    }
}

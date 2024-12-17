using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Odev.Migrations
{
    /// <inheritdoc />
    public partial class yeni2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Randevular");
        }
    }
}

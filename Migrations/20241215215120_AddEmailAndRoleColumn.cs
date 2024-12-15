using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Odev.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAndRoleColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CalisanID",
                table: "Randevular",
                newName: "CalisanId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_CalisanID",
                table: "Randevular",
                newName: "IX_Randevular_CalisanId");

            migrationBuilder.AlterColumn<int>(
                name: "CalisanId",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Islem",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanId",
                table: "Randevular",
                column: "CalisanId",
                principalTable: "Calisanlar",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Calisanlar_CalisanId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Islem",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "CalisanId",
                table: "Randevular",
                newName: "CalisanID");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_CalisanId",
                table: "Randevular",
                newName: "IX_Randevular_CalisanID");

            migrationBuilder.AlterColumn<int>(
                name: "CalisanID",
                table: "Randevular",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IslemID",
                table: "Randevular",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KullaniciID",
                table: "Randevular",
                type: "int",
                nullable: true);

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
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Islemler_IslemID",
                table: "Randevular",
                column: "IslemID",
                principalTable: "Islemler",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Kullanicilar_KullaniciID",
                table: "Randevular",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "ID");
        }
    }
}

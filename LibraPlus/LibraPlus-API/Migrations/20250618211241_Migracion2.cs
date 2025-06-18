using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraPlus_API.Migrations
{
    /// <inheritdoc />
    public partial class Migracion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescargaURL",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "EsDigital",
                table: "Compras",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Compras_LibroID",
                table: "Compras",
                column: "LibroID");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioID",
                table: "Compras",
                column: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Libros_LibroID",
                table: "Compras",
                column: "LibroID",
                principalTable: "Libros",
                principalColumn: "LibroID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Usuarios_UsuarioID",
                table: "Compras",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Libros_LibroID",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Usuarios_UsuarioID",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_LibroID",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_UsuarioID",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "EsDigital",
                table: "Compras");

            migrationBuilder.AlterColumn<string>(
                name: "DescargaURL",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

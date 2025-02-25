using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario360.Migrations
{
    /// <inheritdoc />
    public partial class SinProductoEnSolicitudDeMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalidaDeBodega_Producto_Producto",
                table: "SalidaDeBodega");

            migrationBuilder.DropIndex(
                name: "IX_SalidaDeBodega_Producto",
                table: "SalidaDeBodega");

            migrationBuilder.DropColumn(
                name: "Producto",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "SalidaDeBodega");

            migrationBuilder.DropColumn(
                name: "Producto",
                table: "SalidaDeBodega");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "SolicitudDeMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "SolicitudDeMaterial",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Solicitante",
                table: "SolicitudDeMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Proveedor",
                table: "Producto",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DetalleSalidaDeBodega",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalidaDeBodegaID = table.Column<int>(type: "int", nullable: false),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleSalidaDeBodega", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DetalleSalidaDeBodega_Producto_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Producto",
                        principalColumn: "ITEM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleSalidaDeBodega_SalidaDeBodega_SalidaDeBodegaID",
                        column: x => x.SalidaDeBodegaID,
                        principalTable: "SalidaDeBodega",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidaDeBodega_ProductoID",
                table: "DetalleSalidaDeBodega",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidaDeBodega_SalidaDeBodegaID",
                table: "DetalleSalidaDeBodega",
                column: "SalidaDeBodegaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleSalidaDeBodega");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Solicitante",
                table: "SolicitudDeMaterial");

            migrationBuilder.AddColumn<int>(
                name: "Producto",
                table: "SolicitudDeMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "SalidaDeBodega",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Producto",
                table: "SalidaDeBodega",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Proveedor",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalidaDeBodega_Producto",
                table: "SalidaDeBodega",
                column: "Producto");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaDeBodega_Producto_Producto",
                table: "SalidaDeBodega",
                column: "Producto",
                principalTable: "Producto",
                principalColumn: "ITEM");
        }
    }
}

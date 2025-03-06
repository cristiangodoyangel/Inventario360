using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario360.Migrations
{
    /// <inheritdoc />
    public partial class FixSolicitudDeMaterial_Final3 : Migration
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
                name: "Cantidad",
                table: "SalidaDeBodega");

            migrationBuilder.DropColumn(
                name: "Producto",
                table: "SalidaDeBodega");

           

       


            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Producto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Proveedor",
                table: "Producto",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreTecnico",
                table: "Producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Medida",
                table: "Producto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Producto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

          

       

           

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidaDeBodega_ProductoID",
                table: "DetalleSalidaDeBodega",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidaDeBodega_SalidaDeBodegaID",
                table: "DetalleSalidaDeBodega",
                column: "SalidaDeBodegaID");

            migrationBuilder.CreateIndex(
                name: "IX_FichaEmpleado_EmpleadoID",
                table: "FichaEmpleado",
                column: "EmpleadoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleSalidaDeBodega");

            migrationBuilder.DropTable(
                name: "FichaEmpleado");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Solicitante",
                table: "SolicitudDeMaterial");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Producto");

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
                name: "UnidadMedida",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Proveedor",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreTecnico",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Medida",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
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

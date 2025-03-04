using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario360.Migrations
{
    /// <inheritdoc />
    public partial class AddDetalleSalidaDeBodega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ Solo agregar lo que no existe en la BD

            migrationBuilder.CreateIndex(
                name: "IX_SalidaDeBodega_ProyectoAsignado",
                table: "SalidaDeBodega",
                column: "ProyectoAsignado");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaDeBodega_ResponsableEntrega",
                table: "SalidaDeBodega",
                column: "ResponsableEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaDeBodega_Solicitante",
                table: "SalidaDeBodega",
                column: "Solicitante");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaDeBodega_Empleado_ResponsableEntrega",
                table: "SalidaDeBodega",
                column: "ResponsableEntrega",
                principalTable: "Empleado",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaDeBodega_Empleado_Solicitante",
                table: "SalidaDeBodega",
                column: "Solicitante",
                principalTable: "Empleado",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaDeBodega_Proyecto_ProyectoAsignado",
                table: "SalidaDeBodega",
                column: "ProyectoAsignado",
                principalTable: "Proyecto",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalidaDeBodega_Empleado_ResponsableEntrega",
                table: "SalidaDeBodega");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidaDeBodega_Empleado_Solicitante",
                table: "SalidaDeBodega");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidaDeBodega_Proyecto_ProyectoAsignado",
                table: "SalidaDeBodega");

            migrationBuilder.DropIndex(
                name: "IX_SalidaDeBodega_ProyectoAsignado",
                table: "SalidaDeBodega");

            migrationBuilder.DropIndex(
                name: "IX_SalidaDeBodega_ResponsableEntrega",
                table: "SalidaDeBodega");

            migrationBuilder.DropIndex(
                name: "IX_SalidaDeBodega_Solicitante",
                table: "SalidaDeBodega");
        }
    }
}

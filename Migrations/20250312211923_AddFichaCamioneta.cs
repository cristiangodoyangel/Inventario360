using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario360.Migrations
{
    /// <inheritdoc />
    public partial class AddFichaCamioneta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "FichaCamionetas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Año = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Kilometraje = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ResponsableID = table.Column<int>(type: "int", nullable: false),
                    FechaMantenimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaPermisoCirculacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaSoap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAntivuelcos = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaLaminas = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaGPS = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Acreditacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichaCamionetas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FichaCamionetas_Empleado_ResponsableID",
                        column: x => x.ResponsableID,
                        principalTable: "Empleado",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Camionetas_ResponsableID",
                table: "Camionetas",
                column: "ResponsableID");

            migrationBuilder.CreateIndex(
                name: "IX_FichaCamionetas_ResponsableID",
                table: "FichaCamionetas",
                column: "ResponsableID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Camionetas");

            migrationBuilder.DropTable(
                name: "FichaCamionetas");
        }
    }
}

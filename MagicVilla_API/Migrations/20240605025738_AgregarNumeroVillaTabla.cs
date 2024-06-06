using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "numeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_numeroVillas_villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 6, 4, 22, 57, 36, 941, DateTimeKind.Local).AddTicks(271), new DateTime(2024, 6, 4, 22, 57, 36, 941, DateTimeKind.Local).AddTicks(259) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 6, 4, 22, 57, 36, 941, DateTimeKind.Local).AddTicks(275), new DateTime(2024, 6, 4, 22, 57, 36, 941, DateTimeKind.Local).AddTicks(274) });

            migrationBuilder.CreateIndex(
                name: "IX_numeroVillas_VillaId",
                table: "numeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "numeroVillas");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9331), new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9315) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9335), new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9334) });
        }
    }
}

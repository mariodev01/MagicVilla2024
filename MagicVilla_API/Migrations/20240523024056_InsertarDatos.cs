using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class InsertarDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 2, "", "La villa del detalle", new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9331), new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9315), "", 100.0, "Villa real", 10, 100.0 },
                    { 3, "", "La villa del detalle", new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9335), new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9334), "", 100.0, "Villa real", 10, 100.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

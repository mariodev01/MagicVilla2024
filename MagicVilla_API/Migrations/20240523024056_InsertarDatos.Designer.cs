﻿// <auto-generated />
using System;
using MagicVilla_API.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVilla_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240523024056_InsertarDatos")]
    partial class InsertarDatos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MetrosCuadrados")
                        .HasColumnType("float");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "La villa del detalle",
                            FechaActualizacion = new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9331),
                            FechaCreacion = new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9315),
                            ImagenUrl = "",
                            MetrosCuadrados = 100.0,
                            Nombre = "Villa real",
                            Ocupantes = 10,
                            Tarifa = 100.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "La villa del detalle",
                            FechaActualizacion = new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9335),
                            FechaCreacion = new DateTime(2024, 5, 22, 22, 40, 55, 625, DateTimeKind.Local).AddTicks(9334),
                            ImagenUrl = "",
                            MetrosCuadrados = 100.0,
                            Nombre = "Villa real",
                            Ocupantes = 10,
                            Tarifa = 100.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

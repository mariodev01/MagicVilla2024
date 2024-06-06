using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Villa> villas { get; set; }

        public DbSet<NumeroVilla> numeroVillas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa real",
                    Detalle = "La villa del detalle",
                    Tarifa = 100,
                    Ocupantes = 10,
                    MetrosCuadrados = 100,
                    ImagenUrl = "",
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa real",
                    Detalle = "La villa del detalle",
                    Tarifa = 100,
                    Ocupantes = 10,
                    MetrosCuadrados = 100,
                    ImagenUrl = "",
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }
            );
        }
    }   
}

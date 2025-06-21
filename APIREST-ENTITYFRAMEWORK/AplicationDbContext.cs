using Microsoft.EntityFrameworkCore;
using ApiRest_EntityFramework.Models;

namespace ApiRest_EntityFramework
{

    // DEFINE EL CONTEXTO DE LA BASE DE DATOS HEREDANDO DE DBCONTEXT
    public class AplicationDbContext : DbContext
    {

        // DEFINE EL DBSET PARA LAS TABLAS
        public DbSet<Equipo> equipos { get; set; }
        public DbSet<Jugador> jugadores { get; set; }


        // CONFIGURA LA CONEXIÓN A LA BASE DE DATOS USANDO LA CADENA DE CONEXIÓN DESDE APPSETTINGS.JSON
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }


        // CONFIGURACION DE LA RELACION PARA QUE IDEQUIPO EN JUGADOR PUEDA SER NULO
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jugador>()
                .HasOne(j => j.equipo)  
                .WithMany(e => e.jugadores)  
                .HasForeignKey(j => j.idEquipo)  
                .OnDelete(DeleteBehavior.SetNull);  
        }
    }

}

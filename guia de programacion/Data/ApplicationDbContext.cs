using Microsoft.EntityFrameworkCore;
using spotify.Models;
namespace spotify.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancion> Canciones { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistCancion> PlaylistCanciones { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de llave compuesta para la tabla intermedia
            modelBuilder.Entity<PlaylistCancion>()
                .HasKey(pc => new { pc.PlaylistId, pc.CancionId });

            // SEEDING: Insertar el Administrador único automáticamente
            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                Id = 1, // ID fijo para que el ORM no cree duplicados
                Nombre = "Administrador Sistema",
                Username = "admin",
                Password = "admin123", // Recuerda que es el que usarás en el Login
                EsAdmin = true
            });
        }
    }
}
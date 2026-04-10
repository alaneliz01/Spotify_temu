using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class InicioModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public InicioModel(ApplicationDbContext context) => _context = context;

        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        public List<Usuario> ArtistasRandom { get; set; } = new List<Usuario>(); // Nueva lista
        public string NombreUsuario { get; set; }

        public async Task OnGetAsync()
        {
            NombreUsuario = HttpContext.Session.GetString("UsuarioNombre") ?? "Usuario";

            // 1. Cargamos canciones
            Canciones = await _context.Canciones.ToListAsync();

            // 2. Cargamos artistas aleatorios (Usuarios que tienen al menos una canción o son marcados como artistas)
            // Usamos Guid.NewGuid() para ordenar al azar en SQL Server
            ArtistasRandom = await _context.Usuarios.Where(u => _context.Canciones.Any(c => c.Artista == u.Nombre)).OrderBy(r => Guid.NewGuid()).Take(6).ToListAsync();
        }
    }
}
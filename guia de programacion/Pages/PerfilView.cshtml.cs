using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class PerfilViewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PerfilViewModel(ApplicationDbContext context) => _context = context;

        public Usuario ArtistaInfo { get; set; }
        public List<Cancion> CancionesArtista { get; set; }
        public string NombreArtista { get; set; }

        public async Task<IActionResult> OnGetAsync(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return RedirectToPage("/Inicio");

            NombreArtista = nombre;

            // Buscamos los datos del perfil del artista (foto, verificado, etc)
            ArtistaInfo = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre.ToLower() == nombre.ToLower());

            // Filtramos sus canciones
            CancionesArtista = await _context.Canciones
                .Where(c => c.Artista.ToLower() == nombre.ToLower())
                .ToListAsync();

            return Page();
        }
    }
}
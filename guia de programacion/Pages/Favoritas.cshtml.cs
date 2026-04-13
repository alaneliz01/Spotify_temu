using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class FavoritasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FavoritasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista de favoritos que cargaremos de la DB
        public List<Favorito> MisFavoritos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Obtenemos el ID del usuario desde la sesión (como haces en tu lógica de perfil)
            var userIdStr = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(userIdStr))
            {
                // Si no hay sesión, lo mandamos al login (Index)
                return RedirectToPage("/Index");
            }

            int usuarioId = int.Parse(userIdStr);

            // Filtramos los favoritos del usuario e incluimos la data de la canción
            MisFavoritos = await _context.Favoritos
                .Include(f => f.Cancion)
                .Where(f => f.UsuarioId == usuarioId)
                .ToListAsync();

            return Page();
        }
    }
}
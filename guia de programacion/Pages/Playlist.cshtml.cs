using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class PlaylistModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PlaylistModel(ApplicationDbContext context) => _context = context;

        public Playlist Playlist { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Playlist = await _context.Playlists
                .Include(p => p.PlaylistCanciones)
                    .ThenInclude(pc => pc.Cancion)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Playlist == null) return RedirectToPage("/Biblioteca");

            return Page();
        }

        // Método para eliminar la playlist
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                var relaciones = _context.PlaylistCanciones.Where(pc => pc.PlaylistId == id);
                _context.PlaylistCanciones.RemoveRange(relaciones);
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Biblioteca");
        }
    }
}
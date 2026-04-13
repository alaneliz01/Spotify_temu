using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class BibliotecaModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BibliotecaModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public List<Playlist> MisPlaylists { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Index");

            int usuarioId = int.Parse(userIdStr);
            MisPlaylists = await _context.Playlists
                .Include(p => p.PlaylistCanciones)
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCrearPlaylistAsync(string nombrePlaylist, IFormFile fotoPlaylist)
        {
            var userIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Index");

            string rutaImagen = "/img/default-playlist.png";

            if (fotoPlaylist != null)
            {
                string folder = Path.Combine(_environment.WebRootPath, "uploads", "playlists");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fotoPlaylist.FileName);
                string fullPath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await fotoPlaylist.CopyToAsync(stream);
                }
                rutaImagen = "/uploads/playlists/" + fileName;
            }

            var nueva = new Playlist { Nombre = nombrePlaylist, UsuarioId = int.Parse(userIdStr), Fotoplaylist = rutaImagen };
            _context.Playlists.Add(nueva);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarPlaylistAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                // También borramos las relaciones de canciones para no dejar basura
                var relaciones = _context.PlaylistCanciones.Where(pc => pc.PlaylistId == id);
                _context.PlaylistCanciones.RemoveRange(relaciones);

                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
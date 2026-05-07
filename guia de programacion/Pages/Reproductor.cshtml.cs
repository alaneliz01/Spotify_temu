using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class ReproductorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReproductorModel(ApplicationDbContext context) => _context = context;

        public Cancion Cancion { get; set; }
        public List<Cancion> ListaReproduccion { get; set; } = new();
        public List<Playlist> MisPlaylists { get; set; } = new();
        public List<int> PlaylistsIdsConEstaCancion { get; set; } = new();
        public bool ArtistaVerificado { get; set; }
        public string FotoArtista { get; set; }
        public int SiguienteCancionId { get; set; }
        public int AnteriorCancionId { get; set; }
        public bool EsFavoritaDelUsuario { get; set; }

        [BindProperty(SupportsGet = true)]
        public double TiempoActual { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Contexto { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Valor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UsuarioId");
            int? usuarioId = !string.IsNullOrEmpty(userIdStr) ? int.Parse(userIdStr) : null;

            // CARGAR LISTA SEGÚN CONTEXTO
            if (Contexto == "playlist" && !string.IsNullOrEmpty(Valor))
            {
                int pId = int.Parse(Valor);
                ListaReproduccion = await _context.PlaylistCanciones.Where(pc => pc.PlaylistId == pId).Include(pc => pc.Cancion).Select(pc => pc.Cancion).ToListAsync();
            }
            else if (Contexto == "favoritos" && usuarioId.HasValue)
            {
                ListaReproduccion = await _context.Favoritos.Where(f => f.UsuarioId == usuarioId.Value).Include(f => f.Cancion).Select(f => f.Cancion).ToListAsync();
            }
            else if (Contexto == "genero" && !string.IsNullOrEmpty(Valor))
            {
                ListaReproduccion = await _context.Canciones
                                                .Where(c => c.Genero == Valor)
                                                .ToListAsync();
            }
            else if (Contexto == "artista" && !string.IsNullOrEmpty(Valor))
            {
                ListaReproduccion = await _context.Canciones
                                                .Where(c => c.Artista.ToLower() == Valor.ToLower())
                                                .ToListAsync();
            }
            else if(Contexto == "busqueda" && !string.IsNullOrEmpty(Valor))
            {
                ListaReproduccion = await _context.Canciones
                    .Where(c => c.Titulo.Contains(Valor) ||
                                c.Artista.Contains(Valor) ||
                                c.Genero == Valor)
                    .ToListAsync();
            }
            else
            {
                ListaReproduccion = await _context.Canciones.ToListAsync();
            }
            Cancion = await _context.Canciones.FirstOrDefaultAsync(c => c.Id == id);
            if (Cancion == null) return RedirectToPage("/Inicio");

            if (usuarioId.HasValue)
            {
                EsFavoritaDelUsuario = await _context.Favoritos.AnyAsync(f => f.UsuarioId == usuarioId.Value && f.CancionId == id);
                MisPlaylists = await _context.Playlists.Where(p => p.UsuarioId == usuarioId.Value).ToListAsync();
                PlaylistsIdsConEstaCancion = await _context.PlaylistCanciones.Where(pc => pc.CancionId == id).Select(pc => pc.PlaylistId).ToListAsync();
            }

            var index = ListaReproduccion.FindIndex(c => c.Id == id);
            if (index == -1) index = 0;
            SiguienteCancionId = index < ListaReproduccion.Count - 1 ? ListaReproduccion[index + 1].Id : ListaReproduccion[0].Id;
            AnteriorCancionId = index > 0 ? ListaReproduccion[index - 1].Id : ListaReproduccion.Last().Id;

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre.ToLower() == Cancion.Artista.ToLower());
            ArtistaVerificado = user?.EsVerificado ?? false;
            FotoArtista = user?.FotoPerfil ?? "/img/default-user.png";

            return Page();
        }

        public async Task<IActionResult> OnPostAgregarAPlaylistAsync(int cancionId, int playlistId, double tiempo, string contexto, string valor)
        {
            var pc = await _context.PlaylistCanciones.FirstOrDefaultAsync(x => x.PlaylistId == playlistId && x.CancionId == cancionId);
            if (pc != null) _context.PlaylistCanciones.Remove(pc);
            else _context.PlaylistCanciones.Add(new PlaylistCancion { PlaylistId = playlistId, CancionId = cancionId });

            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = cancionId, tiempoActual = tiempo, contexto, valor });
        }

        public async Task<IActionResult> OnPostToggleFavoritoAsync(int id, double tiempo, string contexto, string valor)
        {
            var userIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Index");
            int uId = int.Parse(userIdStr);
            var fav = await _context.Favoritos.FirstOrDefaultAsync(f => f.UsuarioId == uId && f.CancionId == id);
            if (fav != null) _context.Favoritos.Remove(fav);
            else _context.Favoritos.Add(new Favorito { UsuarioId = uId, CancionId = id });
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id, tiempoActual = tiempo, contexto, valor });
        }
    }
}
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
        public List<Cancion> ListaReproduccion { get; set; }
        public bool ArtistaVerificado { get; set; }
        public string FotoArtista { get; set; }
        public int SiguienteCancionId { get; set; }
        public int AnteriorCancionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public double TiempoActual { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Contexto { get; set; } // "artista" o "genero"

        [BindProperty(SupportsGet = true)]
        public string Valor { get; set; } // Nombre del artista o género

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 1. Filtrar lista según el contexto
            IQueryable<Cancion> query = _context.Canciones;

            if (Contexto == "artista" && !string.IsNullOrEmpty(Valor))
                query = query.Where(c => c.Artista == Valor);
            else if (Contexto == "genero" && !string.IsNullOrEmpty(Valor))
                query = query.Where(c => c.Genero == Valor);

            ListaReproduccion = await query.ToListAsync();
            Cancion = await _context.Canciones.FirstOrDefaultAsync(c => c.Id == id);

            if (Cancion == null) return RedirectToPage("/Inicio");

            // 2. Navegación sobre la lista filtrada
            var index = ListaReproduccion.FindIndex(c => c.Id == id);
            if (index == -1)
            { // Por si acaso la canción no está en el contexto
                ListaReproduccion = await _context.Canciones.ToListAsync();
                index = ListaReproduccion.FindIndex(c => c.Id == id);
            }

            SiguienteCancionId = index < ListaReproduccion.Count - 1 ? ListaReproduccion[index + 1].Id : ListaReproduccion[0].Id;
            AnteriorCancionId = index > 0 ? ListaReproduccion[index - 1].Id : ListaReproduccion.Last().Id;

            // 3. Info del Artista
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre.ToLower() == Cancion.Artista.ToLower());
            ArtistaVerificado = user?.EsVerificado ?? false;
            FotoArtista = user?.FotoPerfil ?? "https://via.placeholder.com/150/282828/ffffff?text=U";

            return Page();
        }

        public async Task<IActionResult> OnPostToggleFavoritoAsync(int id, double tiempo, string contexto, string valor)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion != null)
            {
                cancion.EsFavorito = !cancion.EsFavorito;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { id = id, tiempoActual = tiempo, contexto = contexto, valor = valor });
        }
    }
}
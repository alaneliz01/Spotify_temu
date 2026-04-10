using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class BuscarModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public BuscarModel(ApplicationDbContext context) => _context = context;

        public List<Cancion> Resultados { get; set; } = new List<Cancion>();

        // Lista para las sugerencias del buscador
        public List<string> Sugerencias { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string TerminoBusqueda { get; set; }

        public async Task OnGetAsync()
        {
            var titulos = await _context.Canciones.Select(c => c.Titulo).ToListAsync();
            var artistas = await _context.Canciones.Select(c => c.Artista).Distinct().ToListAsync();
            Sugerencias = titulos.Concat(artistas).Distinct().OrderBy(s => s).ToList();

            if (!string.IsNullOrEmpty(TerminoBusqueda))
            {
                Resultados = await _context.Canciones
                    .Where(c => c.Titulo.Contains(TerminoBusqueda) ||
                                c.Artista.Contains(TerminoBusqueda) ||
                                c.Genero == TerminoBusqueda)
                    .ToListAsync();
            }
        }
    }
}
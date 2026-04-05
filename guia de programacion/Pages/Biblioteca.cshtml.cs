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

        public BibliotecaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Cancion> Canciones { get; set; } = new List<Cancion>();

        // Propiedad opcional por si quieres que el reproductor del Layout funcione aquí también
        public Cancion? CancionSeleccionada { get; set; }

        public async Task OnGetAsync(int? idActual)
        {
            // Cargamos las canciones de SQL Server
            Canciones = await _context.Canciones.ToListAsync();

            if (idActual.HasValue)
            {
                CancionSeleccionada = Canciones.FirstOrDefault(c => c.Id == idActual.Value);
            }
        }
    }
}
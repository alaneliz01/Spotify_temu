using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages.Admin
{
    public class PanelModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PanelModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Cancion> Canciones { get; set; } = new List<Cancion>();

        public async Task OnGetAsync()
        {
            // Cargamos todas las canciones de la base de datos
            Canciones = await _context.Canciones.ToListAsync();
        }

        // MÉTODO PARA ELIMINAR CANCIÓN
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion != null)
            {
                _context.Canciones.Remove(cancion);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        // MÉTODO PARA EDITAR NOMBRE (Rápido)
        public async Task<IActionResult> OnPostActualizarNombreAsync(int id, string nuevoNombre)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion != null && !string.IsNullOrEmpty(nuevoNombre))
            {
                cancion.Titulo = nuevoNombre;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
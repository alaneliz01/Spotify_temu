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

        public ReproductorModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cancion Cancion { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscamos la canción por ID en la base de datos
            Cancion = await _context.Canciones.FirstOrDefaultAsync(m => m.Id == id);

            if (Cancion == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
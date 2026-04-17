using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using spotify.Models;
using spotify.Data;
using Microsoft.EntityFrameworkCore;

namespace spotify.Pages.Admin
{
    public class CancionesAdminModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CancionesAdminModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Cancion> Canciones { get; set; }

        public async Task OnGetAsync()
        {
            Canciones = await _context.Canciones.ToListAsync();
        }

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
    }
}
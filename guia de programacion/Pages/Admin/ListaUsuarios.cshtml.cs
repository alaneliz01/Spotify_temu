using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages.Admin
{
    public class ListaUsuariosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListaUsuariosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public async Task OnGetAsync()
        {
            Usuarios = await _context.Usuarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            // Evitamos eliminar al admin principal (ID 1)
            if (usuario != null && usuario.Id != 1)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTogglePremiumAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // Cambiamos el estado opuesto
                usuario.EsPremium = !usuario.EsPremium;

                // LOGICA IMPORTANTE:
                // Si ahora es Premium pero no tiene plan, le damos uno por defecto (Individual)
                // Si ya no es Premium, su plan vuelve a ser "Gratis"
                if (usuario.EsPremium)
                {
                    if (string.IsNullOrEmpty(usuario.Plan) || usuario.Plan == "Gratis")
                    {
                        usuario.Plan = "Individual";
                    }
                }
                else
                {
                    usuario.Plan = "Gratis";
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
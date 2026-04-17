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

        public async Task<IActionResult> OnPostAsignarPlanAsync(int id, string plan)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                usuario.Plan = plan;
                usuario.EsPremium = (plan != "Gratis");
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { usuarioAbiertoId = id });
        }

        public async Task<IActionResult> OnPostToggleVerificadoAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.EsVerificado = !usuario.EsVerificado;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { usuarioAbiertoId = id });
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null && usuario.Id != 1)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

    }
}
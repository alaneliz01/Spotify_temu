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

        // Carga la lista de usuarios al entrar a la página
        public async Task OnGetAsync()
        {
            Usuarios = await _context.Usuarios.ToListAsync();
        }

        // Maneja la eliminación de usuarios
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            // Seguridad: No permitimos eliminar al Admin principal (ID 1)
            if (usuario != null && usuario.Id != 1)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        // Maneja el botón de DAR PRIME / QUITAR PRIME
        public async Task<IActionResult> OnPostTogglePremiumAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // Solo invertimos el estado Premium
                usuario.EsPremium = !usuario.EsPremium;

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

        // Maneja el botón de VERIFICAR / QUITAR VERIFICADO
        public async Task<IActionResult> OnPostToggleVerificadoAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // Invertimos el estado de verificación (el check azul)
                usuario.EsVerificado = !usuario.EsVerificado;

                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
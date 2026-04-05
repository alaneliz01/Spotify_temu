using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;

namespace spotify.Pages.Premium
{
    public class PlanesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PlanesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NombreUsuario { get; set; }

        public void OnGet()
        {
            // Validamos sesión al entrar
            NombreUsuario = HttpContext.Session.GetString("UsuarioNombre") ?? "Invitado";
        }

        public async Task<IActionResult> OnPostComprarAsync(string tipoPlan)
        {
            // 1. Buscamos el nombre en la sesión
            var nombreSesion = HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(nombreSesion))
            {
                return RedirectToPage("/Index");
            }

            // 2. Buscamos al usuario en la DB
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombreSesion);

            if (usuario != null)
            {
                // 3. Actualizamos sus datos
                usuario.EsPremium = true;
                usuario.Plan = tipoPlan;

                await _context.SaveChangesAsync();

                // 4. Actualizamos la sesión para que el Layout refleje los cambios
                HttpContext.Session.SetString("EsPremium", "true");
                HttpContext.Session.SetString("TipoPlan", tipoPlan);
            }

            return RedirectToPage("/Inicio");
        }
    }
}
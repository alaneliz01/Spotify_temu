using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string ErrorMensaje { get; set; }

    public async Task<IActionResult> OnPostAsync(string usuario, string password)
    {
        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        {
            ErrorMensaje = "No puedes dejar campos vacíos";
            return Page();
        }

        // Buscamos en la base de datos comparando Username y Password
        var usuarioValido = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == usuario && u.Password == password);

        if (usuarioValido != null)
        {
            // SEGURIDAD: Limpiamos cualquier sesión anterior para evitar cruce de datos
            HttpContext.Session.Clear();

            // 1. Guardamos el nombre para el saludo
            HttpContext.Session.SetString("UsuarioNombre", usuarioValido.Nombre);

            // 2. Guardamos el rol de administrador (en minúsculas para comparar fácil en JS/Liquid)
            HttpContext.Session.SetString("EsAdmin", usuarioValido.EsAdmin.ToString().ToLower());

            // 3. NUEVO: Guardamos el estado Premium y el Tipo de Plan desde la DB
            // Esto evita que un usuario normal vea el plan de otro
            HttpContext.Session.SetString("EsPremium", usuarioValido.EsPremium.ToString().ToLower());
            HttpContext.Session.SetString("TipoPlan", usuarioValido.Plan ?? "Gratis");

            // Lógica de Redirección según el Rol
            if (usuarioValido.EsAdmin)
            {
                return RedirectToPage("/Admin/Panel");
            }

            return RedirectToPage("/inicio");
        }

        ErrorMensaje = "Usuario o contraseña incorrectos";
        return Page();
    }
}
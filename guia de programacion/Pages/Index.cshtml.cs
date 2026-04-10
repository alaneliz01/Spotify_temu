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
            HttpContext.Session.SetString("EsAdmin", usuarioValido.EsAdmin.ToString().ToLower());
            HttpContext.Session.SetString("EsPremium", usuarioValido.EsPremium.ToString().ToLower());
            HttpContext.Session.SetString("TipoPlan", usuarioValido.Plan ?? "Gratis");
            HttpContext.Session.SetString("EsVerificado", usuarioValido.EsVerificado ? "True" : "False");
            HttpContext.Session.SetString("UsuarioFoto", usuarioValido.FotoPerfil ?? "");

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
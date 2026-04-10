using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;
using spotify.Services;

namespace spotify.Pages
{
    public class PerfilModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureBlobService _blobService;

        public PerfilModel(ApplicationDbContext context, AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [BindProperty]
        public Usuario Usuario { get; set; }

        [BindProperty]
        public IFormFile? NuevaFoto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var nombre = HttpContext.Session.GetString("UsuarioNombre");
            if (string.IsNullOrEmpty(nombre)) return RedirectToPage("/Index");

            Usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userDb = await _context.Usuarios.FindAsync(Usuario.Id);
            if (userDb == null) return Page();

            // Actualizar contraseña si se cambió
            userDb.Password = Usuario.Password;

            // Actualizar foto si se subió una nueva
            if (NuevaFoto != null)
            {
                userDb.FotoPerfil = await _blobService.SubirArchivoAsync(NuevaFoto);
                // Actualizar sesión para que el Layout refleje el cambio de inmediato
                HttpContext.Session.SetString("UsuarioFoto", userDb.FotoPerfil);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Inicio");
        }
    }
}
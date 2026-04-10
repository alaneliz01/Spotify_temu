using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;
using spotify.Services;

namespace spotify.Pages
{
    public class CreateUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureBlobService _blobService;

        public CreateUserModel(ApplicationDbContext context, AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [BindProperty]
        public Usuario NuevoUsuario { get; set; }

        [BindProperty]
        public IFormFile? FotoSubida { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // 1. VALIDACIÓN: Evitar duplicados
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Nombre.ToLower() == NuevoUsuario.Nombre.ToLower());

            if (existe)
            {
                ModelState.AddModelError(string.Empty, "Este nombre de usuario ya está registrado.");
                return Page();
            }

            try
            {
                NuevoUsuario.Username = NuevoUsuario.Nombre;

                // 2. FOTO OPCIONAL: Subir a Azure solo si seleccionó una
                if (FotoSubida != null)
                {
                    NuevoUsuario.FotoPerfil = await _blobService.SubirArchivoAsync(FotoSubida);
                }
                else
                {
                    // URL de avatar por defecto
                    NuevoUsuario.FotoPerfil = "https://via.placeholder.com/150/282828/ffffff?text=User";
                }

                _context.Usuarios.Add(NuevoUsuario);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return Page();
            }
        }
    }
}
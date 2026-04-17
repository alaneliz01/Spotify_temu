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
            var userIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Index");

            int userId = int.Parse(userIdStr);
            Usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);

            if (Usuario == null) return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userDb = await _context.Usuarios.FindAsync(Usuario.Id);
            if (userDb == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(Usuario.Password))
            {
                userDb.Password = Usuario.Password;
            }
            if (NuevaFoto != null)
            {
                userDb.FotoPerfil = await _blobService.SubirArchivoAsync(NuevaFoto);

                HttpContext.Session.SetString("UsuarioFoto", userDb.FotoPerfil ?? "");
            }

            try
            {
                _context.Usuarios.Update(userDb);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Page();
            }
            return RedirectToPage("/Inicio");
        }
    }
}
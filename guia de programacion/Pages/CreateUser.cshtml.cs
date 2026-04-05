using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class CreateUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateUserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario NuevoUsuario { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Esto nos ayuda a ver si falta algún campo obligatorio en la consola de salida
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                NuevoUsuario.Username = NuevoUsuario.Nombre;

                _context.Usuarios.Add(NuevoUsuario);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // Si hay un error, el programa NO se cerrará. 
                // Podrás ver el error en la variable 'ex' poniendo un breakpoint aquí.
                ModelState.AddModelError(string.Empty, "Error al guardar: " + ex.Message);
                return Page();
            }
        }
    }
}
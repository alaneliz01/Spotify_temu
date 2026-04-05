using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using spotify.Models;
using spotify.Data;
using spotify.Services;
using System.ComponentModel.DataAnnotations;

namespace spotify.Pages.Admin
{
    public class CancionesAdminModel : PageModel
    {
        private readonly AzureBlobService _blobService;
        private readonly ApplicationDbContext _context;

        public CancionesAdminModel(ApplicationDbContext context, AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [BindProperty, Required]
        public string Nombre { get; set; }

        [BindProperty, Required]
        public string Artista { get; set; }

        [BindProperty] // No ponemos Required aquí para manejar la validación manual y evitar errores de casteo
        public IFormFile Archivo { get; set; }

        [BindProperty]
        public IFormFile Portada { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Verificamos que los archivos existan antes de llamar a Azure
            if (Archivo == null || Portada == null)
            {
                ModelState.AddModelError(string.Empty, "Debes seleccionar tanto el MP3 como la Portada.");
                return Page();
            }

            if (!ModelState.IsValid) return Page();

            // 1. Subir a Azure Blob Storage
            var urlAudio = await _blobService.SubirArchivoAsync(Archivo);
            var urlPortada = await _blobService.SubirArchivoAsync(Portada);

            // 2. Crear objeto Cancion para SQL
            var cancion = new Cancion
            {
                Titulo = Nombre,
                Artista = Artista,
                RutaArchivo = urlAudio,
                RutaPortada = urlPortada
            };

            // 3. Guardar en Base de Datos
            _context.Canciones.Add(cancion);
            await _context.SaveChangesAsync();

            // 4. Redirigir al inicio para ver la nueva canción
            return RedirectToPage("/Inicio");
        }
    }
}
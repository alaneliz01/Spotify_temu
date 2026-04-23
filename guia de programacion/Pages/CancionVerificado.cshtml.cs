using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using spotify.Models;
using spotify.Data;
using spotify.Services;
using System.ComponentModel.DataAnnotations;

namespace spotify.Pages
{
    // Esta clase se llama CancionVerificadoModel
    public class CancionVerificadoModel : PageModel
    {
        private readonly AzureBlobService _blobService;
        private readonly ApplicationDbContext _context;

        public CancionVerificadoModel(ApplicationDbContext context, AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [BindProperty, Required]
        public string Nombre { get; set; }

        [BindProperty, Required]
        public string Genero { get; set; }

        [BindProperty]
        public IFormFile Archivo { get; set; }

        [BindProperty]
        public IFormFile Portada { get; set; }

        //ESTE ES EL CODIGO ORIGINAL, LO DE ABAJO LO PUSE PARA HACER PRUEBAS DE SUBIR CANCION 
       // public IActionResult OnGet()
       // {
          //  var esVerificado = HttpContext.Session.GetString("EsVerificado");
           // if (string.IsNullOrEmpty(esVerificado) || !esVerificado.Equals("True", StringComparison.OrdinalIgnoreCase))
           // {
              //  return RedirectToPage("/inicio");
           // }

           // return Page();
       // }
        public IActionResult OnGet()
        {
            HttpContext.Session.SetString("Rol", "Admin"); // forzado para hacer prueba con admin para poder subir porque no deja entrar,
                                                           // no dejaste activo el boton de subir para admin

            var rol = HttpContext.Session.GetString("Rol");
            Console.WriteLine("ROL: " + rol);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Archivo == null || Portada == null) return Page();

            // Aquí el artista es automático
            var artistaSesion = HttpContext.Session.GetString("UsuarioNombre") ?? "Artista Desconocido";

            var urlAudio = await _blobService.SubirArchivoAsync(Archivo);
            var urlPortada = await _blobService.SubirArchivoAsync(Portada);

            var cancion = new Cancion
            {
                Titulo = Nombre,
                Artista = artistaSesion,
                Genero = Genero,
                RutaArchivo = urlAudio,
                RutaPortada = urlPortada
            };

            _context.Canciones.Add(cancion);
            await _context.SaveChangesAsync();
            return RedirectToPage("/inicio");
        }
    }
}
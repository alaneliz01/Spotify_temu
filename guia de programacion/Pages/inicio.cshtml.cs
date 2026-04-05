using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;

namespace spotify.Pages
{
    public class InicioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InicioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NombreUsuario { get; set; } = "Invitado";
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();

        // Esta propiedad es la que el Layout va a leer para poner la música
        public Cancion? CancionSeleccionada { get; set; }

        public async Task OnGetAsync(int? idActual)
        {
            // 1. Cargar todas las canciones de la tabla dbo.Canciones
            Canciones = await _context.Canciones.ToListAsync();

            // 2. Si el usuario hizo clic (idActual tiene valor), buscamos esa canción
            if (idActual.HasValue)
            {
                CancionSeleccionada = Canciones.FirstOrDefault(c => c.Id == idActual.Value);
            }

            NombreUsuario = HttpContext.Session.GetString("UsuarioNombre") ?? "Usuario";
        }
    }
}
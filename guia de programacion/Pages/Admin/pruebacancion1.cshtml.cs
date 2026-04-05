using Microsoft.AspNetCore.Mvc.RazorPages;
using spotify.Data;
using spotify.Models;
using System.Collections.Generic;
using System.Linq;

namespace spotify.Pages.Admin
{
    public class Pruebacancion1Model : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Cancion> Canciones { get; set; }

        public Pruebacancion1Model(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Canciones = _context.Canciones.ToList();
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace spotify.Models
{
    public class Cancion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string RutaPortada { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public TimeSpan Duracion { get; set; }
        public bool EsFavorito { get; set; }
    }
}
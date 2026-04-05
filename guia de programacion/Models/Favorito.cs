using System.ComponentModel.DataAnnotations;

namespace spotify.Models
{
    public class Favorito
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CancionId { get; set; }
        public Cancion Cancion { get; set; } = null!;
    }
}
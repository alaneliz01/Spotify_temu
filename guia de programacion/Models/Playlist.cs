using System.ComponentModel.DataAnnotations;

namespace spotify.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public List<PlaylistCancion> PlaylistCanciones { get; set; } = new();
    }

    public class PlaylistCancion
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
        public int CancionId { get; set; }
        public Cancion Cancion { get; set; } = null!;
    }
}
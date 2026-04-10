using System.ComponentModel.DataAnnotations;

namespace spotify.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public bool EsAdmin { get; set; } = false;
        public bool EsPremium { get; set; } = false;
        public bool EsVerificado { get; set; } = false;

        public string Plan { get; set; } = "Gratis";

        // Nuevo campo para la foto de perfil
        public string FotoPerfil { get; set; } = string.Empty;
    }
}
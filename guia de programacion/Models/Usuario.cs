using System.ComponentModel.DataAnnotations;

namespace spotify.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public bool EsAdmin { get; set; } = false;
        public bool EsPremium { get; set; } = false;

        // Esta columna almacenará "Individual", "Familiar" o "Blasphemous"
        public string Plan { get; set; } = "Gratis";
    }
}
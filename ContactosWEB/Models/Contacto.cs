using System.ComponentModel.DataAnnotations;

namespace ContactosWEB.Models
{
    public class Contacto
    {
        public int ContactoId { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [StringLength(50, ErrorMessage = "No puede tener mas de {0} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Telefono { get; set; }

        [StringLength(40, ErrorMessage = "No puede tener mas de {0} caracteres")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }

        public TipoTelefono TipoTelefono { get; set; }

        public string Foto { get; set; }

    }
    public enum TipoTelefono
    {
        personal,
        comercial
    }
}

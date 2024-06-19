using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("Usuarios", Schema = "Seguridad")]
    public class UsuariosModel
    {
        [Key]
        public int idUsuario { get; set; }

        [Required]
        [StringLength(40)]
        public string codigoUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreUsuario { get; set; }

        [Required]
        [StringLength(80)]
        public string passwordUsuario { get; set; }

        [Required]
        public bool isActivo { get; set; }

        [Required]
        public DateTime ultimaConexion { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuarioRegistro { get; set; }
    }
}

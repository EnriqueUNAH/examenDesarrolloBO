using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("Agencias", Schema = "General")]
    public class AgenciasModel
    {
        [Key]
        public int idAgencia { get; set; }

        [Required]
        public int idCanalServicio { get; set; }

        [Required]
        [StringLength(6)]
        public string codigoAgencia { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreAgencia { get; set; }

        [Required]
        [StringLength(255)]
        public string direccionAgencia { get; set; }

        [Required]
        [StringLength(20)]
        public string telefonoAgencia { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("CanalServicio", Schema = "General")]
    public class CanalServicioModel
    {
        [Key]
        public int idCanalServicio { get; set; }

        [Required]
        [StringLength(5)]
        public string codigoCanalServicio { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreCanalServicio { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("MotivoTransaccion", Schema = "General")]
    public class MotivoTransaccionModel
    {
        [Key]
        public int idMotivoTransaccion { get; set; }

        [Required]
        public int idTipoTransaccion { get; set; }

        [Required]
        [StringLength(5)]
        public string codigoMotivoTransaccion { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreMotivoTransaccion { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

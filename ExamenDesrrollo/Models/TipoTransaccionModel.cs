using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("TipoTransaccion", Schema = "Parametros")]
    public class TipoTransaccionModel
    {
        [Key]
        public int idTipoTransaccion { get; set; }

        [Required]
        [StringLength(2)]
        public string codigoTipoMovimiento { get; set; }

        [Required]
        public int codigoTipoTransaccion { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreTipoTransaccion { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificacion { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

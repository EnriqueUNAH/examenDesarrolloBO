using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("TipoCliente", Schema = "General")]
    public class TipoClienteModel
    {
        [Key]
        public int idTipoCliente { get; set; }

        [Required]
        [StringLength(5)]
        public string codigoTipoCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreTipoCliente { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

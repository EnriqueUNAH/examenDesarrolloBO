using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    [Table("Clientes", Schema = "General")]
    public class ClientesModel
    {
        [Key]
        public int idCliente { get; set; }

        [Required]
        public int idTipoCliente { get; set; }

        [Required]
        [StringLength(20)]
        public string codigoCliente { get; set; }

        [Required]
        [StringLength(40)]
        public string numeroIdentidad { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreCliente { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public DateTime fechaModificado { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

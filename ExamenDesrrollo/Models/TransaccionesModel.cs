using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    public class TransaccionesModel
    {
        [Key]
        public int idTransaccion { get; set; }

        [Required]
        public int idTipoTransaccion { get; set; }

        [Required]
        public int idAgencia { get; set; }

        [Required]
        public int idCliente { get; set; }

        [Required]
        public DateTime fechaTransaccion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal montoTransaccion { get; set; }

        [Required]
        public int idMotivoTransaccion { get; set; }

        [Required]
        public int idUsuario { get; set; }
    }
}

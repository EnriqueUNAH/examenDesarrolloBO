namespace ExamenDesrrollo.Models
{
    public class TransaccionesReporteModel
    {
        public DateTime FechaTransaccion { get; set; }
        public string TipoCliente { get; set; }
        public string NumeroIdentidad { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoAgencia { get; set; }
        public string NombreAgencia { get; set; }
        public string CanalServicio { get; set; }
        public string CodigoMotivoTransaccion { get; set; }
        public string NombreMotivoTransaccion { get; set; }
        public decimal MontoTransaccion { get; set; }
    }
}

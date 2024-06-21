using System;
using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class TransaccionesReporteData
    {
        private readonly string conexion;

        public TransaccionesReporteData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<TransaccionesReporteModel>> ObtenerReporteTransacciones()
        {
            List<TransaccionesReporteModel> lista = new List<TransaccionesReporteModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTransaccionesReporte", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new TransaccionesReporteModel
                        {
                            FechaTransaccion = Convert.ToDateTime(reader["Fecha Transacción"]),
                            TipoCliente = reader["Tipo de Cliente"].ToString(),
                            NumeroIdentidad = reader["Identidad del Cliente"].ToString(),
                            NombreCliente = reader["Nombre del Cliente"].ToString(),
                            CodigoAgencia = reader["Codigo Agencia"].ToString(),
                            NombreAgencia = reader["Nombre Agencia"].ToString(),
                            CanalServicio = reader["Canal de Servicio"].ToString(),
                            CodigoMotivoTransaccion = reader["Codigo Motivo Transacción"].ToString(),
                            NombreMotivoTransaccion = reader["Nombre Motivo Transacción"].ToString(),
                            MontoTransaccion = Convert.ToDecimal(reader["Monto de Transacción"]),
                        });
                    }
                }
            }
            return lista;
        }
    }
}

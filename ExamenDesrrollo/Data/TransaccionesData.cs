using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class TransaccionesData
    {
        private readonly string conexion;

        public TransaccionesData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<TransaccionesModel>> Lista()
        {
            List<TransaccionesModel> lista = new List<TransaccionesModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTransacciones", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new TransaccionesModel
                        {
                            idTransaccion = Convert.ToInt32(reader["idTransaccion"]),
                            idMotivoTransaccion = Convert.ToInt32(reader["idMotivoTransaccion"]),
                            idAgencia = Convert.ToInt32(reader["idAgencia"]),
                            idCliente = Convert.ToInt32(reader["idCliente"]),
                            fechaTransaccion = Convert.ToDateTime(reader["fechaTransaccion"]),
                            montoTransaccion = Convert.ToDecimal(reader["montoTransaccion"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<TransaccionesModel> ObtenerPorId(int id)
        {
            TransaccionesModel transaccion = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTransaccionByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTransaccion", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        transaccion = new TransaccionesModel
                        {
                            idTransaccion = Convert.ToInt32(reader["idTransaccion"]),
                            idMotivoTransaccion = Convert.ToInt32(reader["idMotivoTransaccion"]),
                            idAgencia = Convert.ToInt32(reader["idAgencia"]),
                            idCliente = Convert.ToInt32(reader["idCliente"]),
                            fechaTransaccion = Convert.ToDateTime(reader["fechaTransaccion"]),
                            montoTransaccion = Convert.ToDecimal(reader["montoTransaccion"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        };
                    }
                }
            }
            return transaccion;
        }

        public async Task<bool> Crear(TransaccionesModel transaccion)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idMotivoTransaccion", transaccion.idMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@idAgencia", transaccion.idAgencia);
                    cmd.Parameters.AddWithValue("@idCliente", transaccion.idCliente);
                    cmd.Parameters.AddWithValue("@fechaTransaccion", transaccion.fechaTransaccion);
                    cmd.Parameters.AddWithValue("@montoTransaccion", transaccion.montoTransaccion);
                    cmd.Parameters.AddWithValue("@idUsuario", transaccion.idUsuario);

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Actualizar(TransaccionesModel transaccion)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTransaccion", transaccion.idTransaccion);
                    cmd.Parameters.AddWithValue("@idMotivoTransaccion", transaccion.idMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@idAgencia", transaccion.idAgencia);
                    cmd.Parameters.AddWithValue("@idCliente", transaccion.idCliente);
                    cmd.Parameters.AddWithValue("@fechaTransaccion", transaccion.fechaTransaccion);
                    cmd.Parameters.AddWithValue("@montoTransaccion", transaccion.montoTransaccion);
                    cmd.Parameters.AddWithValue("@idUsuario", transaccion.idUsuario);

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteTransaccion", con);
                    cmd.Parameters.AddWithValue("@idTransaccion", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

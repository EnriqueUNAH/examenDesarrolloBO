using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class MotivoTransaccionData
    {
        private readonly string conexion;

        public MotivoTransaccionData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<MotivoTransaccionModel>> Lista()
        {
            List<MotivoTransaccionModel> lista = new List<MotivoTransaccionModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetMotivosTransaccion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new MotivoTransaccionModel
                        {
                            idMotivoTransaccion = reader["idMotivoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idMotivoTransaccion"]) : 0,
                            idTipoTransaccion = reader["idTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idTipoTransaccion"]) : 0,
                            codigoMotivoTransaccion = reader["codigoMotivoTransaccion"] != DBNull.Value ? reader["codigoMotivoTransaccion"].ToString() : string.Empty,
                            nombreMotivoTransaccion = reader["nombreMotivoTransaccion"] != DBNull.Value ? reader["nombreMotivoTransaccion"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<MotivoTransaccionModel> ObtenerPorId(int id)
        {
            MotivoTransaccionModel motivo = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetMotivoTransaccionByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idMotivoTransaccion", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        motivo = new MotivoTransaccionModel
                        {
                            idMotivoTransaccion = reader["idMotivoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idMotivoTransaccion"]) : 0,
                            idTipoTransaccion = reader["idTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idTipoTransaccion"]) : 0,
                            codigoMotivoTransaccion = reader["codigoMotivoTransaccion"] != DBNull.Value ? reader["codigoMotivoTransaccion"].ToString() : string.Empty,
                            nombreMotivoTransaccion = reader["nombreMotivoTransaccion"] != DBNull.Value ? reader["nombreMotivoTransaccion"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        };
                    }
                }
            }
            return motivo;
        }

        public async Task<(bool, string)> Crear(MotivoTransaccionModel motivo)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateMotivoTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTipoTransaccion", motivo.idTipoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoMotivoTransaccion", motivo.codigoMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@nombreMotivoTransaccion", motivo.nombreMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", motivo.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", motivo.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", motivo.idUsuario);

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return (rowsAffected > 0, null);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000) // Este es el número de error para RAISERROR con severidad 16
                {
                    return (false, ex.Message);
                }
                return (false, "Error al crear el motivo de transacción.");
            }
        }

        public async Task<(bool, string)> Actualizar(MotivoTransaccionModel motivo)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateMotivoTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idMotivoTransaccion", motivo.idMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@idTipoTransaccion", motivo.idTipoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoMotivoTransaccion", motivo.codigoMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@nombreMotivoTransaccion", motivo.nombreMotivoTransaccion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", motivo.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", motivo.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", motivo.idUsuario);

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return (rowsAffected > 0, null);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000) // Este es el número de error para RAISERROR con severidad 16
                {
                    return (false, ex.Message);
                }
                return (false, "Error al actualizar el motivo de transacción.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteMotivoTransaccion", con);
                    cmd.Parameters.AddWithValue("@idMotivoTransaccion", id);
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

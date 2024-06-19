using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class TipoTransaccionData
    {
        private readonly string conexion;

        public TipoTransaccionData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<TipoTransaccionModel>> Lista()
        {
            List<TipoTransaccionModel> lista = new List<TipoTransaccionModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTiposTransaccion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new TipoTransaccionModel
                        {
                            idTipoTransaccion = reader["idTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idTipoTransaccion"]) : 0,
                            codigoTipoMovimiento = reader["codigoTipoMovimiento"] != DBNull.Value ? reader["codigoTipoMovimiento"].ToString() : string.Empty,
                            codigoTipoTransaccion = reader["codigoTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["codigoTipoTransaccion"]) : 0,
                            nombreTipoTransaccion = reader["nombreTipoTransaccion"] != DBNull.Value ? reader["nombreTipoTransaccion"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificacion = reader["fechaModificacion"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificacion"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<TipoTransaccionModel> ObtenerPorId(int id)
        {
            TipoTransaccionModel tipo = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTipoTransaccionByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTipoTransaccion", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        tipo = new TipoTransaccionModel
                        {
                            idTipoTransaccion = reader["idTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["idTipoTransaccion"]) : 0,
                            codigoTipoMovimiento = reader["codigoTipoMovimiento"] != DBNull.Value ? reader["codigoTipoMovimiento"].ToString() : string.Empty,
                            codigoTipoTransaccion = reader["codigoTipoTransaccion"] != DBNull.Value ? Convert.ToInt32(reader["codigoTipoTransaccion"]) : 0,
                            nombreTipoTransaccion = reader["nombreTipoTransaccion"] != DBNull.Value ? reader["nombreTipoTransaccion"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificacion = reader["fechaModificacion"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificacion"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        };
                    }
                }
            }
            return tipo;
        }

        public async Task<(bool, string)> Crear(TipoTransaccionModel tipo)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateTipoTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoTipoMovimiento", tipo.codigoTipoMovimiento);
                    cmd.Parameters.AddWithValue("@codigoTipoTransaccion", tipo.codigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@nombreTipoTransaccion", tipo.nombreTipoTransaccion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", tipo.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificacion", tipo.fechaModificacion);
                    cmd.Parameters.AddWithValue("@idUsuario", tipo.idUsuario);

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
                return (false, "Error al crear el tipo de transacción.");
            }
        }

        public async Task<(bool, string)> Actualizar(TipoTransaccionModel tipo)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateTipoTransaccion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTipoTransaccion", tipo.idTipoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoTipoMovimiento", tipo.codigoTipoMovimiento);
                    cmd.Parameters.AddWithValue("@codigoTipoTransaccion", tipo.codigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@nombreTipoTransaccion", tipo.nombreTipoTransaccion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", tipo.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificacion", tipo.fechaModificacion);
                    cmd.Parameters.AddWithValue("@idUsuario", tipo.idUsuario);

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
                return (false, "Error al actualizar el tipo de transacción.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteTipoTransaccion", con);
                    cmd.Parameters.AddWithValue("@idTipoTransaccion", id);
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

using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class TipoClienteData
    {
        private readonly string conexion;

        public TipoClienteData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<TipoClienteModel>> Lista()
        {
            List<TipoClienteModel> lista = new List<TipoClienteModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTiposCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new TipoClienteModel
                        {
                            idTipoCliente = reader["idTipoCliente"] != DBNull.Value ? Convert.ToInt32(reader["idTipoCliente"]) : 0,
                            codigoTipoCliente = reader["codigoTipoCliente"] != DBNull.Value ? reader["codigoTipoCliente"].ToString() : string.Empty,
                            nombreTipoCliente = reader["nombreTipoCliente"] != DBNull.Value ? reader["nombreTipoCliente"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<TipoClienteModel> ObtenerPorId(int id)
        {
            TipoClienteModel tipoCliente = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTipoClienteByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTipoCliente", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        tipoCliente = new TipoClienteModel
                        {
                            idTipoCliente = reader["idTipoCliente"] != DBNull.Value ? Convert.ToInt32(reader["idTipoCliente"]) : 0,
                            codigoTipoCliente = reader["codigoTipoCliente"] != DBNull.Value ? reader["codigoTipoCliente"].ToString() : string.Empty,
                            nombreTipoCliente = reader["nombreTipoCliente"] != DBNull.Value ? reader["nombreTipoCliente"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        };
                    }
                }
            }
            return tipoCliente;
        }

        public async Task<(bool, string)> Crear(TipoClienteModel tipoCliente)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateTipoCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoTipoCliente", tipoCliente.codigoTipoCliente);
                    cmd.Parameters.AddWithValue("@nombreTipoCliente", tipoCliente.nombreTipoCliente);
                    cmd.Parameters.AddWithValue("@fechaRegistro", tipoCliente.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", tipoCliente.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", tipoCliente.idUsuario);

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
                return (false, "Error al crear el tipo de cliente.");
            }
        }

        public async Task<(bool, string)> Actualizar(TipoClienteModel tipoCliente)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateTipoCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTipoCliente", tipoCliente.idTipoCliente);
                    cmd.Parameters.AddWithValue("@codigoTipoCliente", tipoCliente.codigoTipoCliente);
                    cmd.Parameters.AddWithValue("@nombreTipoCliente", tipoCliente.nombreTipoCliente);
                    cmd.Parameters.AddWithValue("@fechaRegistro", tipoCliente.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", tipoCliente.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", tipoCliente.idUsuario);

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
                return (false, "Error al actualizar el tipo de cliente.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteTipoCliente", con);
                    cmd.Parameters.AddWithValue("@idTipoCliente", id);
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

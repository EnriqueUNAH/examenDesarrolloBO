using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class AgenciasData
    {
        private readonly string conexion;

        public AgenciasData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<AgenciasModel>> Lista()
        {
            List<AgenciasModel> lista = new List<AgenciasModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetAgencias", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new AgenciasModel
                        {
                            idAgencia = Convert.ToInt32(reader["idAgencia"]),
                            idCanalServicio = Convert.ToInt32(reader["idCanalServicio"]),
                            codigoAgencia = reader["codigoAgencia"].ToString(),
                            nombreAgencia = reader["nombreAgencia"].ToString(),
                            direccionAgencia = reader["direccionAgencia"].ToString(),
                            telefonoAgencia = reader["telefonoAgencia"].ToString(),
                            fechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                            fechaModificado = Convert.ToDateTime(reader["fechaModificado"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<AgenciasModel> ObtenerPorId(int id)
        {
            AgenciasModel agencia = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetAgenciaByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAgencia", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        agencia = new AgenciasModel
                        {
                            idAgencia = Convert.ToInt32(reader["idAgencia"]),
                            idCanalServicio = Convert.ToInt32(reader["idCanalServicio"]),
                            codigoAgencia = reader["codigoAgencia"].ToString(),
                            nombreAgencia = reader["nombreAgencia"].ToString(),
                            direccionAgencia = reader["direccionAgencia"].ToString(),
                            telefonoAgencia = reader["telefonoAgencia"].ToString(),
                            fechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                            fechaModificado = Convert.ToDateTime(reader["fechaModificado"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        };
                    }
                }
            }
            return agencia;
        }

        public async Task<(bool, string)> Crear(AgenciasModel agencia)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateAgencia", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCanalServicio", agencia.idCanalServicio);
                    cmd.Parameters.AddWithValue("@codigoAgencia", agencia.codigoAgencia);
                    cmd.Parameters.AddWithValue("@nombreAgencia", agencia.nombreAgencia);
                    cmd.Parameters.AddWithValue("@direccionAgencia", agencia.direccionAgencia);
                    cmd.Parameters.AddWithValue("@telefonoAgencia", agencia.telefonoAgencia);
                    cmd.Parameters.AddWithValue("@fechaRegistro", agencia.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", agencia.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", agencia.idUsuario);

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
                return (false, "Error al crear la agencia.");
            }
        }

        public async Task<(bool, string)> Actualizar(AgenciasModel agencia)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateAgencia", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idAgencia", agencia.idAgencia);
                    cmd.Parameters.AddWithValue("@idCanalServicio", agencia.idCanalServicio);
                    cmd.Parameters.AddWithValue("@codigoAgencia", agencia.codigoAgencia);
                    cmd.Parameters.AddWithValue("@nombreAgencia", agencia.nombreAgencia);
                    cmd.Parameters.AddWithValue("@direccionAgencia", agencia.direccionAgencia);
                    cmd.Parameters.AddWithValue("@telefonoAgencia", agencia.telefonoAgencia);
                    cmd.Parameters.AddWithValue("@fechaRegistro", agencia.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", agencia.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", agencia.idUsuario);

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
                return (false, "Error al actualizar la agencia.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteAgencia", con);
                    cmd.Parameters.AddWithValue("@idAgencia", id);
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

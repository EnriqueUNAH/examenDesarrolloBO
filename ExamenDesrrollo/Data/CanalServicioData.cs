using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class CanalServicioData
    {
        private readonly string conexion;

        public CanalServicioData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<CanalServicioModel>> Lista()
        {
            List<CanalServicioModel> lista = new List<CanalServicioModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetCanalesServicio", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new CanalServicioModel
                        {
                            idCanalServicio = reader["idCanalServicio"] != DBNull.Value ? Convert.ToInt32(reader["idCanalServicio"]) : 0,
                            codigoCanalServicio = reader["codigoCanalServicio"] != DBNull.Value ? reader["codigoCanalServicio"].ToString() : string.Empty,
                            nombreCanalServicio = reader["nombreCanalServicio"] != DBNull.Value ? reader["nombreCanalServicio"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<CanalServicioModel> ObtenerPorId(int id)
        {
            CanalServicioModel canalServicio = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetCanalServicioByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCanalServicio", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        canalServicio = new CanalServicioModel
                        {
                            idCanalServicio = reader["idCanalServicio"] != DBNull.Value ? Convert.ToInt32(reader["idCanalServicio"]) : 0,
                            codigoCanalServicio = reader["codigoCanalServicio"] != DBNull.Value ? reader["codigoCanalServicio"].ToString() : string.Empty,
                            nombreCanalServicio = reader["nombreCanalServicio"] != DBNull.Value ? reader["nombreCanalServicio"].ToString() : string.Empty,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                        };
                    }
                }
            }
            return canalServicio;
        }

        public async Task<(bool, string)> Crear(CanalServicioModel canalServicio)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateCanalServicio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoCanalServicio", canalServicio.codigoCanalServicio);
                    cmd.Parameters.AddWithValue("@nombreCanalServicio", canalServicio.nombreCanalServicio);
                    cmd.Parameters.AddWithValue("@fechaRegistro", canalServicio.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", canalServicio.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", canalServicio.idUsuario);

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
                return (false, "Error al crear el canal de servicio.");
            }
        }

        public async Task<(bool, string)> Actualizar(CanalServicioModel canalServicio)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateCanalServicio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCanalServicio", canalServicio.idCanalServicio);
                    cmd.Parameters.AddWithValue("@codigoCanalServicio", canalServicio.codigoCanalServicio);
                    cmd.Parameters.AddWithValue("@nombreCanalServicio", canalServicio.nombreCanalServicio);
                    cmd.Parameters.AddWithValue("@fechaRegistro", canalServicio.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", canalServicio.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", canalServicio.idUsuario);

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
                return (false, "Error al actualizar el canal de servicio.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteCanalServicio", con);
                    cmd.Parameters.AddWithValue("@idCanalServicio", id);
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

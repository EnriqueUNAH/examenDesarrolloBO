using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class UsuariosData
    {
        private readonly string conexion;

        public UsuariosData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<UsuariosModel>> Lista()
        {
            List<UsuariosModel> lista = new List<UsuariosModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetUsuarios", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new UsuariosModel
                        {
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                            codigoUsuario = reader["codigoUsuario"] != DBNull.Value ? reader["codigoUsuario"].ToString() : string.Empty,
                            nombreUsuario = reader["nombreUsuario"] != DBNull.Value ? reader["nombreUsuario"].ToString() : string.Empty,
                            passwordUsuario = reader["passwordUsuario"] != DBNull.Value ? reader["passwordUsuario"].ToString() : string.Empty,
                            isActivo = reader["isActivo"] != DBNull.Value ? Convert.ToBoolean(reader["isActivo"]) : false,
                            ultimaConexion = reader["ultimaConexion"] != DBNull.Value ? Convert.ToDateTime(reader["ultimaConexion"]) : DateTime.MinValue,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuarioRegistro = reader["idUsuarioRegistro"] != DBNull.Value ? Convert.ToInt32(reader["idUsuarioRegistro"]) : 0,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<UsuariosModel> ObtenerPorId(int id)
        {
            UsuariosModel usuario = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetUsuarioByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new UsuariosModel
                        {
                            idUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : 0,
                            codigoUsuario = reader["codigoUsuario"] != DBNull.Value ? reader["codigoUsuario"].ToString() : string.Empty,
                            nombreUsuario = reader["nombreUsuario"] != DBNull.Value ? reader["nombreUsuario"].ToString() : string.Empty,
                            passwordUsuario = reader["passwordUsuario"] != DBNull.Value ? reader["passwordUsuario"].ToString() : string.Empty,
                            isActivo = reader["isActivo"] != DBNull.Value ? Convert.ToBoolean(reader["isActivo"]) : false,
                            ultimaConexion = reader["ultimaConexion"] != DBNull.Value ? Convert.ToDateTime(reader["ultimaConexion"]) : DateTime.MinValue,
                            fechaRegistro = reader["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["fechaRegistro"]) : DateTime.MinValue,
                            fechaModificado = reader["fechaModificado"] != DBNull.Value ? Convert.ToDateTime(reader["fechaModificado"]) : DateTime.MinValue,
                            idUsuarioRegistro = reader["idUsuarioRegistro"] != DBNull.Value ? Convert.ToInt32(reader["idUsuarioRegistro"]) : 0,
                        };
                    }
                }
            }
            return usuario;
        }

        public async Task<(bool, string)> Crear(UsuariosModel usuario)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoUsuario", usuario.codigoUsuario);
                    cmd.Parameters.AddWithValue("@nombreUsuario", usuario.nombreUsuario);
                    cmd.Parameters.AddWithValue("@passwordUsuario", usuario.passwordUsuario);
                    cmd.Parameters.AddWithValue("@isActivo", usuario.isActivo);
                    cmd.Parameters.AddWithValue("@ultimaConexion", usuario.ultimaConexion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", usuario.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", usuario.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuarioRegistro", usuario.idUsuarioRegistro);

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
                return (false, "Error al crear el usuario.");
            }
        }

        public async Task<(bool, string)> Actualizar(UsuariosModel usuario)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.idUsuario);
                    cmd.Parameters.AddWithValue("@codigoUsuario", usuario.codigoUsuario);
                    cmd.Parameters.AddWithValue("@nombreUsuario", usuario.nombreUsuario);
                    cmd.Parameters.AddWithValue("@passwordUsuario", usuario.passwordUsuario);
                    cmd.Parameters.AddWithValue("@isActivo", usuario.isActivo);
                    cmd.Parameters.AddWithValue("@ultimaConexion", usuario.ultimaConexion);
                    cmd.Parameters.AddWithValue("@fechaRegistro", usuario.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", usuario.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuarioRegistro", usuario.idUsuarioRegistro);

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
                return (false, "Error al actualizar el usuario.");
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteUsuario", con);
                    cmd.Parameters.AddWithValue("@idUsuario", id);
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

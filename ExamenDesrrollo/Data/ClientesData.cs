using System.Data;
using ExamenDesrrollo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Data
{
    public class ClientesData
    {
        private readonly string conexion;

        public ClientesData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<ClientesModel>> Lista()
        {
            List<ClientesModel> lista = new List<ClientesModel>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetClientes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new ClientesModel
                        {
                            idCliente = Convert.ToInt32(reader["idCliente"]),
                            idTipoCliente = Convert.ToInt32(reader["idTipoCliente"]),
                            codigoCliente = reader["codigoCliente"].ToString(),
                            numeroIdentidad = reader["numeroIdentidad"].ToString(),
                            nombreCliente = reader["nombreCliente"].ToString(),
                            fechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                            fechaModificado = Convert.ToDateTime(reader["fechaModificado"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<ClientesModel> ObtenerPorId(int id)
        {
            ClientesModel cliente = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetClienteByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cliente = new ClientesModel
                        {
                            idCliente = Convert.ToInt32(reader["idCliente"]),
                            idTipoCliente = Convert.ToInt32(reader["idTipoCliente"]),
                            codigoCliente = reader["codigoCliente"].ToString(),
                            numeroIdentidad = reader["numeroIdentidad"].ToString(),
                            nombreCliente = reader["nombreCliente"].ToString(),
                            fechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                            fechaModificado = Convert.ToDateTime(reader["fechaModificado"]),
                            idUsuario = Convert.ToInt32(reader["idUsuario"]),
                        };
                    }
                }
            }
            return cliente;
        }

        public async Task<(bool, string)> Crear(ClientesModel cliente)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_CreateCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTipoCliente", cliente.idTipoCliente);
                    cmd.Parameters.AddWithValue("@codigoCliente", cliente.codigoCliente);
                    cmd.Parameters.AddWithValue("@numeroIdentidad", cliente.numeroIdentidad);
                    cmd.Parameters.AddWithValue("@nombreCliente", cliente.nombreCliente);
                    cmd.Parameters.AddWithValue("@fechaRegistro", cliente.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", cliente.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", cliente.idUsuario);

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
                return (false, "Error al crear el cliente.");
            }
        }


        public async Task<bool> Actualizar(ClientesModel cliente)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_UpdateCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", cliente.idCliente);
                    cmd.Parameters.AddWithValue("@idTipoCliente", cliente.idTipoCliente);
                    cmd.Parameters.AddWithValue("@codigoCliente", cliente.codigoCliente);
                    cmd.Parameters.AddWithValue("@numeroIdentidad", cliente.numeroIdentidad);
                    cmd.Parameters.AddWithValue("@nombreCliente", cliente.nombreCliente);
                    cmd.Parameters.AddWithValue("@fechaRegistro", cliente.fechaRegistro);
                    cmd.Parameters.AddWithValue("@fechaModificado", cliente.fechaModificado);
                    cmd.Parameters.AddWithValue("@idUsuario", cliente.idUsuario);

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
                    SqlCommand cmd = new SqlCommand("sp_DeleteCliente", con);
                    cmd.Parameters.AddWithValue("@idCliente", id);
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnos.Data;
using ExamenDesrrollo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turnos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesData _ClientesData;

        public ClientesController(ClientesData ClientesData)
        {
            _ClientesData = ClientesData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<ClientesModel> lista = await _ClientesData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron clientes." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = await _ClientesData.ObtenerPorId(id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado." });
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ClientesModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _ClientesData.Crear(cliente);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.idCliente }, new { message = "Cliente creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el cliente." });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ClientesModel cliente)
        {
            if (id != cliente.idCliente || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _ClientesData.Actualizar(cliente);
            if (resultado)
            {
                return Ok(new { message = "Cliente actualizado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el cliente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _ClientesData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Cliente eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el cliente." });
        }
    }
}

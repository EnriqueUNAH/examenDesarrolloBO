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
    public class TipoClienteController : ControllerBase
    {
        private readonly TipoClienteData _TipoClienteData;

        public TipoClienteController(TipoClienteData TipoClienteData)
        {
            _TipoClienteData = TipoClienteData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<TipoClienteModel> lista = await _TipoClienteData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron tipos de cliente." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tipoCliente = await _TipoClienteData.ObtenerPorId(id);
            if (tipoCliente == null)
            {
                return NotFound(new { message = "Tipo de cliente no encontrado." });
            }
            return Ok(tipoCliente);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] TipoClienteModel tipoCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _TipoClienteData.Crear(tipoCliente);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = tipoCliente.idTipoCliente }, new { message = "Tipo de cliente creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el tipo de cliente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] TipoClienteModel tipoCliente)
        {
            if (id != tipoCliente.idTipoCliente || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _TipoClienteData.Actualizar(tipoCliente);
            if (resultado)
            {
                return Ok(new { message = "Tipo de cliente actualizado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el tipo de cliente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _TipoClienteData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Tipo de cliente eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el tipo de cliente." });
        }
    }
}

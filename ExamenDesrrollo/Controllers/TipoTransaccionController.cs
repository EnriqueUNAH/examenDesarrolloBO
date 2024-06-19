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
    public class TipoTransaccionController : ControllerBase
    {
        private readonly TipoTransaccionData _TipoTransaccionData;

        public TipoTransaccionController(TipoTransaccionData TipoTransaccionData)
        {
            _TipoTransaccionData = TipoTransaccionData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<TipoTransaccionModel> lista = await _TipoTransaccionData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron tipos de transacción." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tipo = await _TipoTransaccionData.ObtenerPorId(id);
            if (tipo == null)
            {
                return NotFound(new { message = "Tipo de transacción no encontrado." });
            }
            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] TipoTransaccionModel tipo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _TipoTransaccionData.Crear(tipo);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = tipo.idTipoTransaccion }, new { message = "Tipo de transacción creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el tipo de transacción." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] TipoTransaccionModel tipo)
        {
            if (id != tipo.idTipoTransaccion || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _TipoTransaccionData.Actualizar(tipo);
            if (resultado)
            {
                return Ok(new { message = "Tipo de transacción actualizado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el tipo de transacción." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _TipoTransaccionData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Tipo de transacción eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el tipo de transacción." });
        }
    }
}

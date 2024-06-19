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
    public class MotivoTransaccionController : ControllerBase
    {
        private readonly MotivoTransaccionData _MotivoTransaccionData;

        public MotivoTransaccionController(MotivoTransaccionData MotivoTransaccionData)
        {
            _MotivoTransaccionData = MotivoTransaccionData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<MotivoTransaccionModel> lista = await _MotivoTransaccionData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron motivos de transacción." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var motivo = await _MotivoTransaccionData.ObtenerPorId(id);
            if (motivo == null)
            {
                return NotFound(new { message = "Motivo de transacción no encontrado." });
            }
            return Ok(motivo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] MotivoTransaccionModel motivo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _MotivoTransaccionData.Crear(motivo);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = motivo.idMotivoTransaccion }, new { message = "Motivo de transacción creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el motivo de transacción." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] MotivoTransaccionModel motivo)
        {
            if (id != motivo.idMotivoTransaccion || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _MotivoTransaccionData.Actualizar(motivo);
            if (resultado)
            {
                return Ok(new { message = "Motivo de transacción actualizado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el motivo de transacción." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _MotivoTransaccionData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Motivo de transacción eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el motivo de transacción." });
        }
    }
}

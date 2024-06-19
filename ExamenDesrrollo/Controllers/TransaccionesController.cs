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
    public class TransaccionesController : ControllerBase
    {
        private readonly TransaccionesData _TransaccionesData;

        public TransaccionesController(TransaccionesData TransaccionesData)
        {
            _TransaccionesData = TransaccionesData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<TransaccionesModel> lista = await _TransaccionesData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron transacciones." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var transaccion = await _TransaccionesData.ObtenerPorId(id);
            if (transaccion == null)
            {
                return NotFound(new { message = "Transacción no encontrada." });
            }
            return Ok(transaccion);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] TransaccionesModel transaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _TransaccionesData.Crear(transaccion);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = transaccion.idTransaccion }, new { message = "Transacción creada exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear la transacción." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] TransaccionesModel transaccion)
        {
            if (id != transaccion.idTransaccion || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _TransaccionesData.Actualizar(transaccion);
            if (resultado)
            {
                return Ok(new { message = "Transacción actualizada exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar la transacción." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _TransaccionesData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Transacción eliminada exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar la transacción." });
        }
    }
}

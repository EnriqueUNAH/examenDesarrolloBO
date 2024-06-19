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
    public class AgenciasController : ControllerBase
    {
        private readonly AgenciasData _AgenciasData;

        public AgenciasController(AgenciasData AgenciasData)
        {
            _AgenciasData = AgenciasData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<AgenciasModel> lista = await _AgenciasData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron agencias." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var agencia = await _AgenciasData.ObtenerPorId(id);
            if (agencia == null)
            {
                return NotFound(new { message = "Agencia no encontrada." });
            }
            return Ok(agencia);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] AgenciasModel agencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _AgenciasData.Crear(agencia);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = agencia.idAgencia }, new { message = "Agencia creada exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear la agencia." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AgenciasModel agencia)
        {
            if (id != agencia.idAgencia || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _AgenciasData.Actualizar(agencia);
            if (resultado)
            {
                return Ok(new { message = "Agencia actualizada exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar la agencia." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _AgenciasData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Agencia eliminada exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar la agencia." });
        }
    }
}

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
    public class CanalServicioController : ControllerBase
    {
        private readonly CanalServicioData _CanalServicioData;

        public CanalServicioController(CanalServicioData CanalServicioData)
        {
            _CanalServicioData = CanalServicioData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<CanalServicioModel> lista = await _CanalServicioData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron canales de servicio." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var canalServicio = await _CanalServicioData.ObtenerPorId(id);
            if (canalServicio == null)
            {
                return NotFound(new { message = "Canal de servicio no encontrado." });
            }
            return Ok(canalServicio);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CanalServicioModel canalServicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _CanalServicioData.Crear(canalServicio);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = canalServicio.idCanalServicio }, new { message = "Canal de servicio creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el canal de servicio." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CanalServicioModel canalServicio)
        {
            if (id != canalServicio.idCanalServicio || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _CanalServicioData.Actualizar(canalServicio);
            if (resultado)
            {
                return Ok(new { message = "Canal de servicio actualizado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el canal de servicio." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _CanalServicioData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Canal de servicio eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el canal de servicio." });
        }
    }
}

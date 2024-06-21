using Microsoft.AspNetCore.Mvc;
using Turnos.Data;
using ExamenDesrrollo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Turnos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosData _UsuariosData;

        public UsuariosController(UsuariosData UsuariosData)
        {
            _UsuariosData = UsuariosData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var lista = await _UsuariosData.Lista();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron usuarios." });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _UsuariosData.ObtenerPorId(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(usuario);
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UsuariosModelS userCredentials)
        {
            var lista = await _UsuariosData.Lista();
            var usuario = lista.FirstOrDefault(u => u.nombreUsuario == userCredentials.nombreUsuario && u.passwordUsuario == userCredentials.passwordUsuario && u.isActivo);

            if (usuario != null)
            {
                return Ok(true);
            }
            return Unauthorized(new { message = "Usuario o contraseña incorrectos o cuenta inactiva." });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuariosModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _UsuariosData.Crear(usuario);
            if (resultado)
            {
                return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.idUsuario }, new { message = "Usuario creado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el usuario." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UsuariosModel usuario)
        {
            if (id != usuario.idUsuario || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (resultado, mensajeError) = await _UsuariosData.Actualizar(usuario);
            if (resultado)
            {
                return Ok(new { message = "Usuario actualizado exitosamente." });
            }
            if (!string.IsNullOrEmpty(mensajeError))
            {
                return Conflict(new { message = mensajeError });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el usuario." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _UsuariosData.Eliminar(id);
            if (resultado)
            {
                return Ok(new { message = "Usuario eliminado exitosamente." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al eliminar el usuario." });
        }
    }
}

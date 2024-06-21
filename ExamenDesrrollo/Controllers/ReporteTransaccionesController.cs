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
    public class ReporteTransaccionesController : ControllerBase
    {
        private readonly TransaccionesReporteData _TransaccionesReporteData;

        public ReporteTransaccionesController(TransaccionesReporteData TransaccionesReporteData)
        {
            _TransaccionesReporteData = TransaccionesReporteData;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte()
        {
            List<TransaccionesReporteModel> lista = await _TransaccionesReporteData.ObtenerReporteTransacciones();
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { message = "No se encontraron transacciones." });
            }
            return Ok(lista);
        }
    }
}

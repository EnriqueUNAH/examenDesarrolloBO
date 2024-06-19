using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class AgenciasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
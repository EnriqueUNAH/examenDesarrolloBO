using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

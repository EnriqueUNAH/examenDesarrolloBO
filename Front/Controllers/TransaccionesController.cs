using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class TransaccionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

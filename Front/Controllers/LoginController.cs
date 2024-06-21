using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Front.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string nombreUsuario, string passwordUsuario)
        {
            var model = new
            {
                nombreUsuario,
                passwordUsuario
            };

            var jsonRequest = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://localhost:7106/api/Usuarios/Authenticate", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var usuarioValido = JsonSerializer.Deserialize<bool>(jsonResponse);

                    if (usuarioValido)
                    {
                        HttpContext.Session.SetString("Usuario", nombreUsuario);
                        HttpContext.Session.SetString("Nombre", nombreUsuario);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, nombreUsuario)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JObject.Parse(jsonResponse);
                    ViewBag.ErrorMessage = errorResponse["message"].ToString();
                }
            }

            return View("Index");
        }
    }
}

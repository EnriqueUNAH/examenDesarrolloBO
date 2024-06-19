using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
                        // Aquí podrías obtener más detalles del usuario si es necesario
                        // var userResponse = await client.GetAsync($"https://localhost:7106/api/Usuarios/{nombreUsuario}");
                        // var userJsonResponse = await userResponse.Content.ReadAsStringAsync();
                        // var user = JObject.Parse(userJsonResponse);

                        HttpContext.Session.SetString("Usuario", nombreUsuario);
                        HttpContext.Session.SetString("Nombre", nombreUsuario); // Ajusta según los detalles del usuario

                        // Autenticar al usuario manualmente
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, nombreUsuario)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al verificar el usuario.";
                }
            }

            return View("Index");
        }
    }
}

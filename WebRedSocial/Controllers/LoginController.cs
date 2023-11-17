using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebRedSocial.Controllers
{
    public class LoginController : Controller
    {
        Sistema s = Sistema.GetInstancia;

        public IActionResult Index()
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null)
            {
                TempData["mensajeError"] = "Ya has iniciado sesión, no necesitas acceder a esta página.";
                return RedirectToAction("Mostrar", "Error");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario unUsuario)
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null)
            {
                TempData["mensajeError"] = "Ya has iniciado sesión, no necesitas acceder a esta página.";
                return RedirectToAction("Mostrar", "Error");
            }
            else
            {
                try
                {
                    Usuario usuarioLogueado = s.ObtenerUsuarioLogueado(unUsuario.Email, unUsuario.Password);
                    HttpContext.Session.SetString("UsuarioLogueado", usuarioLogueado.Email); //Guarda en la session que el usuario está logueado y verifica datos
                    HttpContext.Session.SetString("Rol", usuarioLogueado.Rol); //Guarda el rol

                    TempData["mensaje"] = false;

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["mensaje"] = true;
                    TempData["mensajeError"] = ex.Message;
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        public IActionResult Registro()
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol == null)
            {
                return View();
            }
            TempData["mensajeError"] = "Ya te hás registrado, no necesitas acceder a esta página.";
            return RedirectToAction("Mostrar", "Error");
        }

        [HttpPost]
        public IActionResult AltaMiembro(Miembro unMiembro)
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol == null)   
            {
                try
                {
                    s.AltaMiembro(unMiembro);
                    TempData["mensaje"] = false; //Porque redirecciona

                    return RedirectToAction("Registro");
                }
                catch (Exception ex)
                {
                    TempData["mensaje"] = true;
                    TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                    return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
                }
            }
            return RedirectToAction("Mostrar", "Error");
        }

        public IActionResult CerrarSesion()
        {
            //HttpContext.Session.Remove("UsuarioLogueado");
            TempData["MostrarAlerta"] = true;

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

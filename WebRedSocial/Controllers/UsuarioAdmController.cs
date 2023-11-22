using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebRedSocial.Controllers
{
    public class UsuarioAdmController : Controller
    {
        Sistema s = Sistema.GetInstancia;

        public IActionResult Index()
        {          
            return View();
        }

        public IActionResult ListarUsuarios()
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                List<Miembro> miembros = s.ListarMiembrosPorNombreApellidoAscendente();
                ViewData["ContadorMiembros"] = miembros.Count();

                return View(miembros);
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
        }

        [HttpPost]
        public IActionResult BanearPost(int Id)
        {
            Administrador administrador = s.ObtenerAdmPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                try
                {
                    Post post = s.ObtenerPostPorIntId(Id);

                    administrador.CensurarPost(post);

                    return RedirectToAction("ListarPosts", "UsuarioAdm");
                }
                catch (Exception ex)    
                {
                    TempData["mensajeError"] = ex.Message;
                    return View();
                }               
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }

        public IActionResult ListarPosts()
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                List<Post> posts = s.ListarPosts();
                ViewData["ContadorPosts"] = posts.Count();
                return View(posts);
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }

        public IActionResult BloquearMiembro(Miembro unEmail)
        {
            Administrador administrador = s.ObtenerAdmPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                try
                {
                    Miembro miembro = s.ObtenerMiembroPorEmail(unEmail);

                    administrador.BloquearMiembro(miembro);

                    return RedirectToAction("ListarUsuarios", "UsuarioAdm");
                }
                catch (Exception ex)
                {
                    TempData["mensajeError"] = ex.Message;
                    return View();
                }            
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }

        public IActionResult DesbloquearMiembro(Miembro unEmail)
        {
            Administrador administrador = s.ObtenerAdmPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                try
                {
                    Miembro miembro = s.ObtenerMiembroPorEmail(unEmail);
                    administrador.DesbloquearMiembro(miembro);

                    return RedirectToAction("ListarUsuarios", "UsuarioAdm");
                }
                catch (Exception ex)
                {
                    TempData["mensajeError"] = ex.Message;
                    return View();
                }
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }



    }
}

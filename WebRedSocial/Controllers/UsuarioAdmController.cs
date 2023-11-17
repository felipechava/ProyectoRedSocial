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
                return View(miembros);
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
        }

        [HttpPost]
        public IActionResult BanearPost(int Id)
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                Post post = s.ObtenerPostPorIntId(Id);

                if (post.esHabilitado == false)
                {
                    post.esHabilitado = true;
                    //TempData["mensajeBaneado"] = "Este post ya está banneado.";
                }
                else
                {
                    post.esHabilitado = false;
                    TempData["mensajeBaneado"] = "Post baneado exitosamente.";
                }

                return RedirectToAction("ListarPosts", "UsuarioAdm");
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
                return View(posts);
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }

        public IActionResult BloquearMiembro(Miembro unEmail) //El problema acá es que no se como hacer para identificar al adm para después hacer la lógica de bloquear desde el this.adm
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                Miembro miembro = s.ObtenerMiembroPorEmail(unEmail);

                if (miembro.esBloqueado == true)
                {
                    TempData["mensajeBloqueado"] = "Este usuario ya está bloqueado.";
                }
                else
                {
                    miembro.esBloqueado = true;
                    TempData["mensajeExito"] = "Usuario bloqueado exitosamente.";
                }
                return RedirectToAction("ListarUsuarios", "UsuarioAdm");
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }
        public IActionResult DesbloquearMiembro(Miembro unEmail) 
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Administrador.RolValor))
            {
                Miembro miembro = s.ObtenerMiembroPorEmail(unEmail);

                if (miembro.esBloqueado == true) //Si está bloqueado
                {
                    miembro.esBloqueado = false; //Lo desbloqueo
                    TempData["mensajeDeExito"] = "Usuario desbloqueado exitosamente.";
                }
                else
                {
                    TempData["message"] = "Este usuario ya está bloqueado.";
                }
                return RedirectToAction("ListarUsuarios", "UsuarioAdm");
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
        }



    }
}

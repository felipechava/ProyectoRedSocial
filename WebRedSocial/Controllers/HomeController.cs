using Dominio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebRedSocial.Models;

namespace WebRedSocial.Controllers
{
    public class HomeController : Controller
    {
        Sistema s = Sistema.GetInstancia;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                string? emailLogueado = HttpContext.Session.GetString("UsuarioLogueado");

                if (emailLogueado != null)
                {
                    Usuario usuario = s.ObtenerUsuarioPorEmail(emailLogueado);

                    if (usuario is Miembro)
                    {
                        Miembro miembro = (Miembro)usuario;
                        ViewBag.NombreUsuario = miembro.Nombre;

                    }
                    else if (usuario is Administrador)
                    {
                        Administrador administrador = (Administrador)usuario;
                        ViewBag.NombreUsuario = administrador.Email;
                    }
                    else if (usuario is Anonimo)
                    {
                        Anonimo anonimo = (Anonimo)usuario;
                        ViewBag.NombreUsuario = anonimo.Nombre;
                    }
                }
                return View();
            }
            catch (Exception ex)    
            {
                TempData["mensajeError"] = ex.Message;
                return RedirectToAction("Index");
            }         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
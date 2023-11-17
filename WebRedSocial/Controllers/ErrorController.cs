using Microsoft.AspNetCore.Mvc;

namespace WebRedSocial.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Mostrar()
        {
            return View();
        }
    }
}

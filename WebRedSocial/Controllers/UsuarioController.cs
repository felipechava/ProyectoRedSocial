using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebRedSocial.Controllers
{
    public class UsuarioController : Controller
    {

        Sistema s = Sistema.GetInstancia;

        public IActionResult VerPosts()
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                List<Publicacion> publicaciones = s.publicaciones;
                List<Post> posts = new List<Post>();
                List<Post> listaPostHabilitados = new List<Post>();

                int cantidadLikes = 0;
                int cantidadDislikes = 0;
                double VA = 0;

                foreach (Publicacion publicacion in publicaciones)
                {
                    if (publicacion is Post) //Si es post 
                    {
                        posts.Add((Post)publicacion); //Agrego a la lista de posts                    
                    }
                }

                foreach (Post post in posts)
                {
                    if (miembro.amigos.Contains(post.Autor) || post.Autor == miembro && post.esHabilitado != false) //Si es amigo o si es post propio
                    {
                        if (post.esPrivado == false || post.esPrivado == true && post.Autor == miembro || post.esPrivado == true && miembro.amigos.Contains(post.Autor) && post.esHabilitado != false) //Si no es privado
                        {
                            cantidadLikes = post.CalcularCantidadLikes();
                            cantidadDislikes = post.CalcularCantidadDislikes();
                            VA = post.CalcularVAPost();

                            listaPostHabilitados.Add(post); //Añado el post a habilitados
                        }
                    }
                }
                return View(listaPostHabilitados);
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }
        }

        public IActionResult ObtenerUsuarioPorEmail(Miembro unEmail)
        {
            try
            {
                Miembro miembro = s.ObtenerMiembroPorEmail(unEmail);

                return View(miembro);
            }
            catch (Exception ex)
            {
                TempData["mensajeError"] = ex.Message;
                return View();
            }           
        }

        public IActionResult VerComentariosPost(int Id)
        {
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                try
                {
                    // Obtener el post y sus comentarios
                    Post post = s.ObtenerPostPorIntId(Id);
                    TempData["lastIdPost"] = post.Id;

                    if (post == null)
                    {
                        // Manejar el caso en que el post no se encuentre
                        TempData["mensajeError"] = "El post no existe.";
                        return RedirectToAction("Mostrar", "Error");
                    }

                    List<Comentario> comentarios = s.ObtenerComentariosPorPost(post);

                    // Pasar el post y comentarios a la vista
                    ViewData["Post"] = post;
                    ViewData["PostId"] = post.Id;
                    ViewData["PostTexto"] = post.Texto;
                    ViewData["PostAutorNombre"] = post.Autor.Nombre;
                    ViewData["PostAutorApellido"] = post.Autor.Apellido;
                    ViewData["PostFecha"] = post.Fecha.ToShortDateString();

                    ViewData["Comentarios"] = comentarios;
                    ViewData["ContadorComentarios"] = comentarios.Count();

                    return View(post);
                }
                catch (Exception ex)
                {
                    TempData["mensajeError"] = ex.Message;
                    return RedirectToAction("Mostrar", "Error");
                }
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }
        }

        [HttpPost]
        public IActionResult AgregarComentario(int Id, string contenidoComentario)
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                if (miembro.esBloqueado == false) //Si no está bloqueado puede hacer todo
                {
                    try
                    {
                        // Obtener el post al que se agregará el comentario
                        Post post = s.ObtenerPostPorIntId(Id);

                        if (post == null)
                        {
                            // Manejar el caso en que el post no se encuentre
                            TempData["mensajeError"] = "El post no existe.";
                            return RedirectToAction("Mostrar", "Error");
                        }

                        // Crear un nuevo comentario              
                        Comentario comentario = new Comentario("Nuevo comentario", miembro, TipoPublicacion.COMENTARIO, $"Título Comentario", contenidoComentario, false);


                        s.AltaComentario(comentario, miembro, post);

                        // Redirigir de nuevo a la página de ver comentarios
                        return RedirectToAction("VerComentariosPost", new { Id = post.Id });
                    }
                    catch (Exception ex)
                    {
                        TempData["mensajeError"] = ex.Message;
                        return RedirectToAction("Mostrar", "Error");
                    }
                }
                else
                {
                    TempData["mensajeError"] = "Usted no puede realizar un comentario porque está bloqueado por un administrador.";
                    return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
                }
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }
        }


        public IActionResult RealizarPost()
        {
            string? miembroLogueado = HttpContext.Session.GetString("UsuarioLogueado");
            string? rol = HttpContext.Session.GetString("Rol");
            Miembro miembro = s.ObtenerMiembroPorEmailString(miembroLogueado);
            TempData["AutorNombre"] = miembro.Nombre;
            TempData["AutorApellido"] = miembro.Apellido;

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                if (miembro.esBloqueado == false)
                {
                    return View();
                }
                else
                {
                    TempData["mensajeError"] = "Usted no puede realizar un post porque está bloqueado por un administrador.";
                    return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
                }
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }

        }

        [HttpPost]
        public IActionResult AltaPost(Post unPost, bool esPrivado)
        {
            string? miembroLogueado = HttpContext.Session.GetString("UsuarioLogueado");
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                Miembro miembro = s.ObtenerMiembroPorEmailString(miembroLogueado);
                unPost.Autor = miembro;
                unPost.esPrivado = esPrivado;

                if (!miembro.esBloqueado)
                {
                    try
                    {
                        s.AltaPost(unPost, miembro);
                        TempData["mensaje"] = false; //Porque redirecciona

                        return RedirectToAction("RealizarPost");
                    }
                    catch (Exception ex)
                    {
                        TempData["mensaje"] = true;
                        TempData["mensajeError"] = ex.Message;

                        return RedirectToAction("RealizarPost");
                    }
                }
                else
                {
                    TempData["mensajeError"] = "Usted no puede realizar un post porque está bloqueado por un administrador.";
                    return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
                }
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }
        }

        [HttpPost]
        public IActionResult DarLikeDislike(int Id, string reaccion)
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                Post post = s.ObtenerPostPorIntId(Id);

                try
                {
                    TipoReaccion tipoReaccion = (reaccion == "like") ? TipoReaccion.LIKE : TipoReaccion.DISLIKE;
                    miembro.ReaccionarPublicacion(post, tipoReaccion);

                    return RedirectToAction("VerPosts");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("VerPosts");
                }
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }
        }

        [HttpPost]
        public IActionResult DarLikeDislikeComentario(int IdPost, int Id, string reaccion)
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))    
            {
                try
                {
                    Comentario comentario = s.ObtenerComentarioPorIntId(Id);
                    //Post post = s.ObtenerPostPorIntId(IdPost);

                    if (comentario == null)
                    {
                        TempData["Error"] = "No existe un comentario con ese ID.";
                        return RedirectToAction("VerComentariosPost", new { IdPost });
                    }

                    TipoReaccion tipoReaccion = (reaccion == "like") ? TipoReaccion.LIKE : TipoReaccion.DISLIKE;
                    miembro.ReaccionarPublicacion(comentario, tipoReaccion);

                    return RedirectToAction("VerComentariosPost", new { Id = IdPost});
                }               
                catch (Exception ex)
                {
                    TempData["mensajeError"] = ex.Message;
                    return RedirectToAction("Mostrar", "Error");
                }
            } else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
            }           
        }

        public IActionResult ListarOtrosMiembros()
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

    }
}

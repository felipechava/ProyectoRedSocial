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
                    if (miembro.amigos.Contains(post.Autor) || post.Autor == miembro && post.esHabilitado != false || post.esPrivado == false) //Si es post de un amigo o si es post propio o si es público
                    {
                        if (!post.esPrivado || post.esPrivado && post.Autor == miembro || post.esPrivado && miembro.amigos.Contains(post.Autor) && post.esHabilitado)
                        {
                            if (post.esHabilitado)  
                            {
                                cantidadLikes = post.CalcularCantidadLikes();
                                cantidadDislikes = post.CalcularCantidadDislikes();
                                VA = post.CalcularVAPost();

                                listaPostHabilitados.Add(post); //Añado el post a habilitados
                            }                         
                        }
                    }
                }
                ViewData["ContadorPostHabilitados"] = listaPostHabilitados.Count();
                return View(listaPostHabilitados);
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir a la página de no permiso
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

                    return RedirectToAction("VerComentariosPost", new { Id = IdPost });
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

        public IActionResult ListarOtrosMiembros()
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            TempData["miembroLogueadoEmail"] = miembro.Email;
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                List<Miembro> miembros = miembro.ListarMiembrosNoAmigos();
                ViewData["ContadorOtrosMiembros"] = miembros.Count();

                return View(miembros);
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
        }

        [HttpPost]
        public IActionResult EnviarInvitacion(string Email)
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            TempData["miembroLogueadoEmail"] = miembro.Email;
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                try
                {
                    //Busca al miembro que va a enviar la invitación
                    Miembro miembroDestinatario = s.BuscarMiembroConEmail(Email);

                    //Chequear si ya se envió una invitacion a ese usuario
                    miembro.ComprobarSiSeEnvioInvitacion(miembroDestinatario);

                    //Lógica de enviar invitación al apretar botón "Enviar Invitación"
                    Invitacion nuevaInvitacion = miembro.EnviarSolicitudAmistad(miembroDestinatario);

                    return RedirectToAction("ListarOtrosMiembros", "Usuario");
                }
                catch (Exception ex)
                {
                    TempData["mensajeError"] = ex.Message;
                    return RedirectToAction("Mostrar", "Error");
                }
            }
            TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
            return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
        }

        public IActionResult VerSolicitudesPendientes()
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                List<Invitacion> solicitudesPendientesDelMiembroLogueado = miembro.ListarSolicitudesPendientes();

                return View(solicitudesPendientesDelMiembroLogueado);
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
            }
        }

        [HttpPost]
        public IActionResult AceptarRechazarInvitacionPendiente(int InvitacionId, string Accion)
        {
            Miembro miembro = s.ObtenerMiembroPorEmailString(HttpContext.Session.GetString("UsuarioLogueado"));
            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals(Miembro.RolValor))
            {
                Invitacion? invitacion = miembro.ListarSolicitudesPendientes().FirstOrDefault(i => i.Id == InvitacionId);

                if (invitacion != null)
                {
                    try
                    {
                        if (Accion == "Aceptar")
                        {
                            miembro.AceptarSolicitudAmistad(invitacion);
                        }
                        else if (Accion == "Rechazar")
                        {
                            miembro.RechazarSolicitudDeAmistad(invitacion);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["mensajeError"] = ex.Message;
                        return RedirectToAction("Mostrar", "Error");
                    }
                }
                return RedirectToAction("VerSolicitudesPendientes", "Usuario");
            }
            else
            {
                TempData["mensajeError"] = "No está autorizado para acceder a esta página.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
            }
        }

        public IActionResult Filter(string text, int number)
        {
            if (text != null)   
            {
                List<Publicacion> publicaciones = s.publicaciones;

                var filterPost = publicaciones
                    .Where(p => p is Post)
                    .Cast<Post>()
                    .Where(post => post.Texto.ToLower().Contains(text.ToLower()) && post.CalcularVAPost() >= number).ToList();

                var filterComments = publicaciones
                    .Where(p => p is Comentario)
                    .Cast<Comentario>()
                    .Where(post => post.Texto.ToLower().Contains(text.ToLower()) && post.CalcularVAComentario() >= number).ToList();

                ViewBag.PostsFilter = filterPost;
                ViewBag.ComentariosFilter = filterComments;

                return View("FilterResults");
            } else
            {
                TempData["mensajeError"] = "El campo no puede estar vacío.";
                return RedirectToAction("Mostrar", "Error"); // Redirigir página de no permiso
            }
            
        }

        public IActionResult FilterResults()
        {
            return View();
        }

    }
}

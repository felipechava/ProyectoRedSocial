using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Dominio
{
    public class Sistema
    {
        //Patrón Singleton
        private static Sistema instancia;

        public static Sistema GetInstancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Sistema();
                }
                return instancia;
            }
        }

        //A T R I B U T O S
        public List<Miembro> miembros { get; set; }

        public List<Administrador> administradores { get; set; }

        public List<Publicacion> publicaciones { get; set; }

        public List<Usuario> usuarios { get ; set; }

        //Constructor
        private Sistema()
        { 
            this.miembros = new List<Miembro>(); //Inicializa las listas
            this.administradores = new List<Administrador>();
            this.publicaciones = new List<Publicacion>();
            this.usuarios = new List<Usuario>();
            PrecargaDatos(); //Se precargan los datos al iniciar el sistema
        }

        //M É T O D O S

        //Precarga de datos
        private void PrecargaDatos()
        {
            PrecargaUsuariosPublicacionesInvitacionesReacciones();
            AgregarMiembrosAListaDeAmigos();
        }           

        private void PrecargaUsuariosPublicacionesInvitacionesReacciones()
        {
            //Miembros
            Miembro miembro1 = new Miembro("miembro1@hotmail.com", "miembro1", "miembro1", "apellidoMiembro1", new DateTime(2000, 01, 01));
            Miembro miembro2 = new Miembro("miembro2@hotmail.com", "miembro2", "miembro2", "apellidoMiembro2", new DateTime(1986, 12, 22));
            Miembro miembro3 = new Miembro("miembro3@hotmail.com", "miembro3", "miembro3", "apellidoMiembro3", new DateTime(2008, 10, 20));
            Miembro miembro4 = new Miembro("miembro4@hotmail.com", "miembro4", "miembro4", "apellidoMiembro4", new DateTime(2005, 07, 17));
            Miembro miembro5 = new Miembro("miembro5@hotmail.com", "miembro5", "miembro5", "apellidoMiembro5", new DateTime(2001, 08, 09));
            Miembro miembro6 = new Miembro("miembro6@hotmail.com", "miembro6", "miembro6", "apellidoMiembro6", new DateTime(1995, 10, 03));
            Miembro miembro7 = new Miembro("miembro7@hotmail.com", "miembro7", "miembro7", "apellidoMiembro7", new DateTime(1993, 03, 02));
            Miembro miembro8 = new Miembro("miembro8@hotmail.com", "miembro8", "miembro8", "apellidoMiembro8", new DateTime(1990, 11, 08));
            Miembro miembro9 = new Miembro("miembro9@hotmail.com", "miembro9", "miembro9", "apellidoMiembro9", new DateTime(1992, 12, 04));
            Miembro miembro10 = new Miembro("miembro10@hotmail.com", "miembro10", "miembro10", "apellidoMiembro10", new DateTime(2000, 12, 20));

            miembros.Add(miembro1);
            miembros.Add(miembro2);
            miembros.Add(miembro3);
            miembros.Add(miembro4);
            miembros.Add(miembro5);
            miembros.Add(miembro6);
            miembros.Add(miembro7);
            miembros.Add(miembro8);
            miembros.Add(miembro9);
            miembros.Add(miembro10);
            usuarios.Add(miembro1);
            usuarios.Add(miembro2);
            usuarios.Add(miembro3);
            usuarios.Add(miembro4);
            usuarios.Add(miembro5);
            usuarios.Add(miembro6);
            usuarios.Add(miembro7);
            usuarios.Add(miembro8);
            usuarios.Add(miembro9);
            usuarios.Add(miembro10);

            //Administrador
            Administrador administrador1 = new Administrador("administrador1@hotmail.com", "administrador1");

            administradores.Add(administrador1);
            usuarios.Add(administrador1);

            //Publicaciones
            Post post1 = new Post("Texto 1", miembro1, TipoPublicacion.POST, "Título Post 1", "Este es el contenido del post 1 público", "imagen1.png", false, true);
            Post post2 = new Post("Texto 2", miembro1, TipoPublicacion.POST, "Título Post 2", "Este es el contenido del post 2 público", "imagen2.png", false, true);
            Post post3 = new Post("Texto 3", miembro3, TipoPublicacion.POST, "Título Post 3", "Este es el contenido del post 3 público", "imagen3.png", false, true);
            Post post4 = new Post("Texto 4", miembro4, TipoPublicacion.POST, "Título Post 4", "Este es el contenido del post 4 público", "imagen4.png", false, true);
            Post post5 = new Post("Texto 5", miembro5, TipoPublicacion.POST, "Título Post 5", "Este es el contenido del post 5 privado", "imagen5.png", true, false);

            publicaciones.Add(post1);
            publicaciones.Add(post2);
            publicaciones.Add(post3);
            publicaciones.Add(post4);
            publicaciones.Add(post5);
            //Post dentroo de la lista de -publicaciones del miembro
            miembro1.publicaciones.Add(post1);
            miembro1.publicaciones.Add(post2);
            miembro3.publicaciones.Add(post3);
            miembro4.publicaciones.Add(post4);
            miembro5.publicaciones.Add(post5);

            //Comentarios
            //Del post1
            Comentario comentario1 = new Comentario("Texto 6", miembro1, TipoPublicacion.COMENTARIO, "Título Comentario 1", "Este es el contenido del comentario 1 público", false);
            Comentario comentario2 = new Comentario("Texto 7", miembro1, TipoPublicacion.COMENTARIO, "Título Comentario 2", "Este es el contenido del comentario 2 público", false);
            Comentario comentario3 = new Comentario("Texto 8", miembro1, TipoPublicacion.COMENTARIO, "Título Comentario 3", "Este es el contenido del comentario 3 público", false);

            //Del post2
            Comentario comentario4 = new Comentario("Texto 9", miembro9, TipoPublicacion.COMENTARIO, "Título Comentario 4", "Este es el contenido del comentario 4 público", false);
            Comentario comentario5 = new Comentario("Texto 10", miembro10, TipoPublicacion.COMENTARIO, "Título Comentario 5", "Este es el contenido del comentario 5 público", false);
            Comentario comentario6 = new Comentario("Texto 11", miembro10, TipoPublicacion.COMENTARIO, "Título Comentario 6", "Este es el contenido del comentario 6 público", false);

            //Del post3
            Comentario comentario7 = new Comentario("Texto 12", miembro2, TipoPublicacion.COMENTARIO, "Título Comentario 7", "Este es el contenido del comentario 7 público", false);
            Comentario comentario8 = new Comentario("Texto 13", miembro2, TipoPublicacion.COMENTARIO, "Título Comentario 8", "Este es el contenido del comentario 8 público", false);
            Comentario comentario9 = new Comentario("Texto 14", miembro3, TipoPublicacion.COMENTARIO, "Título Comentario 9", "Este es el contenido del comentario 9 público", false);

            //Del post4
            Comentario comentario10 = new Comentario("Texto 15", miembro4, TipoPublicacion.COMENTARIO, "Título Comentario 10", "Este es el contenido del comentario 10 público", false);
            Comentario comentario11 = new Comentario("Texto 16", miembro4, TipoPublicacion.COMENTARIO, "Título Comentario 11", "Este es el contenido del comentario 11 público", false);
            Comentario comentario12 = new Comentario("Texto 17", miembro4, TipoPublicacion.COMENTARIO, "Título Comentario 12", "Este es el contenido del comentario 12 público", false);

            //Del post5
            Comentario comentario13 = new Comentario("Texto 18", miembro1, TipoPublicacion.COMENTARIO, "Título Comentario 13", "Este es el contenido del comentario 13 privado", true);
            Comentario comentario14 = new Comentario("Texto 19", miembro2, TipoPublicacion.COMENTARIO, "Título Comentario 14", "Este es el contenido del comentario 14 privado", true);
            Comentario comentario15 = new Comentario("Texto 20", miembro3, TipoPublicacion.COMENTARIO, "Título Comentario 15", "Este es el contenido del comentario 15 privado", true);

            //Comentario dentro lista -publicaciones
            publicaciones.Add(comentario1);
            publicaciones.Add(comentario2);
            publicaciones.Add(comentario3);
            publicaciones.Add(comentario4);
            publicaciones.Add(comentario5);
            publicaciones.Add(comentario6);
            publicaciones.Add(comentario7);
            publicaciones.Add(comentario8);
            publicaciones.Add(comentario9);
            publicaciones.Add(comentario10);
            publicaciones.Add(comentario11);
            publicaciones.Add(comentario12);
            publicaciones.Add(comentario13);
            publicaciones.Add(comentario14);
            publicaciones.Add(comentario15);
            //Comentarios dentro de lis lista -comentarios de los posts
            post1.comentarios.Add(comentario1);
            post1.comentarios.Add(comentario2);
            post1.comentarios.Add(comentario3);
            post2.comentarios.Add(comentario4);
            post2.comentarios.Add(comentario5);
            post2.comentarios.Add(comentario6);
            post3.comentarios.Add(comentario7);
            post3.comentarios.Add(comentario8);
            post3.comentarios.Add(comentario9);
            post4.comentarios.Add(comentario10);
            post4.comentarios.Add(comentario11);
            post4.comentarios.Add(comentario12);
            post5.comentarios.Add(comentario13);
            post5.comentarios.Add(comentario14);
            post5.comentarios.Add(comentario15);
            //Comentarios dentro de la lista -publicaciones de los miembros
            miembro1.publicaciones.Add(comentario1);
            miembro1.publicaciones.Add(comentario2);
            miembro1.publicaciones.Add(comentario3);
            miembro9.publicaciones.Add(comentario4);
            miembro10.publicaciones.Add(comentario5);
            miembro10.publicaciones.Add(comentario6);
            miembro2.publicaciones.Add(comentario7);
            miembro2.publicaciones.Add(comentario8);
            miembro3.publicaciones.Add(comentario9);
            miembro4.publicaciones.Add(comentario10);
            miembro4.publicaciones.Add(comentario11);
            miembro4.publicaciones.Add(comentario12);
            miembro1.publicaciones.Add(comentario13);
            miembro2.publicaciones.Add(comentario14);
            miembro3.publicaciones.Add(comentario15);

            //Invitaciones
            //Miembro1 amigo de casi todos
            Invitacion solicitudAceptada1 = new Invitacion(miembro2, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudPendiente2 = new Invitacion(miembro3, miembro1, EstadoInvitacion.PENDIENTE_APROBACION);
            Invitacion solicitudAceptada3 = new Invitacion(miembro4, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada4 = new Invitacion(miembro5, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada5 = new Invitacion(miembro6, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada6 = new Invitacion(miembro7, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada7 = new Invitacion(miembro8, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada8 = new Invitacion(miembro9, miembro1, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada9 = new Invitacion(miembro10, miembro1, EstadoInvitacion.APROBADA);

            miembro1.solicitudesAprobadas.Add(solicitudAceptada1);
            miembro1.solicitudesPendientes.Add(solicitudPendiente2);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada3);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada4);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada5);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada6);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada7);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada8);
            miembro1.solicitudesAprobadas.Add(solicitudAceptada9);

            //Miembro2 amigo de todos
            Invitacion solicitudAceptada01 = new Invitacion(miembro1, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada02 = new Invitacion(miembro3, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada03 = new Invitacion(miembro4, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada04 = new Invitacion(miembro5, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada05 = new Invitacion(miembro6, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada06 = new Invitacion(miembro7, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada07 = new Invitacion(miembro8, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada08 = new Invitacion(miembro9, miembro2, EstadoInvitacion.APROBADA);
            Invitacion solicitudAceptada09 = new Invitacion(miembro10, miembro2, EstadoInvitacion.APROBADA);

            miembro2.solicitudesAprobadas.Add(solicitudAceptada01);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada02);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada03);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada04);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada05);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada06);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada07);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada08);
            miembro2.solicitudesAprobadas.Add(solicitudAceptada09);

            //m3
            //miembro3.solicitudesAprobadas.Add(solicitudAceptada2);
            miembro3.solicitudesAprobadas.Add(solicitudAceptada02);

            Invitacion solicitudPendiente1 = new Invitacion(miembro3, miembro4, EstadoInvitacion.PENDIENTE_APROBACION);
            miembro3.solicitudesPendientes.Add(solicitudPendiente1); //Pendiente con el m4
            //m4
            miembro4.solicitudesAprobadas.Add(solicitudAceptada3);
            miembro4.solicitudesAprobadas.Add(solicitudAceptada03);
            miembro4.solicitudesPendientes.Add(solicitudPendiente1); //Pendiente con el m3
            //m5
            miembro5.solicitudesAprobadas.Add(solicitudAceptada4);
            miembro5.solicitudesAprobadas.Add(solicitudAceptada04);

            //Invitacion solicitudPendiente2 = new Invitacion(miembro5, miembro6, EstadoInvitacion.PENDIENTE_APROBACION);
            miembro5.solicitudesPendientes.Add(solicitudPendiente2); //Pendiente con el m6
            //m6
            miembro6.solicitudesAprobadas.Add(solicitudAceptada5);
            miembro6.solicitudesAprobadas.Add(solicitudAceptada05);
            miembro6.solicitudesPendientes.Add(solicitudPendiente2); //Pendiente con el m5
            //m7
            miembro7.solicitudesAprobadas.Add(solicitudAceptada6);
            miembro7.solicitudesAprobadas.Add(solicitudAceptada06);

            Invitacion solicitudRechazada1 = new Invitacion(miembro7, miembro8, EstadoInvitacion.RECHAZADA);
            miembro7.solicitudesRechazadas.Add(solicitudRechazada1); //Rechazada con el m8
            //m8
            miembro8.solicitudesAprobadas.Add(solicitudAceptada7);
            miembro8.solicitudesAprobadas.Add(solicitudAceptada07);
            miembro8.solicitudesRechazadas.Add(solicitudRechazada1); //Rechazada con el m7
            //m9
            miembro9.solicitudesAprobadas.Add(solicitudAceptada8);
            miembro9.solicitudesAprobadas.Add(solicitudAceptada08);

            Invitacion solicitudRechazada2 = new Invitacion(miembro9, miembro10, EstadoInvitacion.RECHAZADA);
            miembro9.solicitudesRechazadas.Add(solicitudRechazada2); //Rechazada con el m10
            //m10
            miembro10.solicitudesAprobadas.Add(solicitudAceptada9);
            miembro10.solicitudesAprobadas.Add(solicitudAceptada09);
            miembro10.solicitudesRechazadas.Add(solicitudRechazada2); //Rechazada con el m9

            //Reacciones
            Reaccion like = new Reaccion(TipoReaccion.LIKE, miembro1);
            Reaccion dislike = new Reaccion(TipoReaccion.DISLIKE, miembro1);
            Reaccion like2 = new Reaccion(TipoReaccion.LIKE, miembro6);
            Reaccion dislike2 = new Reaccion(TipoReaccion.LIKE, miembro7);
            Reaccion like4 = new Reaccion(TipoReaccion.DISLIKE, miembro5);
            Reaccion dislike4 = new Reaccion(TipoReaccion.DISLIKE, miembro4);

            //Reaccion dentro de posts
            post4.reacciones.Add(like); 
            post5.reacciones.Add(dislike);
            post1.reacciones.Add(like2);
            post1.reacciones.Add(dislike2);
            post1.reacciones.Add(like4);
            post1.reacciones.Add(dislike4);

            Reaccion like3 = new Reaccion(TipoReaccion.LIKE, miembro5);
            Reaccion dislike3 = new Reaccion(TipoReaccion.DISLIKE, miembro10);

            //Reaccion dentro de comentarios
            comentario1.reacciones.Add(like3);
            comentario3.reacciones.Add(dislike3);
        }

        private void AgregarMiembrosAListaDeAmigos()
        {
            foreach (Miembro miembro in miembros) //Agrega a amigos 
            {
                foreach (Invitacion invitacionAceptada in miembro.solicitudesAprobadas)  
                {
                    if (invitacionAceptada.miembroSolicitante == miembro) //Del m1 y m2 a los demás
                    {
                    miembro.amigos.Add(invitacionAceptada.miembroSolicitado);
                    }

                    if (invitacionAceptada.miembroSolicitado == miembro) //De los demás a m1 y m2
                    {
                        miembro.amigos.Add(invitacionAceptada.miembroSolicitante);
                    }
                }
            }         
        }

        public Post MostrarPost(Post unPost)
        {
            if (!unPost.esHabilitado)   
            {
                throw new Exception($"No se puede mostrar este post porque está deshabilitado por un administrador.");
            }
            else
            {
                return unPost;
            }
        }

        public List<Miembro> ListarMiembrosConMasPublicaciones()
        {
            List<Miembro> elMasPublicador = new List<Miembro>();
            int maxPublicaciones = 0;

            foreach (Miembro miembro in miembros)
            {
                int totalPublicaciones = miembro.publicaciones.Count; //Calcula la cantidad de publicaciones (Post + Comentarios)

                if (totalPublicaciones > maxPublicaciones) //Verifica si el miembro tiene más publicaciones que el máximo
                {
                    elMasPublicador.Clear(); //Limpia la lista
                    maxPublicaciones = totalPublicaciones;
                }

                if (totalPublicaciones == maxPublicaciones)
                {
                    elMasPublicador.Add(miembro);
                }
            }
            return elMasPublicador;
        }

        public bool BuscarMiembroConEmailBooleano(string unEmail)
        {
            foreach (Miembro miembro in miembros)
            {
                if (miembro.Email.Equals(unEmail))
                {
                    return true;
                }
            }
            return false;
        }

        public bool SeEncontroMiembroConPassword(string unPassword)
        {
            foreach (Miembro miembro in miembros)   
            {
                if (miembro.Password.Equals(unPassword))    
                {
                    return true;
                }
            }
            return false;
        }

        public Miembro BuscarMiembroConEmail(string unEmail)
        {
            Miembro miembroEncontrado = null;

            foreach (Miembro miembro in miembros)
            {
                if (miembro.Email.Equals(unEmail))
                {
                    miembroEncontrado = miembro;
                    break; //Porque ya lo encontró
                }
            }
            return miembroEncontrado;
        }

        public List<Publicacion> ListarPublicacionesConComentarios(Miembro unMiembro)
        {
            List<Publicacion> publicacionesConComentarios = new List<Publicacion>();

            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion is Post post)
                {
                    foreach (Comentario comentario in post.comentarios)
                    {
                        if (comentario.Autor == unMiembro)
                        {
                            publicacionesConComentarios.Add(post);
                            break; //Hizo un comentario en ese post, no es necesario volver a iterar.
                        }
                    }
                }
            }
            return publicacionesConComentarios;
        }

        public List<Publicacion> ListarPublicacionesConEmail(Miembro unMiembro)
        {
            List<Publicacion> listaPublicacionesFiltradas = new List<Publicacion>();

            foreach (Miembro miembro in miembros)
            {
                if (miembro.Email == unMiembro.Email)
                {
                    foreach (Publicacion publicacion in publicaciones)
                    {
                        if (publicacion.Autor == miembro)
                        {
                            listaPublicacionesFiltradas.Add(publicacion);
                        }
                    }
                }
            }

            if (listaPublicacionesFiltradas.Count == 0)
            {
                throw new Exception("El miembro ingresado no tiene publicaciones hechas.");
            }

            return listaPublicacionesFiltradas;
        }

        public void AltaPost(Post unPost, Miembro autorPost)
        {
            unPost.ValidacionesPublicacion();
            unPost.ValidacionesPost();
            autorPost.HacerPublicacion(unPost);
            publicaciones.Add(unPost);           
        }

        public void AltaPublicacion(Publicacion unaPublicacion)
        {
            unaPublicacion.ValidacionesPublicacion();
            publicaciones.Add(unaPublicacion);
        }

        public void AltaComentario(Comentario unComentario, Miembro autorComentario, Post unPost)
        {
            unComentario.ValidacionesPublicacion();
            unComentario.Validaciones();
            autorComentario.HacerComentario(unPost, unComentario);
            publicaciones.Add(unComentario);
        }

        public void AltaMiembro(Miembro unMiembro)
        {
            unMiembro.ValidacionesUsuario();
            unMiembro.Validaciones();
            usuarios.Add(unMiembro); //Se agrega a la lista 
            miembros.Add(unMiembro);
        }

        public bool TryParseFecha(string fechaString, out DateTime fecha) //Para parsear una fecha al formato deseado
        {
            return DateTime.TryParseExact(fechaString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha);
        }

        public bool EmailEsValido(string unEmail)
        {
            return unEmail.Trim().Length > 0 && unEmail.Contains("@") && !unEmail.StartsWith("@") && !unEmail.EndsWith("@");
        }

        public bool PasswordEsValido(string unPassword)
        {
            return unPassword != null && unPassword.Trim().Length > 0;
        }

        public int askInt(string unMessage) //Verifica que se introduzca un int
        {
            bool validSelection = false;
            int selected = -1;
            while (!validSelection)
            {
                try
                {
                    Console.WriteLine(unMessage);
                    selected = int.Parse(Console.ReadLine());
                    validSelection = true;
                }
                catch (Exception e)
                {
                    validSelection = false;
                    Console.WriteLine("Solo se aceptan números. Inténtalo de nuevo..."); //Única línea de consola en el dominio!
                }
            }
            return selected;
        }

        public List<Miembro> ListarMiembrosPorNombreApellidoAscendente()
        {
            List<Miembro> retorno = new List<Miembro>(miembros);

            retorno.Sort();
            return retorno;
        }

        public List<Miembro> ListarTodosLosMiembrosMenosA(Miembro yo)
        {
            List<Miembro> retorno = new List<Miembro>();

            foreach (Miembro miembro in miembros)   
            {
                if (!miembro.Equals(yo))    
                {
                    retorno.Add(miembro);
                }
            }
            retorno.Sort();
            return retorno;
        }

        public Usuario ObtenerUsuarioPorEmail(string unEmail)
        {
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Email == unEmail)
                {
                    return usuario;
                }
            }
            throw new Exception($"No existe un usuario con ese e-mail.");
        }

        public Miembro ObtenerMiembroPorEmail(Miembro unMiembro)
        {
            foreach (Miembro miembro in miembros)
            {
                if (miembro.Email == unMiembro.Email)
                {
                    return miembro;
                }
            }
            throw new Exception($"No existe un miembro con ese e-mail.");
        }

        public Miembro ObtenerMiembroPorEmailString(string unEmail)
        {
            foreach (Miembro miembro in miembros)
            {
                if (miembro.Email == unEmail)
                {
                    return miembro;
                }
            }
            throw new Exception($"No existe un miembro con ese e-mail.");
        }

        public Usuario ObtenerUsuarioLogueado(string unEmail, string unPassword)
        {
            foreach (Usuario usuario in usuarios)   
            {
                if (usuario.Email == unEmail && usuario.Password == unPassword) 
                {
                    return usuario;
                }              
            }
            throw new Exception("Email y contraseña incorrectos.");
        }

        public Post ObtenerPostPorId(Post unPost)
        {
            foreach (Publicacion post in publicaciones)    
            {
                if (post is Post)   
                {
                    if (post.Id.Equals(unPost.Id))   
                    {
                        return (Post)post;
                    }
                }
            }
            throw new Exception($"No existe un post con ese ID.");
        }

        public Post ObtenerPostPorIntId(int unId)
        {
            foreach (Publicacion post in publicaciones)
            {
                if (post is Post)
                {
                    if (post.Id.Equals(unId))
                    {
                        return (Post)post;
                    }
                }
            }
            throw new Exception($"No existe un post con ese ID.");
        }

        public Comentario ObtenerComentarioPorIntId(int unId)
        {
            foreach (Publicacion comentario in publicaciones)
            {
                if (comentario is Comentario)
                {
                    if (comentario.Id.Equals(unId))
                    {
                        return (Comentario)comentario;
                    }
                }
            }
            throw new Exception($"No existe un comentario con ese ID.");
        }

        public List<Comentario> ObtenerComentariosPorPost(Post unPost)
        {
            List<Post> posts = new List<Post>();
            List<Comentario> comentariosDelPost = new List<Comentario>();

            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion is Post)
                {
                    posts.Add((Post)publicacion);
                }
            }

            foreach (Post post in posts)
            {
                if (post.Id == unPost.Id)
                {
                    comentariosDelPost.AddRange(post.comentarios);
                }
            }
            return comentariosDelPost;
        }

        public List<Post> ListarPosts()
        {
            List<Post> posts = new List<Post>();

            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion is Post)
                {
                    posts.Add((Post)publicacion);
                }
            }
            return posts;
        }





    }
}

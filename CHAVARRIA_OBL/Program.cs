using Dominio;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace CHAVARRIA_OBL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Instanciar instancia
            Sistema s = Sistema.GetInstancia;

            //MENÚ
            bool exit = false;
            while (!exit)
            {
                MostrarMenu();
                string option = Console.ReadLine();

                switch (option)
                {
                    case "0":
                        Console.WriteLine($"Saliendo del programa...");
                        exit = true;
                        break;

                    case "1":
                        MenuAltaMiembro();
                        Despejar();
                        break;

                    case "2":
                        MenuListarPublicacionesConEmail();
                        Despejar();
                        break;

                    case "3":
                        MenuListarPostConComentariosConEmail();
                        Despejar();
                        break;

                    case "4":
                        MenuMostrarPublicacionesEntreFechas();
                        Despejar();
                        break;

                    case "5":
                        MenuObtenerMiembroConMasPublicaciones();
                        Despejar();
                        break;

                    default:
                        Console.WriteLine($"Ingrese una opción correcta.");
                        Console.WriteLine($"Pulse cualquier tecla para continuar...");
                        Despejar();
                        break;
                }
            }
        }

        //M É T O D O S

        private static void MostrarMenu()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"M   E   N   Ú - SOCIAL.NETWORK\n");
            Console.ResetColor();
            Console.WriteLine($"Seleccione la opción deseada:\n");
            Console.WriteLine($"0- Salir");
            Console.WriteLine($"1- Alta de un miembro");
            Console.WriteLine($"2- Dado un e-mail listar las publicaciones realizadas por un miembro.");
            Console.WriteLine($"3- Dado un e-mail listar las publicaciones que tengan comentarios.");
            Console.WriteLine($"4- Mostrar las publiciones realizadas coomprendidas entre dos fechas.");
            Console.WriteLine($"5- Obtener el/los miembros con mayor cantidad de publicaciones.");
        }

        private static void Despejar()
        {
            Console.ReadKey();
            Console.Clear();
        }

        public static DateTime PedirYDevolverFecha()
        {
            Sistema s = Sistema.GetInstancia;

            string fechaString;
            DateTime fechaMiembro;
            do
            {
                fechaString = Console.ReadLine();

                if (!s.TryParseFecha(fechaString, out fechaMiembro))
                {
                    Console.WriteLine("Fecha no válida. Intente nuevamente...\n");
                }

            } while (!s.TryParseFecha(fechaString, out fechaMiembro));
            return fechaMiembro;
        }

        public static void MenuMostrarPublicacionesEntreFechas()
        {
            Sistema s = Sistema.GetInstancia;
            Console.Clear();

            Console.WriteLine($"Ingrese la primer fecha (dd/mm/yyyy): ");
            DateTime fechaInicio = PedirYDevolverFecha();

            Console.WriteLine($"\nIngrese la segunda fecha (dd/mm/yyyy): ");
            DateTime fechaFinal = PedirYDevolverFecha();

            List<Publicacion> postsEnRango = new List<Publicacion>();

            foreach (Publicacion publicacion in s.publicaciones)    
            {
                if (publicacion is Post && publicacion.Fecha >= fechaInicio && publicacion.Fecha <= fechaFinal)  
                {
                    postsEnRango.Add(publicacion);
                }
            }

            postsEnRango.Sort((post1, post2) => post2.Fecha.CompareTo(post1.Fecha)); //Para ordenarlo de forma descendente
 
            Console.WriteLine($"\nPosts realizados entre las fechas ingresadas:\n");

            Console.WriteLine("ID\tFecha de publicación\tTítulo\t\tTexto\n"); //Encabezado de la tabla             
            foreach (Post post in postsEnRango)
            {
                string textoRecortado = post.Texto.Length > 50 ? post.Texto.Substring(0, 50) : post.Texto;

                Console.WriteLine($"{post.Id}\t{post.Fecha.ToShortDateString()}\t\t{post.Titulo}\t{textoRecortado}");
            }
        }

        private static void MenuObtenerMiembroConMasPublicaciones()
        {
            Sistema s = Sistema.GetInstancia;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"LISTAR MIEMBROS CON MAYOR CANTIDAD DE PUBLICACIÓNES\n");
            Console.ResetColor();

            List<Miembro> miembrosConMasPublicaciones = s.ListarMiembrosConMasPublicaciones();

            Console.WriteLine($"Lista de los miembros con más publicaciones: \n");

            Console.WriteLine("E-mail\t\t\t\tNombre\t\tApellido\t\tFecha de Nacimiento\n"); //Encabezado de la tabla
            foreach (Miembro miembro in miembrosConMasPublicaciones)
            {
                Console.WriteLine(miembro); //LLama al .ToString()
            }
        }

        private static void MenuListarPostConComentariosConEmail()
        {
            Sistema s = Sistema.GetInstancia;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"LISTAR PUBLICACIONES CON COMENTARIOS\n");
            Console.ResetColor();

            string emailIngresado = PedirYDevolverEmailValidado();
            Miembro miembroEncontrado = s.BuscarMiembroConEmail(emailIngresado);

            if (miembroEncontrado == null)
            {
                Console.WriteLine($"No existe un usuario con ese e-mail.");
                return;
            }

            try
            {
                List<Publicacion> listaRetorno = s.ListarPublicacionesConComentarios(miembroEncontrado);
                Console.Clear();

                    if (listaRetorno.Count == 0)
                {
                    Console.WriteLine($"El miembro no ha realizado ningún comentario.");
                    return;
                }

                Console.WriteLine($"Publicaciones donde '{miembroEncontrado.Email}' ha hecho comentarios:\n");
                Console.WriteLine("ID\tTipo\t\tTítulo\n"); //Encabezado de la tabla
                foreach (Post posts in listaRetorno)
                {
                    Console.WriteLine(posts); //LLama al .ToString()
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
            }
            Console.WriteLine($"\nPulse cualquier tecla para volver al menú...");
        }

        private static void MenuListarPublicacionesConEmail()
        {
            Sistema s = Sistema.GetInstancia;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"LISTAR PUBLICACIONES\n");
            Console.ResetColor();

            string emailIngresado = PedirYDevolverEmailValidado();
            Miembro miembroEncontrado = s.BuscarMiembroConEmail(emailIngresado);

            if (miembroEncontrado == null)  
            {
                Console.WriteLine($"No existe un usuario con ese e-mail.");
                return;
            }

            try
            {
                List<Publicacion> listaRetorno = s.ListarPublicacionesConEmail(miembroEncontrado);
                Console.Clear();

                Console.WriteLine($"Publicaciones realizadas por '{miembroEncontrado.Email}':\n");
                Console.WriteLine("ID\tTipo\t\tTítulo\n"); //Encabezado de la tabla
                foreach (Publicacion publicacion in listaRetorno)
                {
                    Console.WriteLine(publicacion); //LLama al .ToString()
                }

            }
            catch (Exception e) 
            {
                Console.WriteLine($"\n{e.Message}");
            }
            Console.WriteLine($"\nPulse cualquier tecla para volver al menú...");
        }

        public static Comentario CrearComentario()
        {
            //Pedir datos específicos de Comentario          
            Miembro autorComentario = InicioSesion(); //Para determinar el autor

            Console.WriteLine($"Ingrese el texto del comentario: ");
            string textoComentario = Console.ReadLine();

            Console.WriteLine($"Ingrese el título del comentario: ");
            string tituloComentario = Console.ReadLine();

            Console.WriteLine($"Ingrese el contenido del comentario: ");
            string contenidoComentario = Console.ReadLine();

            int seleccionPrivadoComentario;
            while (true)
            {
                Console.WriteLine($"¿Es un comentario privado?");
                Console.WriteLine($"1- Si, privado.");
                Console.WriteLine($"2- No, público.\n");
                string inputSeleccionPrivado = Console.ReadLine();

                if (int.TryParse(inputSeleccionPrivado, out seleccionPrivadoComentario) && (seleccionPrivadoComentario == 1 || seleccionPrivadoComentario == 2))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Opción no válida. Inténte nuevamente.\n");
                }
            }

            bool esPrivadoComentario = false;
            if (seleccionPrivadoComentario == 2)
            {
                esPrivadoComentario = true;
            }

            //Se crea y asigna atributos objeto Post
            Comentario nuevoComentario = new Comentario(textoComentario, autorComentario, TipoPublicacion.COMENTARIO, tituloComentario, contenidoComentario, esPrivadoComentario);         

            return nuevoComentario;
        }

        public static Post PedirDatosPost(Publicacion unaPublicacion)
        { 
            //Pedir datos específicos de Post
            Console.WriteLine($"Ingrese el título del post: ");
            string tituloPost = Console.ReadLine();

            Console.WriteLine($"Ingrese el contenido del post: ");
            string contenidoPost = Console.ReadLine();

            Console.WriteLine($"Ingrese la imagen del post: ");
            string imagenPost = Console.ReadLine();

            int seleccionPrivadoPost;
            while (true)
            {
                Console.WriteLine($"¿Es un post privado?");
                Console.WriteLine($"1- Si, privado.");
                Console.WriteLine($"2- No, público.\n");
                string inputSeleccionPrivado = Console.ReadLine();

                if (int.TryParse(inputSeleccionPrivado, out seleccionPrivadoPost) && (seleccionPrivadoPost == 1 || seleccionPrivadoPost == 2))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Opción no válida. Inténte nuevamente.\n");
                }
            }

            bool esPrivadoPost = false;
            if (seleccionPrivadoPost == 2)
            {
                esPrivadoPost = true;
            }

            bool esHabilitadoPost = true;

            //Se crea y asigna atributos objeto Post
            Post nuevoPost = new Post(unaPublicacion.Texto, unaPublicacion.Autor, TipoPublicacion.POST, tituloPost, contenidoPost, imagenPost, esPrivadoPost, esHabilitadoPost);
            
            return nuevoPost;
        }

        private static void AltaComentario(Post unPost)
        {
            Sistema s = Sistema.GetInstancia;

            Comentario nuevoComentario = CrearComentario();
            Miembro autorComentario = s.BuscarMiembroConEmail(nuevoComentario.Autor.Email);

            //Validar datos
            try
            {
                s.AltaComentario(nuevoComentario, autorComentario, unPost);
                Console.Clear();
                Console.WriteLine("Alta de comentario realizada con éxito! Presione cualquier tecla para volver al menú");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
                Console.WriteLine($"Intente nuevamente.\nVolviendo al menú...");
            }
            Console.ReadKey();
        }

        public static void AltaPost(Publicacion unaPublicacion)
        {
            Sistema s = Sistema.GetInstancia;

            Post nuevoPost = PedirDatosPost(unaPublicacion);
            Miembro autorPost = s.BuscarMiembroConEmail(nuevoPost.Autor.Email);

            //Validar datos
            try
            {
                s.AltaPost(nuevoPost, autorPost);
                Console.Clear();
                Console.WriteLine("Alta de post realizada con éxito! Presione cualquier tecla para volver al menú");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
                Console.WriteLine($"Intente nuevamente.\nVolviendo al menú...");
            }
            Console.ReadKey();
        }

        public static void PedirDatosYAltaPublicacion()
        {
            Sistema s = Sistema.GetInstancia;

            Miembro miembroAutorPublicacion = InicioSesion(); //Para asegurar que un miembro sea quien hace una publicación

            //Pedir datos de publicación
            Console.WriteLine($"Ingrese un texto: ");
            string textoPublicacion = Console.ReadLine();

            int opcionSeleccionadaTipoPublicacion = -1;

            while (opcionSeleccionadaTipoPublicacion < 0 || opcionSeleccionadaTipoPublicacion > 2)
            {
                Console.WriteLine($"Seleccione que tipo de publicación es:\n");
                Console.WriteLine($"1- Post");
                Console.WriteLine($"2- Comentario\n");

                opcionSeleccionadaTipoPublicacion = s.askInt("Seleccione la opción deseada:");

                if (opcionSeleccionadaTipoPublicacion < 0 || opcionSeleccionadaTipoPublicacion > 2)
                {
                    Console.WriteLine("Seleccion no válida. Presione cualquier tecla para volver a seleccionar");
                    Console.ReadKey();
                }
                else
                {
                    if (opcionSeleccionadaTipoPublicacion == 1)
                    {                      
                        //AltaPost(nuevaPublicacion);
                    }

                    if (opcionSeleccionadaTipoPublicacion == 2)
                    {
                        Comentario nuevoComentario = CrearComentario();
                        Miembro autorComentrio = nuevoComentario.Autor;
                        try
                        {
                            //s.AltaComentario(nuevoComentario, autorComentrio);
                            Console.WriteLine($"\nEl comentario se creó correctamente!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"\n{e.Message}");
                            Console.WriteLine($"Intente nuevamente.\nVolviendo al menú...");
                        }
                    }
                }
            }
        }

        private static Miembro InicioSesion()
        {
            Sistema s = Sistema.GetInstancia;

            string emailIngresado = PedirYDevolverEmailValidado();
            string passwordIngresado = PedirYDevolverPasswordValidado();

            //Buscar si hay un miembro con el email y contraseña ingresado
            Miembro miembroEncontrado = s.miembros.FirstOrDefault(miembro => miembro.Email == emailIngresado && miembro.Password == passwordIngresado);

            if (miembroEncontrado != null)
            {
                Console.WriteLine($"¡Bienvenido, {miembroEncontrado.Nombre}!");
                return miembroEncontrado; //Inicio de sesion exitoso
            }
            else
            {
                Console.WriteLine("Credenciales incorrectas. No se ha encontrado un miembro con estos datos.");
                return null; //El usuario no se ha autenticado.
            }
        }

        private static string PedirYDevolverPasswordValidado()
        {
            Sistema s = Sistema.GetInstancia;

            string password = "";

            while (!s.PasswordEsValido(password))
            {
                Console.WriteLine("Ingrese su contraseña:");
                password = Console.ReadLine();

                if (!s.PasswordEsValido(password))
                {
                    Console.WriteLine("\nContraseña no válida. Intente nuevamente...");
                }
            }
            return password;
        } 

        private static string PedirYDevolverEmailValidado()
        {
            Sistema s = Sistema.GetInstancia;

            string email = "";

            while (!s.EmailEsValido(email))
            {
                Console.WriteLine("Ingrese su email:");
                email = Console.ReadLine();

                if (!s.EmailEsValido(email))
                {
                    Console.WriteLine("\nEmail no válido. Intente nuevamente...");
                }
            }
            return email;
        }

        private static void MenuAltaMiembro()
        {
            Console.Clear();
            Sistema s = Sistema.GetInstancia;
            Console.BackgroundColor = ConsoleColor.DarkGray; 
            Console.WriteLine($"ALTA DE UN MIEMBRO\n");
            Console.ResetColor();

            //Pedir Datos
            string emailMiembro = PedirYDevolverEmailValidado();

            if (s.BuscarMiembroConEmailBooleano(emailMiembro))  
            {
                Console.WriteLine($"\nEl email ingresado ya está registrado.\nVolviendo al menú...");
                return;
            }

            Console.WriteLine($"\nIngrese su contraseña: ");
            string passwordMiembro = Console.ReadLine();

            Console.WriteLine($"\nIngrese el nombre: ");
            string nombreMiembro = Console.ReadLine();

            Console.WriteLine($"\nIngrese el apellido: ");
            string apellidoMiembro = Console.ReadLine();

            string fechaString;
            DateTime fechaMiembro;
            do
            {
                Console.WriteLine($"\nIngrese su fecha de nacimiento (dd/mm/yyyy):");
                fechaString = Console.ReadLine();

                if (!s.TryParseFecha(fechaString, out fechaMiembro))
                {
                    Console.WriteLine("Fecha no válida. Intente nuevamente...\n");
                }

            } while (!s.TryParseFecha(fechaString, out fechaMiembro));          

            //Crear el objeto
            Miembro nuevoMiembro = new Miembro(emailMiembro, passwordMiembro, nombreMiembro, apellidoMiembro, fechaMiembro);

            //Validar datos
            try
            {
                s.AltaMiembro(nuevoMiembro);                             
                Console.WriteLine($"\nEl miembro se dió de alta correctamente!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
                Console.WriteLine($"Intente nuevamente.\nVolviendo al menú...");
            }
        }   
    }
}
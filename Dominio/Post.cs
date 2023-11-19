using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Post : Publicacion
    {
        //A T R I B U T O S
        public string Imagen {  get; set; }

        public List<Comentario> comentarios { get; set; }

        public bool esHabilitado { get; set; } = true; //Comienzan todos habilitados

        public string Contenido { get; set; }

        public string Titulo { get; set; }

        public bool esPrivado { get; set; }

        private int lastId { get; set; }
      
        //Constructor
        public Post(string unTexto, Miembro unAutor, TipoPublicacion unTipo,string unTitulo, string unContenido, string unaImagen, bool esPrivado, bool esHabilitado) : base(unTexto, unAutor, unTipo)
        {
            this.Titulo = unTitulo;
            this.Contenido = unContenido;
            this.Imagen = unaImagen;
            this.esPrivado = esPrivado;
            this.comentarios = new List<Comentario>();
            this.esHabilitado = esHabilitado;
        }

        public Post()
        {         
            this.Tipo = TipoPublicacion.POST;
            //this.Autor = ObtenerAutor("hola");
            this.lastId = Id;
            this.esHabilitado = true;
            this.esPrivado = false;
            this.comentarios = new List<Comentario>();
            reacciones = new List<Reaccion>();
        }

        //M É T O D O S

        public void ValidacionesPost()
        {
            ValidarImagenNoVacio();
            //ValidarImagen();
            ValidarTituloNoVacio();
            ValidarLargoTitulo();
            ValidarContenidoNoVacio();
        }

        private void ValidarContenidoNoVacio()
        {
            if (this.Contenido == null || this.Contenido.Trim().Length == 0)
            {
                throw new Exception($"El campo del contenido no debe estar vacío.");
            }
        }

        private void ValidarLargoTitulo()
        {
            if (this.Titulo.Trim().Length <= 3)
            {
                throw new Exception($"El campo del título debe ser más largo.");
            }
        }

        private void ValidarTituloNoVacio()
        {
            if (this.Titulo == null || this.Titulo.Trim().Length == 0)
            {
                throw new Exception($"El campo del título no debe estar vacío.");
            }
        }

        private void ValidarImagenNoVacio()
        {
            if(this.Imagen == null || this.Imagen.Trim().Length == 0)
            {
                throw new Exception($"El campo de la imagen no debe estar vacío.");
            }
        }

        private void ValidarImagen()
        {
            if (!this.Imagen.EndsWith(".jpg") || !this.Imagen.EndsWith(".png"))   
            {
                throw new Exception($"La imagen debe tener terminación '.jpg' o '.png'.");
            }
        }

        public bool PuedeVerPost(Miembro unMiembro)
        {
            if (this.esPrivado)
            {
                return this.Autor.amigos.Contains(unMiembro); //Si el post es privado verifica que el miembro esté en la lista de amigos           
            }
            else if (!this.esHabilitado)
            {
                throw new Exception($"No puedes ver el post porque el mismo está censurado por un administrador.");
            }
            return true; //Si es público todos lo pueden ver
        }

        public bool PuedeComentarPost(Miembro unMiembro)
        {
            if (this.esPrivado) 
            {
                return this.Autor.amigos.Contains(unMiembro); //Si el post es privado verifica que el miembro esté en la lista de amigos
            }
            else if (!this.esHabilitado)
            {
                throw new Exception($"No puedes ver el post porque el mismo está censurado por un administrador.");
            }
            return true; //Si es público cualquiera lo puede comentar
        }

        public override string ToString()
        {
            return $"{this.Id}\t{this.Tipo}\t\t'{this.Titulo}'";
        }

        public double CalcularVAPost()
        {
            int cantidadLikesModificado = CalcularCantidadLikes() * 5;
            int cantidadDislikesModificado = CalcularCantidadDislikes() * (-2);
            double total = (cantidadLikesModificado) + (cantidadDislikesModificado);

            if (this.esPrivado == false) //Si es público   
            {
                total += 10;
                return total;
            }
            else //Si es privado
            {
                return total;
            }
        }

        
        
    }
}

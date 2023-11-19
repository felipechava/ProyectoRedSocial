using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Comentario : Publicacion, IValidaciones
    {
        //A T R I B U T O S
        public string Titulo { get; set; }

        public string Contenido { get; set; }

        public bool esPrivado { get; set; } = false;

        //Constructor
        public Comentario(string unTexto, Miembro unAutor, TipoPublicacion unTipo, string unTitulo, string unContenido, bool esPrivado) : base (unTexto, unAutor, unTipo)
        {
            this.Titulo = unTitulo;
            this.Contenido = unContenido;
            this.esPrivado = esPrivado;
            reacciones = new List<Reaccion>();
        }

        public Comentario()
        {
            this.Titulo = $"Título Comentario {this.Id}";
            this.Tipo = TipoPublicacion.COMENTARIO;
            this.esPrivado = false;
            reacciones = new List<Reaccion>();
        }

        //M É T O D O S

        public void Validaciones()
        {
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

        public bool PuedeComentarPost(Miembro unMiembro, Post unPost)
        {
            if (unPost.esPrivado)
            {
                if (this.Autor.amigos.Contains(unMiembro))  
                {
                    this.esPrivado = true;
                }
                return this.Autor.amigos.Contains(unMiembro); //Si el post es privado verifica que el miembro esté en la lista de amigos
            }
            return true; //Si es público cualquiera lo puede comentar
        }

        public bool PuedeVerComentario(Miembro unMiembro)
        {
            if (this.esPrivado)
            {
                return this.Autor.amigos.Contains(unMiembro); //Si el comentario es privado verifica que el miembro esté en la lista de amigos           
            }
            return true; //Si es público todos lo pueden ver
        }

        public override string ToString()
        {
            return $"{this.Id}\t{this.Tipo}\t'{this.Titulo}'";
        }

        public double CalcularVAComentario()
        {
            int cantidadLikes = CalcularCantidadLikes();
            int cantidadDislikes = CalcularCantidadDislikes();
            double total = (cantidadLikes * 5) + (cantidadDislikes * -2);

            return total;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Publicacion
    {
        //A T R I B U T O S
        public int Id { get; set; }

        private static int lastId {  get; set; } = 1;

        public string Texto { get; set; }

        public  DateTime Fecha { get; set; }

        public Miembro Autor { get; set; }

        public TipoPublicacion Tipo { get; set; }

        public List<Reaccion> reacciones { get; set; }

        //Constructor
        public Publicacion(string unTexto, Miembro unAutor, TipoPublicacion unTipo) 
        {
            this.Id = lastId++;
            this.Texto = unTexto;
            Fecha = DateTime.Now;
            this.Autor = unAutor;
            this.Tipo = unTipo;
            this.reacciones = new List<Reaccion>();
        }

        public Publicacion()
        {
            this.Id = lastId++;
            Fecha = DateTime.Now;
            this.reacciones = new List<Reaccion>();
        }

        //M É T O D O S

        public void ValidacionesPublicacion()
        {
            ValidarTexto();
        }

        private void ValidarTexto()
        {
            if (this.Texto == null || this.Texto.Trim().Length == 0)   
            {
                throw new Exception("El campo del texto no debe estar vacío.");
            }
        }

        public int CompareTo(object? obj) //this primero ascendente, this despues descendente
        {
            Publicacion comparar = (Post)obj;
            return comparar.Fecha.CompareTo(this.Fecha);
        }

        public override string ToString()
        {
            return $"{this.Id}\t{this.Tipo}";
        }

        public int CalcularCantidadLikes()
        {
            List<Reaccion> likes = new List<Reaccion>();

            foreach (Reaccion reaccion in this.reacciones)
            {
                if (reaccion.TipoReaccion == TipoReaccion.LIKE)
                {
                    likes.Add(reaccion);
                }
            }
            return likes.Count;
        }

        public int CalcularCantidadDislikes()
        {
            List<Reaccion> dislikes = new List<Reaccion>();

            foreach (Reaccion reaccion in this.reacciones)
            {
                if (reaccion.TipoReaccion == TipoReaccion.DISLIKE)
                {
                    dislikes.Add(reaccion);
                }
            }
            return dislikes.Count;
        }
    }
}

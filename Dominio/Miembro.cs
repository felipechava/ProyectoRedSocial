using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Miembro : Usuario, IValidaciones, IComparable<Miembro>
    {
        //A T R I B U T O S
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public bool esBloqueado = false;

        public List<Miembro> amigos { get; set; }

        public List<Publicacion> publicaciones { get; set; }

        public List<Invitacion> solicitudesPendientes { get; set; }

        public List<Invitacion> solicitudesAprobadas { get; set; }

        public List<Invitacion> solicitudesRechazadas { get; set; }

        public static string RolValor = "MIE";

        //Constructor
        public Miembro(string unEmail, string unPassword, string suNombre, string suApellido, DateTime suFechaNacimiento) : base(unEmail, unPassword)
        {
            this.Nombre = suNombre;
            this.Apellido = suApellido;
            this.fechaNacimiento = suFechaNacimiento;
            this.esBloqueado = false;
            this.amigos = new List<Miembro>(); //Inicializa las listas
            this.publicaciones = new List<Publicacion>();
            this.solicitudesPendientes = new List<Invitacion>();
            this.solicitudesAprobadas = new List<Invitacion>();
            this.solicitudesRechazadas = new List<Invitacion>();
            this.Rol = Miembro.RolValor;
        }  
        
        public Miembro(string unEmail, string unPassword) : base(unEmail, unPassword)
        {
            this.Rol = Miembro.RolValor;
        }

        public Miembro()
        {
            this.Rol = Miembro.RolValor;
            this.esBloqueado = false;
            this.amigos = new List<Miembro>(); //Inicializa las listas
            this.publicaciones = new List<Publicacion>();
            this.solicitudesPendientes = new List<Invitacion>();
            this.solicitudesAprobadas = new List<Invitacion>();
            this.solicitudesRechazadas = new List<Invitacion>();
        } 

        //M É T O D O S

        public void Validaciones()
        {
            ValidarNombre();
            ValidarApellido();
        }

        private void ValidarNombre()
        {
            if (this.Nombre == null || this.Nombre.Trim().Length == 0)
            {
                throw new Exception("El campo del nombre no debe estar vacío.");
            }
        }

        private void ValidarApellido()
        {
            if (this.Apellido == null || this.Apellido.Trim().Length == 0)
            {   
                throw new Exception($"El campo del apellido no debe estar vacío.");
            }
        }
       
        public Invitacion EnviarSolicitudAmistad(Miembro miembroDestinatario)
        {

            if (this.esBloqueado) 
            {
                throw new Exception($"No puedes enviar la solicitud porque estás bloqueado por un administrador.");
            }
            else if (miembroDestinatario.esBloqueado)
            {
                throw new Exception($"No puedes enviar la solicitud porque el destinatario está bloqueado por un administrador.");
            }
            else
            {
                Invitacion solicitud = new Invitacion(this, miembroDestinatario, EstadoInvitacion.PENDIENTE_APROBACION);

                miembroDestinatario.solicitudesPendientes.Add(solicitud);

                return solicitud;
            }
        }

        public void AceptarSolicitudAmistad(Invitacion unaSolicitudDeAmistad)
        {
            if (this.esBloqueado)
            {
                throw new Exception($"No puedes aceptar la solicitud porque estás bloqueado por un administrador.");
            }
            else
            {
                if (unaSolicitudDeAmistad.Estado == EstadoInvitacion.PENDIENTE_APROBACION)   
                {
                    unaSolicitudDeAmistad.Estado = EstadoInvitacion.APROBADA;
                    solicitudesAprobadas.Add(unaSolicitudDeAmistad);
                    unaSolicitudDeAmistad.miembroSolicitante.solicitudesAprobadas.Add(unaSolicitudDeAmistad); //Agrega la solicitud a la lista de aprobadas del miembro que envió la solicitud
                }
                else
                {
                    throw new Exception($"No puedes aceptar la solicitud porque no está en estado pendiente.");
                }
            }
        }

        public void RechazarSolicitudDeAmistad(Invitacion unaSolicitudDeAmistad)
        {
            if (this.esBloqueado)
            {
                throw new Exception($"No puedes aceptar la solicitud porque estás bloqueado por un administrador.");
            }
            else
            {
                if (unaSolicitudDeAmistad.Estado == EstadoInvitacion.PENDIENTE_APROBACION)
                {
                    unaSolicitudDeAmistad.Estado = EstadoInvitacion.RECHAZADA;
                    solicitudesRechazadas.Add(unaSolicitudDeAmistad);
                    unaSolicitudDeAmistad.miembroSolicitante.solicitudesRechazadas.Add(unaSolicitudDeAmistad); //Agrega la solicitud a la lista de rechazadas del miembro que envió la solicitud
                }
                else
                {
                    throw new Exception($"No puedes rechazar la solicitud porque no está en estado pendiente.");
                }
            }
        }

        public void HacerPublicacion(Post unPost)
        {
            if (this.esBloqueado)
            {
                throw new Exception($"No puedes hacer una publicación porque estas bloqueado por un administrador.");
            }
            else
            {
                this.publicaciones.Add(unPost);
            }
        }

        public void HacerComentario(Post unPost, Comentario unComentario)
        {
            if (!unPost.esHabilitado)
            {
                throw new Exception($"No se puede comentar este post porque está deshabilitado por un administrador.");
            }
            else
            {
                this.publicaciones.Add(unComentario);
                unPost.comentarios.Add(unComentario);
            }
        }

        public void ReaccionarPublicacion(Publicacion unaPublicacion, TipoReaccion unTipoDeReaccion)
        {
            // Verificar si el miembro ya dio una reacción a esta publicación o comentario.
            if (unaPublicacion.reacciones.Any(reaccion => reaccion.miembroQueReacciona == this))
            {
                throw new Exception($"Ya has reaccionado a esta publicación!");
            }
            else
            {
                // Agregar la reacción a la lista de reacciones de la publicación o comentario.
                Reaccion nuevaReaccion = new Reaccion(unTipoDeReaccion, this);
                unaPublicacion.reacciones.Add(nuevaReaccion);
            }
        }

        public override string ToString()
        {
            return $"{this.Email}\t\t{this.Nombre}\t{this.Apellido}\t{this.fechaNacimiento.ToShortDateString()}";
        }

        public int CompareTo(Miembro otroMiembro) //Para ordenar por apellido y luego por nombre ascendentemente
        {
            int resultadoApellido = Apellido.CompareTo(otroMiembro.Apellido);
            if (resultadoApellido == 0)
            {
                return Nombre.CompareTo(otroMiembro.Nombre);
            }
            return resultadoApellido;
        }

    }
}

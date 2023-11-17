using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Invitacion
    {
        //A T R I B U T O S
        public int Id {  get; set; }

        private static int lastId { get; set; } = 0;

        public Miembro miembroSolicitante { get; set; }

        public Miembro miembroSolicitado { get; set; }

        public EstadoInvitacion Estado { get; set; }

        public DateTime fechaSolicitud { get; set; }

        //Constructor
        public Invitacion(Miembro elQueSolicita, Miembro elQueRecibe, EstadoInvitacion unEstado)
        {
            this.Id = lastId++;
            this.miembroSolicitante = elQueSolicita;
            this.miembroSolicitado = elQueRecibe;
            this.Estado = unEstado;
            fechaSolicitud = DateTime.Now;
        }
    }
}

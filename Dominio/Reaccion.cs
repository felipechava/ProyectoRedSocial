using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Reaccion
    {
        public TipoReaccion TipoReaccion { get; set; }

        public Miembro miembroQueReacciona { get; set; }

        //Contructor
        public Reaccion(TipoReaccion unTipoReaccion, Miembro unMiembroQueReacciona)
        {
            this.TipoReaccion = unTipoReaccion;
            this.miembroQueReacciona = unMiembroQueReacciona;
        }

        //M É T O D O S


    }
}

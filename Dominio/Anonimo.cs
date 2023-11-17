using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Anonimo : Miembro
    {
        public Anonimo(string unEmail, string unPassword, string suNombre, string suApellido, DateTime suFechaNacimiento) : base(unEmail, unPassword, suNombre, suApellido, suFechaNacimiento)
        {
            this.Rol = RolValor;
        }

        public Anonimo() 
        {
            this.Rol = Miembro.RolValor;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Administrador : Usuario
    {
        public static string RolValor = "ADM";
        
        //Constructor
        public Administrador(string unEmail, string unPassword) : base(unEmail, unPassword) 
        {
            this.Rol = Administrador.RolValor;
        }

        public Administrador() 
        {
            this.Rol = Administrador.RolValor;
        }

        //M É T O D O S

        public void BloquearMiembro(Miembro unMiembro)
        {
            if (unMiembro.esBloqueado == true)
            {
                throw new Exception($"El miembro ya está bloqueado.");
            }
            else
            {
                unMiembro.esBloqueado = true;
            }
        }

        public void DesbloquearMiembro(Miembro unMiembro)
        {
            if (unMiembro.esBloqueado == true) //Si está bloqueado
            {
                unMiembro.esBloqueado = false; //Lo desbloqueo
            }
            else
            {
                throw new Exception($"Este usuario ya está bloqueado.");
            }
        }

        public void CensurarPost(Post unPost)
        {
            if (!unPost.esHabilitado)
            {
                throw new Exception($"La publicación ya está baneada.");
            }
            else
            {
                unPost.esHabilitado = false;
            }
        }


    }
}

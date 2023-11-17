using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dominio
{
    public class Usuario
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Rol {  get; set; }

        public Usuario(string unEmail, string unPassword)
        {
            this.Email = unEmail;
            this.Password = unPassword;
        }

        public Usuario()
        {

        }

        public void ValidacionesUsuario()
        {
            ValidarEmailVacío();
            ValidarEmailArroba();
            ValidarPassword();
        }

        private void ValidarPassword()
        {
            if (this.Password.Trim() == null || this.Password.Trim().Length == 0)
            {
                throw new Exception($"El campo de la contraseña no debe estar vacío.");
            }
        }

        private void ValidarEmailVacío()
        {
            if (this.Email.Trim() == null || this.Email.Trim().Length == 0)
            {
                throw new Exception($"El campo del email no debe estar vacío.");
            }
        }

        private void ValidarEmailArroba()
        {
            if (!this.Email.Contains("@") && this.Email.EndsWith(".com"))
            {
                throw new Exception($"El campo del email debe tener un @ y terminar con '.com'.");
            }
        }

        public bool IniciarSesion(string emailIngresado, string passwordIngresado)
        {
            if (this.Email == emailIngresado && this.Password == passwordIngresado) 
            {
                return this.Email == emailIngresado && this.Password == passwordIngresado;
            } else
            {
                throw new Exception($"Email o contraseña incorrecto.");
            }
        }
    }

    
}

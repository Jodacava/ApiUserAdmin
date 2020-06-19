using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAdministration.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Celular { get; set; }
        public string Rol { get; set; }
        public string Cuenta { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
    }
}

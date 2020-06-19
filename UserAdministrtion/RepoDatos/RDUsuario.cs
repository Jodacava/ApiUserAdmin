using System;
using System.Collections.Generic;
using System.Linq;
using UserAdministration.Models;

namespace UserAdministration.RepoDatos
{
    public class RDUsuario
    {

        public static List<Usuario> _listaUsuarios = new List<Usuario> { 
            new Usuario() { Id = 0, Nombre = "Root", Apellido = "Root", Celular = "1111111111", Cuenta="000", Rol="admin", Email="root", Contrasenia="12345678a" }
        };

        public IEnumerable<Usuario> ObtenerUsuario(int id)
        {
            if (id == 0)
            {
                return _listaUsuarios;
            }
            else
            {
                return _listaUsuarios.Where(x => x.Id == id);
            }            
        }

        public string CrearUsuario(Usuario newUser)
        {
            string respuesta = "Usuario: " + newUser.Nombre + ", creado con exito!";
            try
            {
                _listaUsuarios.Add(newUser);
            }
            catch (Exception ex)
            {
                respuesta = "Error al crear el usuario: " + newUser.Nombre + ". Error -> " + ex.Message;
            }
            
            return respuesta;
        }

        public string DelUsuario(int id)
        {
            string respuesta = "Usuario con id: " + id.ToString() + ", eliminado con Exito!";
            try{
                int idx = _listaUsuarios.FindIndex(x => x.Id == id);
                if (idx >= 0)
                {
                    _listaUsuarios.RemoveAt(idx);
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al borrar el usuario: " + id + ". Error -> " + ex.Message;
            }
            return respuesta;
        }

        public static Login GetUserForLogin(string Email, string Pass)
        {
            Login loginData = new Login();
            Usuario userData = new Usuario();
            userData = _listaUsuarios.Find(x=> x.Email == Email && x.Contrasenia == Pass);
            if (userData != null)
            {
                loginData.usuario = userData.Email;
                loginData.contrasenia = userData.Contrasenia;
            }            
            return loginData;
        }

        public static Usuario GetUserFromEmail(string Email)
        {
            Usuario userData = new Usuario();
            userData = _listaUsuarios.Find(x => x.Email == Email);
            return userData;
        }

        public static Usuario GetUserfromName(string Name)
        {
            Usuario userData = new Usuario();
            userData = _listaUsuarios.Find(x => x.Nombre == Name);
            return userData;
        }

        public string UpdateUsuario(Usuario newData)
        {
            int idx = _listaUsuarios.FindIndex(x=> x.Id == newData.Id);
            if (idx >= 0)
            {
                _listaUsuarios[idx].Nombre = newData.Nombre != null ? newData.Nombre : _listaUsuarios[idx].Nombre;
                _listaUsuarios[idx].Apellido = newData.Apellido != null ? newData.Apellido : _listaUsuarios[idx].Apellido;
                _listaUsuarios[idx].Celular = newData.Celular != null ? newData.Celular : _listaUsuarios[idx].Celular;
                _listaUsuarios[idx].Email = newData.Email != null ? newData.Email : _listaUsuarios[idx].Email;
                _listaUsuarios[idx].Contrasenia = newData.Contrasenia != null ? newData.Contrasenia : _listaUsuarios[idx].Contrasenia;
                _listaUsuarios[idx].Rol = newData.Rol != null ? newData.Rol : _listaUsuarios[idx].Rol;
                return "Usuario: " + newData.Id + ", Actualizado!";
            }
            else return "Usuario no existe!";
        }

        public string Transferir(string nombre, double monto)
        {
            int idx = _listaUsuarios.FindIndex(x => x.Nombre == nombre);
            if (idx >= 0)
            {
                Cuenta cuentaActualizada = RDCuentacs.updateBalance(_listaUsuarios[idx].Cuenta, monto);
                return "Nuevo balance: " + cuentaActualizada.Balance.ToString();
            }
            else return "Usuario no encontrado";           
        }
    }
}

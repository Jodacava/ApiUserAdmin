using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserAdministration.Models;
using UserAdministration.RepoDatos;
using Microsoft.AspNetCore.Authorization;

namespace UserAdministration.Controllers
{
    /// <summary>
    /// Controlador para los métodos que puede usasr un usuario no administrador.
    /// </summary>
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Endpoint para obtener usuario por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            RDUsuario usuarios = new RDUsuario();

            var user = usuarios.ObtenerUsuario(id);

            if (user.Count() == 0)
            {
                return NotFound("El cliente " + id.ToString() + " no existe.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Endpoint Get para obtener el balance del usuario. Recibe de parámetro el email del usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("getBalance/{email}")]
        public IActionResult GetBalance(string email)
        {           
            Usuario dataUsuario = new Usuario();
            dataUsuario = RDUsuario.GetUserFromEmail(email);
            if (dataUsuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }
            else
            {
                Cuenta cuentaActual = new Cuenta();
                cuentaActual = RDCuentacs.getBalance(dataUsuario.Cuenta);                
                return Ok(cuentaActual.Balance);
            }
        }

        /// <summary>
        /// Endpoint para realizar traslado de balance.
        /// </summary>
        /// <param name="email"> este parámetro entra por la url</param>
        /// <param name="cuentaDes"> este parámetro se determina por el body como objeto de la entidad Cuenta</param>
        /// <returns></returns>
        [HttpPut("{email}")]
        public IActionResult Transferir(string email, [FromBody] Cuenta cuentaDes)
        {
            RDUsuario usuarios = new RDUsuario();

            var respuesta = usuarios.Transferir(email, cuentaDes.Balance);

            return Ok(respuesta);
        }
    }
}

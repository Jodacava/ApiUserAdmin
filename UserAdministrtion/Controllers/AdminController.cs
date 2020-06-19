using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAdministration.Models;
using UserAdministration.RepoDatos;
using UserAdministration.Services;

namespace UserAdministration.Controllers
{
    /// <summary>
    /// Controlador para las opciones del administrador
    /// </summary>
    [Route("api/admin")]
    [Authorize] //Roles = "admin"
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Contructor con la inclusión del servicio de autenticación
        /// </summary>
        /// <param name="authService"></param>
        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint para saber si el rol del usuario registrado es Admin.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool isAdmin()
        {
            var apiClaims = User.Claims.ToList();
            bool isAdmin = apiClaims.Any(x => x.Type == "Roles" && x.Value == "admin");
            return isAdmin;
        }

        /// <summary>
        /// Endpoint para obtener los usuarios registrados
        /// </summary>
        /// <returns></returns>         
        [HttpGet("getUsuarios")]
        public IActionResult GetUsuarios()
        {
            if (isAdmin())
            {
                RDUsuario usuarios = new RDUsuario();
                return Ok(usuarios.ObtenerUsuario(0));
            }
            return BadRequest("Usuario no es Administrador.");
        }

        /// <summary>
        /// Endopint para la consulta de un usuario por Id
        /// </summary>
        /// <param name="id">parámetro que viene por la url</param>
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
        /// Endpoint para la creación de usuarios
        /// </summary>
        /// <param name="newUsuario">Parámetro con el objeto Usuario con la data del usuario</param>
        /// <returns></returns>
        [HttpPost("addUsuario")]
        public IActionResult AddUsuario(Usuario newUsuario)
        {
            if (isAdmin())
            {
                RDUsuario usuario = new RDUsuario();
                usuario.CrearUsuario(newUsuario);
                Cuenta newCuenta = new Cuenta();
                newCuenta.Numero = newUsuario.Cuenta;
                newCuenta.Balance = 0;
                RDCuentacs.createCuenta(newCuenta);
                return CreatedAtAction(nameof(AddUsuario), newUsuario);
            }
            return BadRequest("Usuario no es Administrador.");
        }

        /// <summary>
        /// Endpoint para eliminación de un usuario por su id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("delUsuario")]
        public IActionResult DelUsuario(int userId)
        {
            if (isAdmin())
            {
                string respuesta = "";
                RDUsuario usuario = new RDUsuario();
                respuesta = usuario.DelUsuario(userId);
                return Ok(respuesta);
            }
            return BadRequest("Usuario no es Administrador.");
        }

        /// <summary>
        /// Edpoint para modificar la información del usuario. Nota: no actualiza el monto.
        /// </summary>
        /// <param name="newUsuario"></param>
        /// <returns></returns>
        [HttpPut("updateUsuario")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateUsuario(Usuario newUsuario)
        {
            if (isAdmin())
            {
                string respuesta = "";
                RDUsuario usuario = new RDUsuario();
                respuesta = usuario.UpdateUsuario(newUsuario);
                return Ok(respuesta);
            }
            return BadRequest("Usuario no es Administrador.");
        }

        /// <summary>
        /// Endpoint para transferir monto al balance entre usuarios
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="cuentaDes"></param>
        /// <returns></returns>
        [HttpPut("{nombre}")]
        public IActionResult Transferir(string nombre, [FromBody] Cuenta cuentaDes)
        {
            if (isAdmin())
            {
                RDUsuario usuarios = new RDUsuario();

                var respuesta = usuarios.Transferir(nombre, cuentaDes.Balance);

                return Ok(respuesta);
            }
            return BadRequest("Usuario no es Administrador.");
        }
    }
}

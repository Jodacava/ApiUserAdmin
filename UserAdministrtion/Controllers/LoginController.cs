using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserAdministration.Models;
using UserAdministration.RepoDatos;
using UserAdministration.Services;

namespace UserAdministration.Controllers
{
    ///Controlador para el login por body y generador del token JWT
    [Route("api/login")]
    [ApiController]    
    public class LoginController : ControllerBase
    
    {

        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Construuctor con los llamados a las interfaces de servicio de autenticación y la configuración general
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="authService"></param>
        public LoginController(IConfiguration configuration, IAuthService authService)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Este endpoint se usa para vaildar al usuario y generar el tocken de JWT
        /// El usuario inicial Admin es root y su clave de acceso es 12345678a
        /// </summary>
        [HttpPost("getToken")]
        public IActionResult getToken([FromBody] Login data)
        {

            if (_authService.validateLogin(data.usuario, data.contrasenia))
            {
                Usuario dataUser = RDUsuario.GetUserFromEmail(data.usuario.ToLower());
                var date = DateTime.UtcNow;
                var expireDate = TimeSpan.FromHours(5);
                var expireDateTime = date.Add(expireDate);

                var token = _authService.generateToken(date, dataUser, expireDate);

                return Ok(new
                {
                    Token = token,
                    ExpireAt = expireDateTime
                });
            }
            else
            {
                return StatusCode(401);
            }
        }
    }
}

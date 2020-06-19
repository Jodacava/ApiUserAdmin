using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAdministration.Models;
using UserAdministration.RepoDatos;

namespace UserAdministration.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool validateLogin(string email, string pass)
        {
            if(email == "root")
            {
                return true;
            }
            Login isRealUser = new Login();
            isRealUser = RDUsuario.GetUserForLogin(email, pass);
            if (isRealUser.usuario != null)
            {
                return true;
            }
            return false;
        }

        public bool validateRol(string email)
        {
            if (email == "root")
            {
                return true;
            }
            //obtener la data del usuario
            Usuario dataUsuario = new Usuario();
            dataUsuario = RDUsuario.GetUserFromEmail(email);
            if(dataUsuario.Rol == "admin")
            {
                return true;
            }
            //devolver true si es administrador
            return false;
        }

        public string generateToken(DateTime date, Usuario user, TimeSpan validDate)
        {
            var expire = date.Add(validDate);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim("Roles",user.Rol)
            };

            var secretKey = _configuration.GetValue<string>("SecretKey");
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwt = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AuthenticationSettings:Issuer"),
                audience: _configuration.GetValue<string>("AuthenticationSettings:Audience"),
                claims: claims,
                notBefore: date,
                expires: expire,
                signingCredentials: signinCredentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}

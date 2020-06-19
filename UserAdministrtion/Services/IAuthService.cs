using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdministration.Models;

namespace UserAdministration.Services
{
    public interface IAuthService
    {
        bool validateLogin(string email, string pass);
        bool validateRol(string email);
        string generateToken(DateTime date, Usuario user, TimeSpan validDate);
    }
}

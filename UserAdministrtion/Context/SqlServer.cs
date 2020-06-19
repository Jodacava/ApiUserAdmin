using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdministration.Models;

namespace UserAdministration.Context
{
    public class SqlServer:DbContext
    {
        public SqlServer(DbContextOptions<SqlServer> options):base(options)
        {
        }

        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Usuario> User { get; set; }
    }
}

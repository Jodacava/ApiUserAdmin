using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using UserAdministration.Context;
using UserAdministration.Services;
using Microsoft.OpenApi.Models;
using System.IO;

namespace UserAdministration
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SqlServer>(options => options.UseSqlServer(Configuration.GetConnectionString("sqlServer")));
            services.AddControllers();
            services.AddScoped<IAuthService, AuthService>();

            services.AddMvc();

            services.AddControllers();

            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });

            var issuer = _configuration["AuthenticationSettings:Issuer"];
            var audience = _configuration["AuthenticationSettings:Audience"];
            var signingKey = _configuration["AuthenticationSettings:SigningKey"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.Audience = audience;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey))
                };
            });

            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, "UserAdministration.xml");
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("ver1", new OpenApiInfo()
                {
                    Title = "Administrador de Usuarios",
                    Version = "v1"
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Inserte el JWT Token.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                    }
                });
                config.IncludeXmlComments(xmlFilePath);
            });
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(config => { config.RouteTemplate = "/swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(config =>
                config.SwaggerEndpoint("/swagger/ver1/swagger.json", "Api Admin. Usuarios")
            );

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ApiRest_EntityFramework.Config;
using ApiRest_EntityFramework.Controllers;
using ApiRest_EntityFramework.Tools;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiRest_EntityFramework
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            try
            {

                var builder = WebApplication.CreateBuilder(args);


                // CONFIGURAR SERVICIOS
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    // Definir el esquema de seguridad para JWT
                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Por favor ingrese el token JWT en el formato: Bearer {token}"
                    });

                    // Requerir autenticación en todos los endpoints
                    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                });

                // CONFIGURA JSON CON REFERENCEHANDLER.PRESERVE PARA MANEJAR CICLOS INFINITOS
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        //options.JsonSerializerOptions.MaxDepth = 64;  // OPCIONAL: AUMENTA EL LÍMITE DE PROFUNDIDAD SI ES NECESARIO
                    });


                // CONFIGURACIÓN DE JWT DESDE APPSETTINGS.JSON
                var jwtSettings = builder.Configuration.GetSection("Jwt");
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };


                // CONFIGURACION CREDENCIALES USUARIO
                var jwtSettingsCredenciales = builder.Configuration.GetSection("CredencialesUsuario");
                AuthController.correo = jwtSettingsCredenciales["correo"];
                AuthController.pass = jwtSettingsCredenciales["contrasena"];


                // CONFIGURAR AUTENTICACIÓN JWT
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = parameters;
                    });


                // AÑADIR TOKENSERVICE COMO SERVICIO TRANSITORIO
                builder.Services.AddScoped<TokenService>();


                // CONFIGURAR DBCONTEXT USANDO LA CADENA DE CONEXIÓN DE APPSETTINGS.JSON
                builder.Services.AddDbContext<AplicationDbContext>(options =>
                    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


                // CREACION DE LA APP WEB
                var app = builder.Build();


                // CARGAR DATOS DESDE CSV SOLO SI NO EXISTEN EN LA BASE DE DATOS
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AplicationDbContext>();

                    if (await AppConfig.cargarConfiguracion(context))
                    {
                        Console.WriteLine("CONFIGURACION CARGADA CORRECTAMENTE");
                    }
                    else
                    {
                        Console.WriteLine("ERROR AL CARGAR LA CONFIGURACION");
                        Environment.Exit(1); // DETENER LA APLICACIÓN SI FALLA LA CARGA DE CONFIGURACIÓN
                    }
                }


                // CONFIGURACIÓN DE ERRORES Y ENTORNO DE DESARROLLO
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                else
                {
                    // CONFIGURAR MANEJO DE EXCEPCIONES EN PRODUCCIÓN
                    app.UseExceptionHandler(errorApp =>
                    {
                        errorApp.Run(async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            context.Response.ContentType = "text/html";

                            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (exceptionHandlerFeature != null)
                            {
                                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                                logger.LogError(exceptionHandlerFeature.Error, "SE PRODUJO UNA EXCEPCIÓN.");
                                await context.Response.WriteAsync("OCURRIÓ UN ERROR EN EL SERVIDOR.");
                            }
                        });
                    });
                }


                // MIDDLEWARE DE AUTENTICACIÓN Y AUTORIZACIÓN
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();

            }
            catch (Exception)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}

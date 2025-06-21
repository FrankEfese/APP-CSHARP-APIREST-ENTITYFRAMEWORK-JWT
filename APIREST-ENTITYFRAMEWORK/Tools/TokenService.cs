using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiRest_EntityFramework.Tools
{
    public class TokenService
    {

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) // INYECCION DE LA CONFIGURACION
        {
            this._configuration = configuration;
        }


        public string GenerateJwtToken() // MÉTODO PARA GENERAR UN TOKEN JWT
        {
            try
            {
                // CREA UNA CLAVE DE SEGURIDAD USANDO LA CLAVE DEL JSON
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Jwt:Key"]));


                // DEFINE LAS CREDENCIALES DE FIRMA UTILIZANDO LA CLAVE DE SEGURIDAD Y EL ALGORITMO HMAC SHA256
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                // CREA EL TOKEN JWT
                var token = new JwtSecurityToken(
                    issuer: this._configuration["Jwt:Issuer"],          // DEFINE EL EMISOR DEL TOKEN, TOMADO DE LA CONFIGURACIÓN
                    audience: this._configuration["Jwt:Audience"],      // DEFINE EL PÚBLICO DEL TOKEN, TOMADO DE LA CONFIGURACIÓN
                    expires: DateTime.Now.AddHours(1),             // ESTABLECE LA FECHA DE EXPIRACIÓN DEL TOKEN A 1 HORA
                    signingCredentials: credentials                // ESTABLECE LAS CREDENCIALES DE FIRMA PARA EL TOKEN
                );


                // GENERA EL TOKEN COMO UNA CADENA USANDO EL MANEJADOR DE TOKENS JWT
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR AL GENERAR EL JWT");
                return null;
            }
        }

    }
}

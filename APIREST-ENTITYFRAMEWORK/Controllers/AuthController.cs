using Microsoft.AspNetCore.Mvc;
using ApiRest_EntityFramework.Models;
using ApiRest_EntityFramework.Tools;

namespace ApiRest_EntityFramework.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        // VARIABLES-CREDENCIALES

        public static string correo = "";

        public static string pass = "";


        private readonly TokenService _tokenService;
        public AuthController(TokenService tokenService) // INYECCIÓN DE DEPENDENCIA DE TOKENSERVICE
        {
            this._tokenService = tokenService;
        }


        // METODOS HTTP

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentials credentials) // MÉTODO PARA LA AUTORIZACIÓN DEL USUARIO Y OBTENCIÓN DEL TOKEN
        {
            try
            {
                // BD


                if (credentials.usuario.Equals(correo) && credentials.password.Equals(pass))
                {
                    var token = this._tokenService.GenerateJwtToken();

                    if (token == null) return Unauthorized(new { message = "CREDENCIALES NO VÁLIDAS" });

                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized(new { message = "CREDENCIALES NO VÁLIDAS" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "ERROR EN EL SERVIDOR" });
            }
        }


    }
}

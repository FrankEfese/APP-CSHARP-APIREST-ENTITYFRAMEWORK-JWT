using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRest_EntityFramework.Models;

namespace ApiRest_EntityFramework.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JugadorController : Controller
    {

        private readonly AplicationDbContext _context;

        public JugadorController(AplicationDbContext context) // INYECCIÓN DE DEPENDENCIA DE DBCONTEXT
        {
            this._context = context;
        }


        // METODOS HTTP

        [HttpGet]
        public async Task<ActionResult<List<Jugador>>> GetAll() // METODO PARA OBTENER TODOS LOS JUGADORES
        {
            try
            {

                var jugadores = await this._context.jugadores.Include(j => j.equipo).ToListAsync();

                if (jugadores == null) return NotFound();

                return jugadores;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpGet("terminates")]
        public async Task<ActionResult<List<Jugador>>> GetAllTerminates() // METODO PARA OBTENER TODOS LOS JUGADORES EN PARO
        {
            try
            {

                var jugadoresEnParo = await this._context.jugadores.Include(j => j.equipo).Where(j => j.idEquipo == null).ToListAsync();

                if (jugadoresEnParo == null) return NotFound();

                return jugadoresEnParo;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Jugador>> Get(int id) // METODO PARA OBTENER UN JUGADOR
        {

            try
            {

                var jugador = await this._context.jugadores.Include(j => j.equipo).FirstOrDefaultAsync(j => j.id == id);

                if (jugador == null) return NotFound();

                return jugador;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpPost("post")]
        public async Task<ActionResult<Jugador>> Post([FromBody] Jugador nuevoJugador) // METODO PARA INSERTAR UN JUGADOR
        {

            try
            {

                if (!await this._context.jugadores.AnyAsync(j => j.nombre.ToLower().Equals(nuevoJugador.nombre.ToLower())))
                {
                    await this._context.jugadores.AddAsync(nuevoJugador);
                    await this._context.SaveChangesAsync();

                    var jugador = await this._context.jugadores.Include(j => j.equipo).FirstOrDefaultAsync(j => j.nombre.ToLower() == nuevoJugador.nombre.ToLower());

                    return Ok(jugador.id);
                }
                else return Conflict();


            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Jugador>> Delete(int id) // METODO PARA ELIMINAR UN JUGADOR
        {

            try
            {

                var jugador = await this._context.jugadores.FirstOrDefaultAsync(j => j.id == id);

                if (jugador == null) return NotFound();

                this._context.jugadores.Remove(jugador);

                await this._context.SaveChangesAsync();

                return Ok("Jugador eliminado con exito.");

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpPut("put/{id}")]
        public async Task<ActionResult<Jugador>> Put(int id, [FromBody] Jugador jugadorActualizado) // METODO PARA ACTUALIZAR UN JUGADOR
        {

            try
            {

                if (!await this._context.jugadores.AnyAsync(j => j.nombre.ToLower().Equals(jugadorActualizado.nombre.ToLower())) ||
                    (await this._context.jugadores.Where(j => j.nombre.ToLower().Equals(jugadorActualizado.nombre.ToLower())).Select(j => j.id).FirstOrDefaultAsync() == id))
                {

                    var jugador = await this._context.jugadores.Include(j => j.equipo).FirstOrDefaultAsync(j => j.id == id);

                    if (jugador == null) return NotFound();

                    jugador.nombre = jugadorActualizado.nombre;
                    jugador.edad = jugadorActualizado.edad;
                    jugador.nacionalidad = jugadorActualizado.nacionalidad;
                    jugador.altura = jugadorActualizado.altura;
                    jugador.peso = jugadorActualizado.peso;

                    await this._context.SaveChangesAsync();

                    return Ok("Jugador actualizado con exito.");

                }
                else return Conflict();

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpPut("put/signing/{id}")]
        public async Task<ActionResult> PutSigning(int id, [FromBody] int idEquipoFichaje) // METODO PARA ACTUALIZAR EL FICHAJE DE UN JUGADOR
        {

            try
            {

                var jugador = await this._context.jugadores.Include(j => j.equipo).FirstOrDefaultAsync(j => j.id == id);

                if(jugador != null)
                {

                    if (jugador.idEquipo == null && await this._context.equipos.AnyAsync(e => e.id == idEquipoFichaje))
                    {

                        jugador.idEquipo = idEquipoFichaje;
                        await this._context.SaveChangesAsync();

                        return Ok("Fichaje realizado con exito.");

                    }
                    else return Conflict();
                    

                }else return NotFound();


            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpPut("put/termination/{id}")]
        public async Task<ActionResult> PutTermination(int id, [FromBody] int idEquipoDespido) // METODO PARA ACTUALIZAR EL DESPIDO DE UN JUGADOR
        {

            try
            {

                var jugador = await this._context.jugadores.Include(j => j.equipo).FirstOrDefaultAsync(j => j.id == id);

                if (jugador != null)
                {

                    if (jugador.idEquipo == idEquipoDespido)
                    {
                        jugador.idEquipo = null;
                        await this._context.SaveChangesAsync();

                        return Ok("Despido realizado con exito.");
                    }
                    else return Conflict();
                    
                }
                else return NotFound();


            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount() // METODO PARA OBTENER EL TOTAL DE JUGADORES
        {

            try
            {

                int total = await this._context.jugadores.CountAsync();

                return total;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpGet("countTerminates")]
        public async Task<ActionResult<int>> GetCountTerminates() // METODO PARA OBTENER EL TOTAL DE JUGADORES PARADOS
        {

            try
            {

                int total = await this._context.jugadores.Include(j => j.equipo).Where(j => j.idEquipo == null).CountAsync();

                return total;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }
    }
}

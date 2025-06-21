using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRest_EntityFramework.Models;

namespace ApiRest_EntityFramework.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipoController : Controller
    {
        
        private readonly AplicationDbContext _context;

        public EquipoController(AplicationDbContext context) // INYECCIÓN DE DEPENDENCIA DE DBCONTEXT
        {
            this._context = context;
        }


        // METODOS HTTP

        [HttpGet]
        public async Task<ActionResult<List<Equipo>>> GetAll() // METODO PARA OBTENER TODOS LOS EQUIPOS
        {
            try
            {

                var equipos =  await this._context.equipos.Include(e => e.jugadores).ToListAsync();

                if(equipos == null) return NotFound();

                return equipos;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }
            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> Get(int id) // METODO PARA OBTENER UN EQUIPO
        {

            try
            {

                var equipo = await this._context.equipos.Include(e => e.jugadores).FirstOrDefaultAsync(e => e.id == id);

                if (equipo == null) return NotFound();

                return equipo;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpPost("post")]
        public async Task<ActionResult<Equipo>> Post([FromBody] Equipo nuevoEquipo) // METODO PARA INSERTAR UN EQUIPO
        {

            try
            {

                if (!await this._context.equipos.AnyAsync(e => e.nombre.ToLower().Equals(nuevoEquipo.nombre.ToLower())))
                {
                    await this._context.equipos.AddAsync(nuevoEquipo);
                    await this._context.SaveChangesAsync();

                    var equipo = await this._context.equipos.Include(e => e.jugadores).FirstOrDefaultAsync(e => e.nombre.ToLower() == nuevoEquipo.nombre.ToLower());

                    return Ok(equipo.id);
                }
                else return Conflict();


            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Equipo>> Delete(int id) // METODO PARA ELIMINAR UN EQUIPO
        {

            try
            {

                var equipo = await this._context.equipos.FirstOrDefaultAsync(e => e.id == id);

                if (equipo == null) return NotFound();

                this._context.equipos.Remove(equipo);

                await this._context.SaveChangesAsync();

                return Ok("Equipo eliminado con exito.");

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }



        [HttpPut("put/{id}")]
        public async Task<ActionResult<Equipo>> Put(int id , [FromBody] Equipo equipoActualizado) // METODO PARA ACTUALIZAR UN EQUIPO
        {

            try
            {

                if (!await this._context.equipos.AnyAsync(e => e.nombre.ToLower().Equals(equipoActualizado.nombre.ToLower())) ||
                    (await this._context.equipos.Where(e => e.nombre.ToLower().Equals(equipoActualizado.nombre.ToLower())).Select(e => e.id).FirstOrDefaultAsync() == id))
                {

                    var equipo = await this._context.equipos.Include(e => e.jugadores).FirstOrDefaultAsync(e => e.id == id);

                    if (equipo == null) return NotFound();

                    equipo.nombre = equipoActualizado.nombre;
                    equipo.liga = equipoActualizado.liga;

                    await this._context.SaveChangesAsync();

                    return Ok("Equipo actualizado con exito.");

                }
                else return Conflict();


            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }


        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount() // METODO PARA OBTENER EL TOTAL DE EQUIPOS
        {

            try
            {

                int total = await this._context.equipos.CountAsync();

                return total;

            }
            catch (Exception)
            {
                return StatusCode(500, "ERROR DE SERVIDOR");
            }

        }

    }
}

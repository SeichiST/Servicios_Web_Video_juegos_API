using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosAPIController : ControllerBase
    {
        private readonly TiendaJuegosContext db;
        public JuegosAPIController(TiendaJuegosContext _db)
        {
            db = _db;
        }

        // GET: api/<JuegosAPIController>/GetJuegos
        [HttpGet("GetJuegos")]
        public async Task<List<Juego>> GetJuegos()
        {
            var listado = await db.Juegos.ToListAsync();
            return listado;
        }

        // GET api/<JuegosAPIController>/GetJuego/5
        [HttpGet("GetJuego/{id}")]
        public async Task<ActionResult<Juego>> GetJuego(int id)
        {
            var obj = await db.Juegos.FindAsync(id);
            if (obj == null)
            {
                return BadRequest("Codigo No Existe");
            }

            return Ok(obj);
        }

        // POST api/<JuegosAPIController>
        [HttpPost]
        public async Task<ActionResult<Juego>> PostJuego([FromBody] Juego value)
        {
            try
            {
                await db.Juegos.AddAsync(value);
                await db.SaveChangesAsync();

                return Ok("Juego Registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // PUT api/<JuegosAPIController>
        [HttpPut]
        public async Task<ActionResult<Juego>> PutJuego([FromBody] Juego value)
        {
            try
            {
                await Task.Run(() => db.Juegos.Update(value));

                await db.SaveChangesAsync(); // actualiza la BD

                return Ok("Juego Actualizado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // DELETE api/<JuegosAPIController>/DeleteJuego/5
        [HttpDelete("DeleteJuego/{id}")]
        public async Task<ActionResult<Juego>> DeleteJuego(int id)
        {
            try
            {
                var buscado = await db.Juegos.FindAsync(id);

                if (buscado == null)
                {
                    return BadRequest("Error, no existe el Codigo");
                }

                // Eliminación lógica cambiando el campo Activo a false
                buscado.Activo = false;

                // actualiza el registro en la BD
                await db.SaveChangesAsync();

                return Ok("Juego Eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }
    }
}
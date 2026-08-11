using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesAPIController : ControllerBase
    {
        private readonly TiendaJuegosContext db;
        public MensajesAPIController(TiendaJuegosContext _db)
        {
            db = _db;
        }

        // GET: api/<MensajesAPIController>/GetMensajes
        [HttpGet("GetMensajes")]
        public async Task<List<Mensaje>> GetMensajes()
        {
            return await db.Mensajes.ToListAsync();
        }

        // POST: api/<MensajesAPIController>
        [HttpPost]
        public async Task<ActionResult> PostMensaje([FromBody] Mensaje value)
        {
            try
            {
                value.FechaEnvio = DateTime.Now;
                value.Estado = "1";

                await db.Mensajes.AddAsync(value);
                await db.SaveChangesAsync();

                return Ok("Mensaje enviado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }
    }
}
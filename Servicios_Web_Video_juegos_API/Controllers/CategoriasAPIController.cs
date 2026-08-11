using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasAPIController : ControllerBase
    {
        private readonly TiendaJuegosContext db;

        public CategoriasAPIController(TiendaJuegosContext _db)
        {
            db = _db;
        }

        // GET: api/<CategoriasAPIController>/GetCategorias
        [HttpGet("GetCategorias")]
        public async Task<List<Categoria>> GetCategorias()
        {
            var listado = await db.Categorias.ToListAsync();
            return listado;
        }

        // GET api/<CategoriasAPIController>/GetCategoria/CAT001
        [HttpGet("GetCategoria/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id)
        {
            var obj = await db.Categorias.FindAsync(id);
            if (obj == null)
            {
                return BadRequest("Codigo No Existe");
            }

            return Ok(obj);
        }

        // POST api/<CategoriasAPIController>
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria([FromBody] Categoria value)
        {
            try
            {
                await db.Categorias.AddAsync(value);
                await db.SaveChangesAsync();

                return Ok("Categoría Registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // PUT api/<CategoriasAPIController>
        [HttpPut]
        public async Task<ActionResult<Categoria>> PutCategoria([FromBody] Categoria value)
        {
            try
            {
                await Task.Run(() => db.Categorias.Update(value));

                await db.SaveChangesAsync();

                return Ok("Categoría Actualizada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // DELETE api/<CategoriasAPIController>/DeleteCategoria/CAT001
        [HttpDelete("DeleteCategoria/{id}")]
        public async Task<ActionResult<Categoria>> DeleteCategoria(string id)
        {
            try
            {
                var buscado = await db.Categorias.FindAsync(id);

                if (buscado == null)
                {
                    return BadRequest("Error, no existe el Codigo");
                }

                db.Categorias.Remove(buscado);
                await db.SaveChangesAsync();

                return Ok("Categoría Eliminada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }
    }
}
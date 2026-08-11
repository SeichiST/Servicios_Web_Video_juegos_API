using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Dto;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesAPIController : ControllerBase
    {
        private readonly TiendaJuegosContext db;

        public ClientesAPIController(TiendaJuegosContext _db)
        {
            db = _db;
        }

        // GET: api/<ClientesAPIController>/GetClientes
        [HttpGet("GetClientes")]
        public async Task<List<Cliente>> GetClientes()
        {
            var listado = await db.Clientes.ToListAsync();
            return listado;
        }

        // GET api/<ClientesAPIController>/GetCliente/5
        [HttpGet("GetCliente/{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var obj = await db.Clientes.FindAsync(id);
            if (obj == null)
            {
                return BadRequest("Codigo No Existe");
            }

            return Ok(obj);
        }

        // POST api/<ClientesAPIController>
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente value)
        {
            try
            {
                await db.Clientes.AddAsync(value);
                await db.SaveChangesAsync();

                return Ok("Cliente Registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // PUT api/<ClientesAPIController>
        [HttpPut]
        public async Task<ActionResult<Cliente>> PutCliente([FromBody] Cliente value)
        {
            try
            {
                await Task.Run(() => db.Clientes.Update(value));

                await db.SaveChangesAsync();

                return Ok("Cliente Actualizado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // DELETE api/<ClientesAPIController>/DeleteCliente/5
        [HttpDelete("DeleteCliente/{id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            try
            {
                var buscado = await db.Clientes.FindAsync(id);

                if (buscado == null)
                {
                    return BadRequest("Error, no existe el Codigo");
                }

                //('0' inactivo, '1' activo)
                buscado.Estado = "0";

                await db.SaveChangesAsync();

                return Ok("Cliente Desactivado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Message);
            }
        }

        // POST api/<ClientesAPIController>/Login
        [HttpPost("Login")]
        public async Task<ActionResult<Cliente>> Login([FromBody] LoginRequestDto request)
        {
            var cliente = await db.Clientes
                .FirstOrDefaultAsync(c => c.Correo == request.Correo && c.Estado == "1");

            if (cliente == null)
            {
                return BadRequest("Correo o contraseña incorrectos");
            }

            if (cliente.Password != request.Password)
            {
                return BadRequest("Correo o contraseña incorrectos");
            }

            return Ok(cliente);
        }
    }
}
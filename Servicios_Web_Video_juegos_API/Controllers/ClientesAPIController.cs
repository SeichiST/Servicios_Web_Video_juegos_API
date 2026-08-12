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
                // 1. Encriptar la contraseña del cliente antes de guardar
                if (!string.IsNullOrEmpty(value.Password))
                {
                    value.Password = BCrypt.Net.BCrypt.HashPassword(value.Password);
                }

                // 2. Gestionar la asignación de roles
                if (value.IdRols != null && value.IdRols.Any())
                {
                    var idsRoles = value.IdRols.Select(r => r.IdRol).ToList();
                    var rolesBD = await db.Roles.Where(r => idsRoles.Contains(r.IdRol)).ToListAsync();
                    value.IdRols = rolesBD;
                }
                else
                {
                    // Asignar rol por defecto (ROLE_CLIENTE = 1) si no se envió alguno
                    var rolCliente = await db.Roles.FindAsync(1);
                    if (rolCliente != null)
                    {
                        value.IdRols.Add(rolCliente);
                    }
                }

                // Asegurar estado activo por defecto
                if (string.IsNullOrEmpty(value.Estado))
                {
                    value.Estado = "1";
                }

                await db.Clientes.AddAsync(value);
                await db.SaveChangesAsync();

                return Ok("Cliente Registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
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
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
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

                // ('0' inactivo, '1' activo)
                buscado.Estado = "0";

                await db.SaveChangesAsync();

                return Ok("Cliente Desactivado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
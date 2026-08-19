using Microsoft.AspNetCore.Mvc;
using Servicios_Web_Video_juegos_API.Dto;
using Servicios_Web_Video_juegos_API.Models;
using Servicios_Web_Video_juegos_API.Controllers;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrarseAPIController : Controller
    {
        private readonly TiendaJuegosContext db;

        public RegistrarseAPIController (TiendaJuegosContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(RegistrarseDto dto)
        {
            var clienteExistente = await db.Clientes.FirstOrDefaultAsync(c => c.Correo == dto.Correo);
            if(clienteExistente != null)
            {
                return BadRequest(new
                {
                    mensaje = "El correo ya esta registrado"
                });
            }

            var cliente = new Cliente
            {
                Apellidos = dto.Apellidos,
                Nombres = dto.Nombres,
                Dni = dto.Dni,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                FechaNacimiento = dto.FechaNacimiento,
                Sexo = dto.Sexo,
                Correo = dto.Correo,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Estado = "1"
            };

            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Cliente registrado correctamente",
                idCliente = cliente.IdCliente
            });
        }
    }
}

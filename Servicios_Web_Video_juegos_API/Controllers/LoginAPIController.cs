using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Dto;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAPIController : Controller
    {
        private readonly TiendaJuegosContext db;

        public LoginAPIController(TiendaJuegosContext _db)
        {
            db = _db;

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            var usuario = await db.Clientes.FirstOrDefaultAsync(c => c.Correo == login.Correo);

            if (usuario == null)
            {
                return Unauthorized("Correo o contraseña incorrectos");
            }

            bool passwordCorrecto = BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password);

            if (!passwordCorrecto)
            {
                return Unauthorized("Correo o contraseña incorrectos");
            }

            return Ok(usuario);

        }
        


    }
}

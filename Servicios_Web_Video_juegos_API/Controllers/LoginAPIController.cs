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
            var usuario = await db.Clientes
                //Cuando activo esto no me deja logearme para nada, me sale el error de login y cuando lo desactivo si me deja.
                //Ya lo arreglé y ahora va por roles, cuando es admin lo manda al panel admin d1
                .Include(c => c.IdRols)
                .FirstOrDefaultAsync(c => c.Correo == login.Correo);

            if (usuario == null)
            {
                return Unauthorized("Correo o contraseña incorrectos");
            }

            bool passwordCorrecto = BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password);

            if (!passwordCorrecto)
            {
                return Unauthorized("Correo o contraseña incorrectos");
            }

//(Tipo Anónimo) Genera un JSON sin referencias circulares
//y con los nombres exactos que espera la aplicación MVC.
            return Ok(new {
                usuario.IdCliente,
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                IdRols = usuario.IdRols.Select(r => new {
                    r.IdRol,
                    r.Nombre
                })
            });

        }

            //[ignorar si no va a consultar con LoginAPI]
            //Alter trayendo todos los datos por si se requiere en un futuro.
            // Devolvemos el cliente COMPLETO sin referencias circulares
            //return Ok(new
            //{
            //        usuario.IdCliente,
            //    usuario.Nombres,
            //    usuario.Apellidos,
            //    usuario.Dni,
            //    usuario.Direccion,
            //    usuario.Telefono,
            //    usuario.FechaNacimiento,
            //    usuario.Sexo,
            //    usuario.Correo,
            //    usuario.Estado,
            //    // Al proyectar la lista de roles sin devolver la entidad Rol completa se rompe la referencia circular
            //    IdRols = usuario.IdRols.Select(r => new
            //    {
            //        r.IdRol, 
            //        r.Nombre
            //    })
            //});



    }
}

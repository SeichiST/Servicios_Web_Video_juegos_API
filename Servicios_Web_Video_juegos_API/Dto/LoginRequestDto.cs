using Microsoft.AspNetCore.Mvc;

namespace Servicios_Web_Video_juegos_API.Dto
{
    public class LoginRequestDto
    {
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

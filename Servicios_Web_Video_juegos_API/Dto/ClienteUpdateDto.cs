namespace Servicios_Web_Video_juegos_MVC.Dto
{
    public class ClienteUpdateDto
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Dni { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        // Como NO acepta nulos en la BD, se usa DateOnly directamente
        public DateOnly FechaNacimiento { get; set; }

        public string? Sexo { get; set; }
        public string Correo { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
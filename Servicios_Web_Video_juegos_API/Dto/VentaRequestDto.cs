namespace Servicios_Web_Video_juegos_API.Dto
{
    public class VentaRequestDto
    {
        public int IdCliente { get; set; }
        public List<DetalleRequestDto> Detalles { get; set; } = new();
    }

 
}

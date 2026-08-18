namespace Servicios_Web_Video_juegos_API.Dto
{
    public class VentaResponseDto
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
        public string NombreCliente { get; set; }
        public List<DetalleResponseDto> Detalles { get; set; }
    }
}

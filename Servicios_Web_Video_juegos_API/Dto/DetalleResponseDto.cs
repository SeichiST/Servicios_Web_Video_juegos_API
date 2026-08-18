namespace Servicios_Web_Video_juegos_API.Dto
{
    public class DetalleResponseDto
    {
        public int IdJuegos { get; set; }
        public string NombreJuego { get; set; }
        public string Imagen { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}

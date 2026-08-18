using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios_Web_Video_juegos_API.Dto;
using Servicios_Web_Video_juegos_API.Models;

namespace Servicios_Web_Video_juegos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasAPIController : ControllerBase
    {
        private readonly TiendaJuegosContext db;

        public VentasAPIController(TiendaJuegosContext _db)
        {
            db = _db;
        }
        // GET: api/<VentasAPIController>/GetVentas (Reporte general)
        [HttpGet("GetVentas")]
        public async Task<List<VentaResponseDto>> GetVentas()
        {
            var listado = await db.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.IdJuegosNavigation)
                .ToListAsync();

            return listado.Select(obj => new VentaResponseDto
            {
                IdVenta = obj.IdVenta,
                FechaVenta = obj.FechaVenta,
                MontoTotal = obj.MontoTotal,
                Estado = obj.Estado,
                NombreCliente = $"{obj.IdClienteNavigation.Nombres} {obj.IdClienteNavigation.Apellidos}",
                Detalles = obj.Detalles.Select(d => new DetalleResponseDto
                {
                    IdJuegos = d.IdJuegos,
                    NombreJuego = d.IdJuegosNavigation.Descripcion,
                    Imagen = d.IdJuegosNavigation.Imagen,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio
                }).ToList()
            }).ToList();
        }

        // GET api/<VentasAPIController>/GetVenta/5
        [HttpGet("GetVenta/{id}")]
        public async Task<ActionResult<VentaResponseDto>> GetVenta(int id)
        {
            var obj = await db.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.IdJuegosNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (obj == null)
                return BadRequest("Codigo No Existe");

            var dto = new VentaResponseDto
            {
                IdVenta = obj.IdVenta,
                FechaVenta = obj.FechaVenta,
                MontoTotal = obj.MontoTotal,
                Estado = obj.Estado,
                NombreCliente = $"{obj.IdClienteNavigation.Nombres} {obj.IdClienteNavigation.Apellidos}",
                Detalles = obj.Detalles.Select(d => new DetalleResponseDto
                {
                    IdJuegos = d.IdJuegos,
                    NombreJuego = d.IdJuegosNavigation.Descripcion,
                    Imagen = d.IdJuegosNavigation.Imagen,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio
                }).ToList()
            };

            return Ok(dto);
        }

        // GET api/<VentasAPIController>/GetVentasCliente/1 (Historial por cliente)
        [HttpGet("GetVentasCliente/{idCliente}")]
        public async Task<List<VentaResponseDto>> GetVentasCliente(int idCliente)
        {
            var listado = await db.Ventas
                .Where(v => v.IdCliente == idCliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.IdJuegosNavigation)
                .Include(v => v.IdClienteNavigation)
                .ToListAsync();

            return listado.Select(obj => new VentaResponseDto
            {
                IdVenta = obj.IdVenta,
                FechaVenta = obj.FechaVenta,
                MontoTotal = obj.MontoTotal,
                Estado = obj.Estado,
                NombreCliente = $"{obj.IdClienteNavigation.Nombres} {obj.IdClienteNavigation.Apellidos}",
                Detalles = obj.Detalles.Select(d => new DetalleResponseDto
                {
                    IdJuegos = d.IdJuegos,
                    NombreJuego = d.IdJuegosNavigation.Descripcion,
                    Imagen = d.IdJuegosNavigation.Imagen,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio
                }).ToList()
            }).ToList();
        }

        // POST api/<VentasAPIController>/RegistrarVenta (Proceso de Carrito de Compras)
        [HttpPost("RegistrarVenta")]
        public async Task<ActionResult> RegistrarVenta([FromBody] VentaRequestDto request)
        {
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                // 1. Crear cabecera de la venta
                var nuevaVenta = new Venta
                {
                    IdCliente = request.IdCliente,
                    FechaVenta = DateTime.Now,
                    MontoTotal = request.Detalles.Sum(d => d.Cantidad * d.Precio),
                    Estado = "1"
                };

                await db.Ventas.AddAsync(nuevaVenta);
                await db.SaveChangesAsync();

                // 2. Registrar cada ítem en el Detalle
                foreach (var item in request.Detalles)
                {
                    var detalle = new Detalle
                    {
                        IdVenta = nuevaVenta.IdVenta,
                        IdJuegos = item.IdJuegos,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Estado = "1"
                    };

                    await db.Detalles.AddAsync(detalle);
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Venta registrada con éxito", idVenta = nuevaVenta.IdVenta });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.InnerException!.Message);
            }
        }
    }
}
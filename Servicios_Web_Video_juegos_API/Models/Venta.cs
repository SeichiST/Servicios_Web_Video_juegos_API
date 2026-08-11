using System;
using System.Collections.Generic;

namespace Servicios_Web_Video_juegos_API.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal MontoTotal { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Detalle> Detalles { get; set; } = new List<Detalle>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}

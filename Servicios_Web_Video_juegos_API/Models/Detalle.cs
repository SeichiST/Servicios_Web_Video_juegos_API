using System;
using System.Collections.Generic;

namespace Servicios_Web_Video_juegos_API.Models;

public partial class Detalle
{
    public int IdVenta { get; set; }

    public int IdJuegos { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Juego IdJuegosNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}

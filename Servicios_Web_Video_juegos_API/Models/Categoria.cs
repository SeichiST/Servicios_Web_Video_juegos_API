using System;
using System.Collections.Generic;

namespace Servicios_Web_Video_juegos_API.Models;

public partial class Categoria
{
    public string IdCategoria { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Juego> Juegos { get; set; } = new List<Juego>();
}

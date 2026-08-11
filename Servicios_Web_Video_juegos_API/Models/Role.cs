using System;
using System.Collections.Generic;

namespace Servicios_Web_Video_juegos_API.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cliente> IdClientes { get; set; } = new List<Cliente>();
}

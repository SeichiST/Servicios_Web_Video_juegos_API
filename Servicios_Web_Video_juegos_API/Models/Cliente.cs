using System;
using System.Collections.Generic;

namespace Servicios_Web_Video_juegos_API.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Apellidos { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<Role> IdRols { get; set; } = new List<Role>();
}

using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Estatus { get; set; }

    public int IdVendedor { get; set; }

    public byte IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Vendedor IdVendedorNavigation { get; set; } = null!;
}

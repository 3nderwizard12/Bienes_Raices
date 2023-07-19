using System;
using System.Collections.Generic;

namespace DL;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public int? Edad { get; set; }

    public string Telefono { get; set; } = null!;

    public string? Observaciones { get; set; }

    public int IdVendedor { get; set; }

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();

    public virtual Vendedor IdVendedorNavigation { get; set; } = null!;
}

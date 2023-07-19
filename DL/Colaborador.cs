﻿using System;
using System.Collections.Generic;

namespace DL;

public partial class Colaborador
{
    public int IdColaborador { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public double? Segmento { get; set; }

    public int IdVendedor { get; set; }

    public virtual Vendedor IdVendedorNavigation { get; set; } = null!;
}

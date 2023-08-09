using System;
using Microsoft.Identity.Client;

namespace ML
{
	public class Cliente
	{
        public int IdCliente { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public int? Edad { get; set; }

        public string Telefono { get; set; } = null!;

        public string? Observaciones { get; set; }

        public ML.Vendedor? Vendedor { get; set; }

        public ML.Direccion? Direccion { get; set; }

        public ML.Contrato? Contrato { get; set; }

        public List<object>? Clientes { get; set; }
    }
}
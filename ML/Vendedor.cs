using System;
namespace ML
{
	public class Vendedor
	{
        public Vendedor()
        {

        }
        public int IdVendedor { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public string Curp { get; set; } = null!;

        public string? Rfc { get; set; }

        public string? Foto { get; set; }

        public string Email { get; set; } = null!;

        public string Celular { get; set; } = null!;

        public List<object>? Vendedores { get; set; }
    }
}


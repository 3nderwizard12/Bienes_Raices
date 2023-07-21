using System;
namespace ML
{
	public class Direccion
	{
        public int IdDireccion { get; set; }

        public string Calle { get; set; } = null!;

        public string? NumeroInterior { get; set; }

        public string Numeroexterior { get; set; } = null!;

        public ML.Cliente? Cliente { get; set; }

        public List<object>? Direcciones { get; set; }
    }
}


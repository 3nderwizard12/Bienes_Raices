using System;
namespace ML
{
	public class Ubicacion
	{
        public int IdUbicacion { get; set; }

        public string Desarrollo { get; set; } = null!;

        public string Manzana { get; set; } = null!;

        public string Lote { get; set; } = null!;

        public string NumeroContrato { get; set; } = null!;

        public ML.Estatus? Estatus { get; set; }

        public ML.Colaborador? Colaborador { get; set; }

        public List<object>? Ubicacaciones { get; set; }
    }
}
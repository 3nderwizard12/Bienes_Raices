using System;
namespace ML
{
	public class Colaborador
	{
        public int IdColaborador { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public double? Segmento { get; set; }

        public ML.Ubicacion? Ubicacion { get; set; }

        public List<object>? Usuarios { get; set; }
    }
}
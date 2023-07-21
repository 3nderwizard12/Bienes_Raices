using System;
namespace ML
{
	public class Costo
	{
        public int IdCosto { get; set; }

        public string? Letras { get; set; }

        public double CostoTotal { get; set; }

        public double TotalxMetro { get; set; }

        public double CostoxMetro { get; set; }

        public ML.Pago? Pago { get; set; }

        public List<object>? Costos { get; set; }
    }
}


using System;
namespace ML
{
	public class Pago
	{
        public int IdPago { get; set; }

        public double Enganche { get; set; }

        public string DiasPago { get; set; }

        public ML.MetodoPago? MetodoPago { get; set; }

        public double Intereses { get; set; }

        public double MensualidadMinima { get; set; }

        public List<object>? Pagos { get; set; }
    }
}


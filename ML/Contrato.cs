using System;
namespace ML
{
	public class Contrato
	{
        public string NumeroContrato { get; set; } = null!;

        public DateTime? FechaInicioContrato { get; set; }

        public DateTime? FechaFinContrato { get; set; }

        public ML.Cliente? Cliente { get; set; }

        public ML.Costo? Costo { get; set; }

        public ML.Pago? Pago { get; set; }

        public ML.EstatusContrato? EstatusContrato { get; set; }

        public ML.Ubicacion? Ubicacion { get; set; }

        public List<object>? NumeroContratos { get; set; }
    }
}


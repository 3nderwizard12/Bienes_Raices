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

    //Vendedor

    public string NombreVendedor { get; set; } = null!;

    //Direccion

    public int IdDireccion { get; set; }

    public string Calle { get; set; } = null!;

    public string? NumeroInterior { get; set; }

    public string Numeroexterior { get; set; } = null!;

    //Pago

    public double Enganche { get; set; }

    public DateTime DiasPago { get; set; }

    public double Intereses { get; set; }

    public double MensualidadMinima { get; set; }

    //Metodo pago

    public byte IdMetodoPago { get; set; }

    public string NombreMetodoPago { get; set; } = null!;

    //Costo

    public string? Letras { get; set; }

    public double CostoTotal { get; set; }

    public double TotalxMetro { get; set; }

    public double CostoxMetro { get; set; }

    public int IdPago { get; set; }

    //Contrato

    public string NumeroContrato { get; set; } = null!;

    public DateTime? FechaInicioContrato { get; set; }

    public DateTime? FechaFinContrato { get; set; }

    public int IdCosto { get; set; }

    //Estatus Contrato

    public byte IdEstatusContrato { get; set; }

    public string NombreEstatusContrato { get; set; } = null!;

    //Ubicacion

    public int IdUbicacion { get; set; }

    public string Desarrollo { get; set; } = null!;

    public string Manzana { get; set; } = null!;

    public string Lote { get; set; } = null!;

    //Estatus

    public byte IdEstatus { get; set; }

    public string NombreEstatus { get; set; } = null!;

    //Colaborador

    public int IdColaborador { get; set; }

    public string NombreColaborador { get; set; } = null!;

    public string ApellidoPaternoColaborador { get; set; } = null!;

    public string ApellidoMaternoColaborador { get; set; } = null!;

    public double? Segmento { get; set; }

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();

    public virtual Vendedor IdVendedorNavigation { get; set; } = null!;
}
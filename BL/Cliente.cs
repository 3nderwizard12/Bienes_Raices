using Microsoft.EntityFrameworkCore;

namespace BL
{
	public class Cliente
	{
        public static ML.Result Add(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"ClienteAdd" +
                        $" '{cliente.Nombre}', '{cliente.ApellidoPaterno}', '{cliente.ApellidoMaterno}', '{cliente.Edad}', '{cliente.Telefono}', '{cliente.Observaciones}', {cliente.Vendedor.IdVendedor}," +
                        $" '{cliente.Direccion.Calle}', '{cliente.Direccion.NumeroInterior}', '{cliente.Direccion.Numeroexterior}'," +
                        $" '{cliente.Contrato.Pago.Enganche}', '{cliente.Contrato.Pago.DiasPago}', {cliente.Contrato.Pago.MetodoPago.IdMetodoPago}, '{cliente.Contrato.Pago.MensualidadMinima}'," +
                        $" '{cliente.Contrato.Pago.Costo.Letras}', '{cliente.Contrato.Pago.Costo.CostoTotal}', '{cliente.Contrato.Pago.Costo.TotalxMetro}', '{cliente.Contrato.Pago.Costo.CostoxMetro}'," +
                        $" '{cliente.Contrato.NumeroContrato}', '{cliente.Contrato.FechaInicioContrato}', '{cliente.Contrato.FechaFinContrato}', {cliente.Contrato.EstatusContrato.IdEstatusContrato}," +
                        $" '{cliente.Contrato.Ubicacion.Desarrollo}', ''{cliente.Contrato.Ubicacion.Manzana}', '{cliente.Contrato.Ubicacion.Lote}', {cliente.Contrato.Ubicacion.Estatus.IdEstatus},");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error ocurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result Delete(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"ClienteDelete {cliente.IdCliente}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error ocurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result Update(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"ClienteUpdate" +
                        $" {cliente.IdCliente}, '{cliente.Nombre}', '{cliente.ApellidoPaterno}', '{cliente.ApellidoMaterno}', '{cliente.Edad}', '{cliente.Telefono}', '{cliente.Observaciones}', {cliente.Vendedor.IdVendedor}," +
                        $" '{cliente.Direccion.Calle}', '{cliente.Direccion.NumeroInterior}', '{cliente.Direccion.Numeroexterior}'," +
                        $" '{cliente.Contrato.Pago.Enganche}', '{cliente.Contrato.Pago.DiasPago}', {cliente.Contrato.Pago.MetodoPago.IdMetodoPago}, '{cliente.Contrato.Pago.MensualidadMinima}'," +
                        $" '{cliente.Contrato.Pago.Costo.Letras}', '{cliente.Contrato.Pago.Costo.CostoTotal}', '{cliente.Contrato.Pago.Costo.TotalxMetro}', '{cliente.Contrato.Pago.Costo.CostoxMetro}'," +
                        $" '{cliente.Contrato.NumeroContrato}', '{cliente.Contrato.FechaInicioContrato}', '{cliente.Contrato.FechaFinContrato}', {cliente.Contrato.EstatusContrato.IdEstatusContrato}," +
                        $" '{cliente.Contrato.Ubicacion.Desarrollo}', ''{cliente.Contrato.Ubicacion.Manzana}', '{cliente.Contrato.Ubicacion.Lote}', {cliente.Contrato.Ubicacion.Estatus.IdEstatus},");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error ocurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result GetAll(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    var query = cnn.Clientes.FromSqlRaw($"ClienteGetAll '{cliente.Nombre}', '{cliente.ApellidoPaterno}', '{cliente.ApellidoMaterno}'").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            cliente = new ML.Cliente();
                            cliente.IdCliente = row.IdCliente;
                            cliente.Nombre = row.Nombre;
                            cliente.ApellidoPaterno = row.ApellidoPaterno;
                            cliente.ApellidoMaterno = row.ApellidoMaterno;
                            cliente.Edad = row.Edad;
                            cliente.Telefono = row.Telefono;
                            cliente.Observaciones = row.Observaciones;

                            cliente.Vendedor = new ML.Vendedor();
                            cliente.Vendedor.IdVendedor = row.IdVendedor;
                            cliente.Vendedor.Nombre = row.NombreVendedor;

                            cliente.Direccion = new ML.Direccion();
                            cliente.Direccion.IdDireccion = row.IdDireccion;
                            cliente.Direccion.NumeroInterior = row.NumeroInterior;
                            cliente.Direccion.Numeroexterior = row.Numeroexterior;

                            cliente.Contrato = new ML.Contrato();
                            cliente.Contrato.NumeroContrato = row.NumeroContrato;
                            cliente.Contrato.FechaInicioContrato = row.FechaInicioContrato;
                            cliente.Contrato.FechaFinContrato = row.FechaFinContrato;

                            cliente.Contrato.EstatusContrato = new ML.EstatusContrato();
                            cliente.Contrato.EstatusContrato.IdEstatusContrato = row.IdEstatusContrato;
                            cliente.Contrato.EstatusContrato.Nombre = row.NombreEstatusContrato;

                            cliente.Contrato.Costo = new ML.Costo();
                            cliente.Contrato.Costo.IdCosto = row.IdCosto;
                            cliente.Contrato.Costo.Letras = row.Letras;
                            cliente.Contrato.Costo.CostoTotal = row.CostoTotal;
                            cliente.Contrato.Costo.TotalxMetro = row.TotalxMetro;
                            cliente.Contrato.Costo.CostoxMetro = row.CostoxMetro;

                            cliente.Contrato.Costo.Pago = new ML.Pago();
                            cliente.Contrato.Costo.Pago.IdPago = row.IdPago;
                            cliente.Contrato.Costo.Pago.Enganche = row.Enganche;
                            cliente.Contrato.Costo.Pago.DiasPago = row.DiasPago;
                            cliente.Contrato.Costo.Pago.Intereses = row.Intereses;
                            cliente.Contrato.Costo.Pago.MensualidadMinima = row.MensualidadMinima;

                            cliente.Contrato.Costo.Pago.MetodoPago = new ML.MetodoPago();
                            cliente.Contrato.Costo.Pago.MetodoPago.IdMetodoPago = row.IdMetodoPago;
                            cliente.Contrato.Costo.Pago.MetodoPago.Nombre = row.NombreMetodoPago;

                            cliente.Contrato.Ubicacion = new ML.Ubicacion();
                            cliente.Contrato.Ubicacion.IdUbicacion = row.IdUbicacion;
                            cliente.Contrato.Ubicacion.Desarrollo = row.Desarrollo;
                            cliente.Contrato.Ubicacion.Manzana = row.Manzana;
                            cliente.Contrato.Ubicacion.Lote = row.Lote;

                            cliente.Contrato.Ubicacion.Estatus = new ML.Estatus();
                            cliente.Contrato.Ubicacion.Estatus.IdEstatus = row.IdEstatus;
                            cliente.Contrato.Ubicacion.Estatus.Nombre = row.NombreEstatus;

                            cliente.Contrato.Ubicacion.Colaborador = new ML.Colaborador();
                            cliente.Contrato.Ubicacion.Colaborador.IdColaborador = row.IdColaborador;
                            cliente.Contrato.Ubicacion.Colaborador.Nombre = row.NombreColaborador;
                            cliente.Contrato.Ubicacion.Colaborador.ApellidoPaterno = row.ApellidoPaternoColaborador;
                            cliente.Contrato.Ubicacion.Colaborador.ApellidoMaterno = row.ApellidoMaternoColaborador;
                            cliente.Contrato.Ubicacion.Colaborador.Segmento = row.Segmento;

                            result.Objects.Add(cliente);
                        }

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error ocurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result GetById(int idCliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    var query = cnn.Clientes.FromSqlRaw($"ClienteById {idCliente}").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Cliente cliente = new ML.Cliente();
                        cliente.IdCliente = query.IdCliente;
                        cliente.Nombre = query.Nombre;
                        cliente.ApellidoPaterno = query.ApellidoPaterno;
                        cliente.ApellidoMaterno = query.ApellidoMaterno;
                        cliente.Edad = query.Edad;
                        cliente.Telefono = query.Telefono;
                        cliente.Observaciones = query.Observaciones;

                        cliente.Vendedor = new ML.Vendedor();
                        cliente.Vendedor.IdVendedor = query.IdVendedor;
                        cliente.Vendedor.Nombre = query.NombreVendedor;

                        cliente.Direccion = new ML.Direccion();
                        cliente.Direccion.IdDireccion = query.IdDireccion;
                        cliente.Direccion.NumeroInterior = query.NumeroInterior;
                        cliente.Direccion.Numeroexterior = query.Numeroexterior;

                        cliente.Contrato = new ML.Contrato();
                        cliente.Contrato.NumeroContrato = query.NumeroContrato;
                        cliente.Contrato.FechaInicioContrato = query.FechaInicioContrato;
                        cliente.Contrato.FechaFinContrato = query.FechaFinContrato;

                        cliente.Contrato.EstatusContrato = new ML.EstatusContrato();
                        cliente.Contrato.EstatusContrato.IdEstatusContrato = query.IdEstatusContrato;
                        cliente.Contrato.EstatusContrato.Nombre = query.NombreEstatusContrato;

                        cliente.Contrato.Costo = new ML.Costo();
                        cliente.Contrato.Costo.IdCosto = query.IdCosto;
                        cliente.Contrato.Costo.Letras = query.Letras;
                        cliente.Contrato.Costo.CostoTotal = query.CostoTotal;
                        cliente.Contrato.Costo.TotalxMetro = query.TotalxMetro;
                        cliente.Contrato.Costo.CostoxMetro = query.CostoxMetro;

                        cliente.Contrato.Costo.Pago = new ML.Pago();
                        cliente.Contrato.Costo.Pago.IdPago = query.IdPago;
                        cliente.Contrato.Costo.Pago.Enganche = query.Enganche;
                        cliente.Contrato.Costo.Pago.DiasPago = query.DiasPago;
                        cliente.Contrato.Costo.Pago.Intereses = query.Intereses;
                        cliente.Contrato.Costo.Pago.MensualidadMinima = query.MensualidadMinima;

                        cliente.Contrato.Costo.Pago.MetodoPago = new ML.MetodoPago();
                        cliente.Contrato.Costo.Pago.MetodoPago.IdMetodoPago = query.IdMetodoPago;
                        cliente.Contrato.Costo.Pago.MetodoPago.Nombre = query.NombreMetodoPago;

                        cliente.Contrato.Ubicacion = new ML.Ubicacion();
                        cliente.Contrato.Ubicacion.IdUbicacion = query.IdUbicacion;
                        cliente.Contrato.Ubicacion.Desarrollo = query.Desarrollo;
                        cliente.Contrato.Ubicacion.Manzana = query.Manzana;
                        cliente.Contrato.Ubicacion.Lote = query.Lote;

                        cliente.Contrato.Ubicacion.Estatus = new ML.Estatus();
                        cliente.Contrato.Ubicacion.Estatus.IdEstatus = query.IdEstatus;
                        cliente.Contrato.Ubicacion.Estatus.Nombre = query.NombreEstatus;

                        cliente.Contrato.Ubicacion.Colaborador = new ML.Colaborador();
                        cliente.Contrato.Ubicacion.Colaborador.IdColaborador = query.IdColaborador;
                        cliente.Contrato.Ubicacion.Colaborador.Nombre = query.NombreColaborador;
                        cliente.Contrato.Ubicacion.Colaborador.ApellidoPaterno = query.ApellidoPaternoColaborador;
                        cliente.Contrato.Ubicacion.Colaborador.ApellidoMaterno = query.ApellidoMaternoColaborador;
                        cliente.Contrato.Ubicacion.Colaborador.Segmento = query.Segmento;

                        result.Object = cliente;

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error ocurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }
    }
}
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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd");

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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioDelete ");

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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioUpdate ");

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
                    var query = cnn.Clientes.FromSqlRaw($"UsuarioGetAll").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            
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
                    var query = cnn.Clientes.FromSqlRaw($"UsuarioById {idCliente}").ToList().FirstOrDefault();

                    if (query != null)
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
    }
}
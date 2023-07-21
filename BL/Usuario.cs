using System;
using Microsoft.EntityFrameworkCore;

namespace BL
{
	public class Usuario
	{
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.vendedor.Nombre}', '{usuario.vendedor.ApellidoPaterno}', '{usuario.vendedor.ApellidoMaterno}', '{usuario.vendedor.Curp}', '{usuario.vendedor.Rfc}', '{usuario.vendedor.Foto}', '{usuario.vendedor.Email}', '{usuario.vendedor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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

        public static ML.Result Delete(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioDelete {usuario.vendedor.Vendedores}");

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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd {usuario.vendedor.IdVendedor}, '{usuario.vendedor.Nombre}', '{usuario.vendedor.ApellidoPaterno}', '{usuario.vendedor.ApellidoMaterno}', '{usuario.vendedor.Curp}', '{usuario.vendedor.Rfc}', '{usuario.vendedor.Foto}', '{usuario.vendedor.Email}', '{usuario.vendedor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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

        public static ML.Result CambiarEstatus(int idUsuario, bool estatus)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioEstatus {idUsuario}, {estatus}");

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

        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.vendedor.Nombre}', '{usuario.vendedor.ApellidoPaterno}', '{usuario.vendedor.ApellidoMaterno}'").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            usuario.vendedor = new ML.Vendedor();
                            usuario.vendedor.IdVendedor = row.IdVendedor;
                            usuario.vendedor.Nombre = row.Nombre;
                            usuario.vendedor.ApellidoPaterno = row.ApellidoPaterno;
                            usuario.vendedor.ApellidoMaterno = row.ApellidoMaterno;
                            usuario.vendedor.Curp = row.Curp;
                            usuario.vendedor.Rfc = row.Rfc;
                            usuario.vendedor.Foto = row.Foto;
                            usuario.vendedor.Email = row.Email;
                            usuario.vendedor.Celular = row.Celular;

                            usuario = new ML.Usuario();
                            usuario.IdUsuario = row.IdUsuario;
                            usuario.Username = row.Username;
                            usuario.Password = row.Password;
                            usuario.Estatus = row.Estatus.Value;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = row.IdRol;
                            usuario.Rol.Nombre = row.NombreRol;

                            result.Objects.Add(usuario);
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

        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();


            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioById {IdUsuario}").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.vendedor = new ML.Vendedor();
                        usuario.vendedor.IdVendedor = query.IdVendedor;
                        usuario.vendedor.Nombre = query.Nombre;
                        usuario.vendedor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.vendedor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.vendedor.Curp = query.Curp;
                        usuario.vendedor.Rfc = query.Rfc;
                        usuario.vendedor.Foto = query.Foto;
                        usuario.vendedor.Email = query.Email;
                        usuario.vendedor.Celular = query.Celular;

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Rol.Nombre = query.NombreRol;

                        result.Object = usuario;

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

        public static ML.Result GetByUsername(string username)
        {
            ML.Result result = new ML.Result();


            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioByUsername {username}").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.vendedor = new ML.Vendedor();
                        usuario.vendedor.IdVendedor = query.IdVendedor;
                        usuario.vendedor.Nombre = query.Nombre;
                        usuario.vendedor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.vendedor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.vendedor.Curp = query.Curp;
                        usuario.vendedor.Rfc = query.Rfc;
                        usuario.vendedor.Foto = query.Foto;
                        usuario.vendedor.Email = query.Email;
                        usuario.vendedor.Celular = query.Celular;

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Rol.Nombre = query.NombreRol;

                        result.Object = usuario;

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


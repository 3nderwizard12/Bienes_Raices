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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Vendor.Nombre}', '{usuario.Vendor.ApellidoPaterno}', '{usuario.Vendor.ApellidoMaterno}', '{usuario.Vendor.Curp}', '{usuario.Vendor.Rfc}', '{usuario.Vendor.Foto}', '{usuario.Vendor.Email}', '{usuario.Vendor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioDelete {usuario.Vendor.Vendedores}");

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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd {usuario.Vendor.IdVendedor}, '{usuario.Vendor.Nombre}', '{usuario.Vendor.ApellidoPaterno}', '{usuario.Vendor.ApellidoMaterno}', '{usuario.Vendor.Curp}', '{usuario.Vendor.Rfc}', '{usuario.Vendor.Foto}', '{usuario.Vendor.Email}', '{usuario.Vendor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Vendor.Nombre}', '{usuario.Vendor.ApellidoPaterno}', '{usuario.Vendor.ApellidoMaterno}'").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            usuario.Vendor = new ML.Vendedor();
                            usuario.Vendor.IdVendedor = row.IdVendedor;
                            usuario.Vendor.Nombre = row.Nombre;
                            usuario.Vendor.ApellidoPaterno = row.ApellidoPaterno;
                            usuario.Vendor.ApellidoMaterno = row.ApellidoMaterno;
                            usuario.Vendor.Curp = row.Curp;
                            usuario.Vendor.Rfc = row.Rfc;
                            usuario.Vendor.Foto = row.Foto;
                            usuario.Vendor.Email = row.Email;
                            usuario.Vendor.Celular = row.Celular;

                            usuario = new ML.Usuario();
                            usuario.IdUsuario = row.IdUsuario;
                            usuario.Username = row.Username;
                            usuario.Password = row.Password;
                            usuario.Estatus = row.Estatus;

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

                        usuario.Vendor = new ML.Vendedor();
                        usuario.Vendor.IdVendedor = query.IdVendedor;
                        usuario.Vendor.Nombre = query.Nombre;
                        usuario.Vendor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.Vendor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Vendor.Curp = query.Curp;
                        usuario.Vendor.Rfc = query.Rfc;
                        usuario.Vendor.Foto = query.Foto;
                        usuario.Vendor.Email = query.Email;
                        usuario.Vendor.Celular = query.Celular;

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus;

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

                        usuario.Vendor = new ML.Vendedor();
                        usuario.Vendor.IdVendedor = query.IdVendedor;
                        usuario.Vendor.Nombre = query.Nombre;
                        usuario.Vendor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.Vendor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Vendor.Curp = query.Curp;
                        usuario.Vendor.Rfc = query.Rfc;
                        usuario.Vendor.Foto = query.Foto;
                        usuario.Vendor.Email = query.Email;
                        usuario.Vendor.Celular = query.Celular;

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus;

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


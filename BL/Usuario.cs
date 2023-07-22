using System;
using System.Net.Http;
using DL;
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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Vendedor.Nombre}', '{usuario.Vendedor.ApellidoPaterno}', '{usuario.Vendedor.ApellidoMaterno}', '{usuario.Vendedor.Curp}', '{usuario.Vendedor.Rfc}', '{usuario.Vendedor.Foto}', '{usuario.Vendedor.Email}', '{usuario.Vendedor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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

            usuario.Vendedor = new ML.Vendedor();

            try
            {
                using (DL.BienesRaicesSqlContext cnn = new DL.BienesRaicesSqlContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioDelete {usuario.Vendedor.Vendedores}");

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
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.Vendedor.IdVendedor}, '{usuario.Vendedor.Nombre}', '{usuario.Vendedor.ApellidoPaterno}', '{usuario.Vendedor.ApellidoMaterno}', '{usuario.Vendedor.Curp}', '{usuario.Vendedor.Rfc}', '{usuario.Vendedor.Foto}', '{usuario.Vendedor.Email}', '{usuario.Vendedor.Celular}', '{usuario.Username}', '{usuario.Password}', {usuario.Rol.IdRol}");

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
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Vendedor.Nombre}', '{usuario.Vendedor.ApellidoPaterno}', '{usuario.Vendedor.ApellidoPaterno}'").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            usuario = new ML.Usuario();
                            usuario.IdUsuario = row.IdUsuario;
                            usuario.Username = row.Username;
                            usuario.Password = row.Password;
                            usuario.Estatus = row.Estatus.Value;

                            usuario.Vendedor = new ML.Vendedor();
                            usuario.Vendedor.IdVendedor = row.IdVendedor;
                            usuario.Vendedor.Nombre = row.Nombre;
                            usuario.Vendedor.ApellidoPaterno = row.ApellidoPaterno;
                            usuario.Vendedor.ApellidoMaterno = row.ApellidoMaterno;
                            usuario.Vendedor.Curp = row.Curp;
                            usuario.Vendedor.Rfc = row.Rfc;
                            usuario.Vendedor.Foto = row.Foto;
                            usuario.Vendedor.Email = row.Email;
                            usuario.Vendedor.Celular = row.Celular;

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

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Vendedor = new ML.Vendedor();
                        usuario.Vendedor.IdVendedor = query.IdVendedor;
                        usuario.Vendedor.Nombre = query.Nombre;
                        usuario.Vendedor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.Vendedor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Vendedor.Curp = query.Curp;
                        usuario.Vendedor.Rfc = query.Rfc;
                        usuario.Vendedor.Foto = query.Foto;
                        usuario.Vendedor.Email = query.Email;
                        usuario.Vendedor.Celular = query.Celular;

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

                        usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.Username;
                        usuario.Password = query.Password;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Vendedor = new ML.Vendedor();
                        usuario.Vendedor.IdVendedor = query.IdVendedor;
                        usuario.Vendedor.Nombre = query.Nombre;
                        usuario.Vendedor.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.Vendedor.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Vendedor.Curp = query.Curp;
                        usuario.Vendedor.Rfc = query.Rfc;
                        usuario.Vendedor.Foto = query.Foto;
                        usuario.Vendedor.Email = query.Email;
                        usuario.Vendedor.Celular = query.Celular;

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
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                 var query = context.Database.ExecuteSqlRaw($"AddUsuario '{usuario.Nombre}','{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}','{usuario.UserName}','{usuario.Email}',@Password", new SqlParameter("@Password", usuario.Password));
                   
                     if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        public static ML.Result GetByUserName(string userName)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    //var query = context.UsuarioGetByUserName(usuario.UserName).FirstOrDefault();
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByUserName {userName}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;
                        usuario.Email = query.Email;

                        result.Object = usuario;
                        result.Correct = true;
                    }

                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetByEmail(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    var obj = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").AsEnumerable().FirstOrDefault();
                    if (obj != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = obj.IdUsuario;
                        usuario.Nombre = obj.Nombre;
                        usuario.ApellidoPaterno = obj.ApellidoPaterno;
                        usuario.ApellidoMaterno = obj.ApellidoMaterno;
                        usuario.UserName = obj.UserName;
                        usuario.Password = obj.Password;
                        usuario.Email = obj.Email;

                        result.Object = usuario;
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo realizar la consulta";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    int RowsAffected = context.Database.ExecuteSqlRaw($"PasswordUpdate '{usuario.Email}',@Password", new SqlParameter("@Password", usuario.Password));

                    if (RowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

    }
}
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

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
                     var query = context.Database.ExecuteSqlRaw($"AddUsuario '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Email}', '{usuario.UserName}', @Password", new SqlParameter("@Password", usuario.Password));

                    //var query = context.AddUsuario(usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Email, usuario.UserName, usuario.Password);

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
        public static ML.Result GetByUserName(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    //var query = context.UsuarioGetByUserName(usuario.UserName).FirstOrDefault();
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByUserName {usuario.UserName}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {

                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;

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
                    //var obj = context.UsuarioGetByEmail(email).FirstOrDefault(); if (obj != null)
                    var obj = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail {email}").AsEnumerable().FirstOrDefault();
                    if (obj != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = obj.IdUsuario;
                        usuario.Nombre = obj.Nombre;
                        usuario.UserName = obj.UserName;
                        usuario.Email = obj.Email;
                        usuario.Password = obj.Password;

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
    }
}
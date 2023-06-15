using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace PL1.Controllers
{
    public class Usuario : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public Usuario(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Login()
        {
            ML.Usuario usuario = new ML.Usuario();
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Login(ML.Usuario usuario, string password1)
        {
            // Crear una instancia del algoritmo de hash bcrypt
            var bcrypt = new Rfc2898DeriveBytes(password1, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            if (usuario.Email != null)
            {
                // Insertar usuario en la base de datos
                usuario.Password = passwordHash;
                ML.Result result = BL.Usuario.Add(usuario);
                return View();
            }
            else
            {
                string userName = usuario.UserName;  
                // Proceso de login
                ML.Result result = BL.Usuario.GetByUserName(userName);
                usuario = (ML.Usuario)result.Object;

                if (usuario.Password.SequenceEqual(passwordHash))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult CambiarPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CambiarPassword(string email)
        {

            //validar que exista el email en la bd
            ML.Result result = BL.Usuario.GetByEmail(email);
            if (result.Correct)
            {
                string emailOrigen = _configuration["Correo"];

                MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
                mailMessage.IsBodyHtml = true;
                string contenidoHTML = System.IO.File.ReadAllText(@"C:\\Users\\Alien8\\Documents\\Liliana Gracida Brioso\\LGracidaUsuarioContraseña\\PL1\\Views\\Usuario\\email.cshtml");
                mailMessage.Body = contenidoHTML;
                string url = _configuration["Url"] + HttpUtility.UrlEncode(email);
                mailMessage.Body = mailMessage.Body.Replace("{URL}", url);

                SmtpClient smtpClient = new SmtpClient(_configuration["SmtpClient"]);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen,_configuration["Password"] );

                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                ViewBag.Modal = "show";
                ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            }
            else
            {

                ViewBag.Mensaje = "El Email no existe";
            }

           
            return View();
        }
        [HttpGet]
        public ActionResult NewPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewPassword(string password, string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;

            // Crear una instancia del algoritmo de hash bcrypt
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);
            usuario.Password = passwordHash;
            ML.Result result = BL.Usuario.Update(usuario);
            if (result.Correct)
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            {

            }
            return View();
        }
    }
}

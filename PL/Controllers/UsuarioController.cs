using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNetCore.StaticFiles;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IHostingEnvironment environment, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario resultUsuario = new ML.Usuario();
            resultUsuario.Vendedor = new ML.Vendedor();

            resultUsuario.Usuarios = new List<object>();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];

                string requestUri = $"Usuario/GetAll";

                var responseTask = client.GetAsync(new Uri(new Uri(urlApi), requestUri));
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Usuario ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                        resultUsuario.Usuarios.Add(ResultItemList);
                    }
                }
            }
            return View(resultUsuario);
        }

        [HttpPost]
        public ActionResult GetAllWebAPI(ML.Usuario usuario)
        {
            usuario.Usuarios = new List<object>();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];

                string nombre = usuario?.Vendedor?.Nombre;
                string apellidoPaterno = usuario?.Vendedor?.ApellidoPaterno;
                string apellidoMaterno = usuario?.Vendedor?.ApellidoMaterno;

                string requestUri = $"Usuario/GetAny?nombre={Uri.EscapeDataString(nombre ?? string.Empty)}&apellidoPaterno={Uri.EscapeDataString(apellidoPaterno ?? string.Empty)}&apellidoMaterno={Uri.EscapeDataString(apellidoMaterno ?? string.Empty)}";

                var responseTask = client.GetAsync(new Uri(new Uri(urlApi), requestUri));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Usuario ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                        usuario.Usuarios.Add(ResultItemList);
                    }
                }
            }
            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta Users";
            }

            return View(usuario);
        }

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Result resultRol = BL.Rol.GetAll();

            ML.Usuario usuario = new ML.Usuario { Vendedor = new ML.Vendedor(), Rol = new ML.Rol() };

            if (resultRol.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;
            }
            if (idUsuario == null)
            {
                return View(usuario);
            }
            else
            {
                ML.Result result = new ML.Result();
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlWebApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Usuario/GetById/" + idUsuario);
                    responseTask.Wait();

                    var resultAPI = responseTask.Result;

                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        usuario = (ML.Usuario)result.Object;

                        usuario.Rol.Roles = resultRol.Objects;
                    }
                }
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<ActionResult> FrmAsync(ML.Usuario usuario, string password)
        {
            IFormFile file = Request.Form.Files["inpImagen"];

            if (file != null)
            {
                usuario.Vendedor.Foto = Convert.ToBase64String(await ConvertToBytesAsync(file));
            }

            if (!string.IsNullOrEmpty(password))
            {
                var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
                usuario.Password = bcrypt.GetBytes(20);
            }

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];
                client.BaseAddress = new Uri(urlApi);

                HttpResponseMessage result;

                if (usuario.IdUsuario == 0)
                {
                    result = await client.PostAsJsonAsync("Usuario/Add", usuario);
                }
                else
                {
                    result = await client.PutAsJsonAsync($"Usuario/Update/{usuario.IdUsuario}", usuario);
                }

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Registro correctamente " + (usuario.IdUsuario == 0 ? "insertado" : "actualizado");
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al realizar la operación";
                }

                return PartialView("Modal");
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario, string password)
        {
            if (ModelState.IsValid)
            {
                IFormFile file = Request.Form.Files["inpImagen"];

                if (file != null)
                {
                    usuario.Vendedor.Foto = Convert.ToBase64String(ConvertToBytes(file));
                }

                if (!string.IsNullOrEmpty(password))
                {
                    var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
                    usuario.Password = bcrypt.GetBytes(20);
                }

                ML.Result result;

                if (usuario.IdUsuario == 0)
                {
                    if (usuario.Password != null)
                    {
                        usuario.Estatus = true;
                    }
                    else
                    {
                        usuario.Estatus = false;
                    }
                    // Add
                    result = BL.Usuario.Add(usuario);
                }
                else
                {
                    if (usuario.Password != null)
                    {
                        usuario.Estatus = true;
                    }
                    else
                    {
                        usuario.Estatus = false;
                    }
                    // Update
                    result = BL.Usuario.Update(usuario);
                }

                if (result.Correct)
                {
                    ViewBag.Message = usuario.IdUsuario == 0 ? "Registro correctamente insertado" : "Registro correctamente actualizado";
                }
                else
                {
                    ViewBag.Message = usuario.IdUsuario == 0 ? "Ocurrio un error al insertar el registro" : "Ocurrio un error al actualizar el registro";
                }

                return View("Modal");
            }
            else
            {
                ML.Result resultRol = BL.Rol.GetAll();

                usuario = new ML.Usuario { Vendedor = new ML.Vendedor(), Rol = new ML.Rol() };

                usuario.Rol.Roles = resultRol.Objects;

                return View(usuario);
            }
        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];
                client.BaseAddress = new Uri(urlApi);

                var postTask = client.GetAsync("Usuario/Delete/" + idUsuario);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Registro correctamente Eliminado";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al eliminar el registro";
                    return PartialView("Modal");
                }
            }
        }

        [HttpPost]
        private JsonResult CambiarEstatus(int idUsuario, bool status)
        {
            ML.Result result = BL.Usuario.CambiarEstatus(idUsuario, status);

            return Json(result);
        }

        private async Task<byte[]> ConvertToBytesAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public IActionResult Login()
        {
            ML.Usuario usuario = new ML.Usuario();
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Login(ML.Usuario usuario, string password)
        {
            ML.Result resultUsuario = BL.Usuario.GetByUsername(usuario.Username);

            if (!resultUsuario.Correct)
            {
                ViewBag.Modal = "show";
                ViewBag.Message = "El Usuario no existe";
                return View();
            }

            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            ML.Usuario fetchedUsuario = new ML.Usuario();

            fetchedUsuario = (ML.Usuario)resultUsuario.Object;

            if (!fetchedUsuario.Estatus)
            {
                // Insertar usuario en la base de datos
                usuario.Password = passwordHash;
                usuario.Estatus = true;

                ML.Result result = BL.Usuario.Password(usuario);
                return View();
            }
            else
            {

                if (!fetchedUsuario.Password.SequenceEqual(passwordHash))
                {
                    ViewBag.Modal = "show";
                    ViewBag.Message = "La contraseña no coincide";
                    return View();
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("Id", fetchedUsuario.IdUsuario);
                    _httpContextAccessor.HttpContext.Session.SetString("Username", fetchedUsuario.Username);
                    _httpContextAccessor.HttpContext.Session.SetString("Rol", fetchedUsuario.Rol.Nombre);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public ActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarPassword(string email)
        {
            ML.Result result = BL.Usuario.FindByEmail(email);
            if (result.Correct)
            {
                string emailOrigen = _configuration["emailOrigen"];

                MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
                mailMessage.IsBodyHtml = true;
                //string contenidoHTML = System.IO.File.ReadAllText(configuration["contenidoHTML"]);
                string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.cshtml"));
                mailMessage.Body = contenidoHTML;
                string url = _configuration["url"] + HttpUtility.UrlEncode(email);
                mailMessage.Body = mailMessage.Body.Replace("{link}", url);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, _configuration["appPassword"]);

                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                ViewBag.Modal = "show";
                ViewBag.Message = "Se ha enviado un correo de confirmación a tu correo electronico";
                return View();
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Mensaje = "El correo es incorrecto";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Email()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewPassword(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Vendedor = new ML.Vendedor();
            usuario.Vendedor.Email = email;

            return View(usuario);
        }

        [HttpPost]
        public ActionResult NewPassword(ML.Usuario usuario, string password)
        {
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);

            var passwordHash = bcrypt.GetBytes(20);
            usuario.Password = passwordHash;

            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                ViewBag.Modal = "show";
                ViewBag.Message = "Se ha actualizado correctamente";
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Mensaje = "Error al actualizar la contraseña";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
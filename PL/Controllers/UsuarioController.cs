using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Security.Cryptography;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private IHostingEnvironment environment;
        private IConfiguration configuration;

        public UsuarioController(IHostingEnvironment _environment, IConfiguration _configuration)
        {
            environment = _environment;
            configuration = _configuration;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario resultUsuario = new ML.Usuario();
            resultUsuario.Vendedor = new ML.Vendedor();

            resultUsuario.Usuarios = new List<object>();

            using (var client = new HttpClient())
            {
                string urlApi = configuration["urlWebApi"];

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
                string urlApi = configuration["urlWebApi"];

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

            ML.Usuario usuario = new ML.Usuario();
            usuario.Vendedor = new ML.Vendedor();
            
            usuario.Rol = new ML.Rol();

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
                    string urlApi = configuration["urlWebApi"];
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
        public ActionResult Form(ML.Usuario usuario, string password)
        {
            IFormFile file = Request.Form.Files["inpImagen"];

            if (file != null)
            {
                usuario.Vendedor.Foto = Convert.ToBase64String(ConvertToBytes(file));
            }

            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            usuario.Password = passwordHash;

            if (usuario.IdUsuario == 0)
            {
                //add
                using (var client = new HttpClient())
                {
                    string urlApi = configuration["urlWebApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add/", usuario);
                    
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Registro correctamente insertado";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al Insertar el registro";
                        return PartialView("Modal");
                    }
                }
            }
            else
            {
                //update
                using (var client = new HttpClient())
                {
                    string urlApi = configuration["urlWebApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var postTask = client.PutAsJsonAsync<ML.Usuario>("Usuario/Update/" + usuario.IdUsuario, usuario);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Registro correctamente actualizado";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al actualizar el actualizado";
                        return PartialView("Modal");
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            using (var client = new HttpClient())
            {
                string urlApi = configuration["urlWebApi"];
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
        public JsonResult CambiarStatus(int idUsuario, bool status)
        {

            ML.Result result = BL.Usuario.CambiarEstatus(idUsuario, status);

            return Json(result);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    ML.Result result = BL.Usuario.GetByUsername(username);

        //    if (result.Correct)
        //    {
        //        ML.Usuario usuario = (ML.Usuario)result.Object;
        //        if (password == usuario.Password)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Contraseña Invalida";
        //            return PartialView("ModalLogin");
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Usuario Invalido";
        //        return PartialView("ModalLogin");
        //    }
        //}
    }
}


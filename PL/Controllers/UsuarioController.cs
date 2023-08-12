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
                string urlApi = configuration["urlWebApi"];
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
                // Add
                result = BL.Usuario.Add(usuario);
            }
            else
            {
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
        private JsonResult CambiarStatus(int idUsuario, bool status)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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
            resultUsuario.Usuarios = new List<object>();

            using (var client = new HttpClient())
            {
                string urlApi = configuration["urlWebApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync("Usuario/GetAll");
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
        public ActionResult GetAll(ML.Usuario resultUsuario)
        {
            resultUsuario.Usuarios = new List<object>();
            resultUsuario.vendedor = new ML.Vendedor();

            using (var client = new HttpClient())
            {
                string urlApi = configuration["urlWebApi"];
                client.BaseAddress = new Uri(urlApi);

                var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/GetAll/", resultUsuario);
                postTask.Wait();

                var result = postTask.Result;

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

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Result resultRol = BL.Rol.GetAll();

            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            if (resultRol.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;;
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
        public ActionResult Form(ML.Usuario usuario)
        {
            usuario.vendedor = new ML.Vendedor();

            IFormFile file = Request.Form.Files["inpImagen"];

            if (file != null)
            {
                usuario.vendedor.Foto = Convert.ToBase64String(ConvertToBytes(file));
            }

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
                        ViewBag.Message = "Ocurrio un error al actualizar el registro";
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
        public JsonResult CambiarStatus(int idAlumno, bool status)
        {

            ML.Result result = BL.Usuario.CambiarEstatus(idAlumno, status);

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

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            ML.Result result = BL.Usuario.GetByUsername(username);

            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (password == usuario.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Contraseña Invalida";
                    return PartialView("ModalLogin");
                }
            }
            else
            {
                ViewBag.Message = "Usuario Invalido";
                return PartialView("ModalLogin");
            }
        }
    }
}


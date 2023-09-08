using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class ClienteController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [Obsolete]
        public ClienteController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IHostingEnvironment environment, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Cliente resultCliente = new ML.Cliente();

            resultCliente.Clientes = new List<object>();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];

                string requestUri = $"Cliente/GetAll";

                var responseTask = client.GetAsync(new Uri(new Uri(urlApi), requestUri));
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Cliente ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cliente>(resultItem.ToString());
                        resultCliente.Clientes.Add(ResultItemList);
                    }
                }
            }
            return View(resultCliente);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Cliente cliente)
        {
            ML.Result result = BL.Cliente.GetAll(cliente);

            if (result.Correct)
            {
                cliente.Clientes = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta Users";
            }

            return View(cliente);
        }

        [HttpGet]
        public ActionResult Form(int? idCliente)
        {
            ML.Result resultMetodoPago = BL.MetodoPago.GetAll();
            ML.Result resultEstatusContrato = BL.Estatus_Contrato.GetAll();
            ML.Result resultEstatus = BL.Estatus.GetAll();

            ML.Cliente cliente = new ML.Cliente
            {
                Vendedor = new ML.Vendedor(),
                Direccion = new ML.Direccion(),
                Contrato = new ML.Contrato
                {
                    EstatusContrato = new ML.EstatusContrato(),
                    Costo = new ML.Costo
                    {
                        Pago = new ML.Pago
                        {
                            MetodoPago = new ML.MetodoPago()
                        }
                    },
                    Ubicacion = new ML.Ubicacion
                    {
                        Estatus = new ML.Estatus(),
                        Colaborador = new ML.Colaborador()
                    }
                }
            };

            if (resultMetodoPago.Correct && resultEstatusContrato.Correct && resultEstatus.Correct)
            {
                cliente.Contrato.Costo.Pago.MetodoPago.MetodosPago = resultMetodoPago.Objects;
                cliente.Contrato.EstatusContrato.EstatusContratos = resultEstatusContrato.Objects;
                cliente.Contrato.Ubicacion.Estatus.Estatuses = resultEstatus.Objects;
            }
            if (idCliente == null)
            {
                return View(cliente);
            }
            else
            {
                ML.Result result = new ML.Result();
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlWebApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Usuario/GetById/" + idCliente);
                    responseTask.Wait();

                    var resultAPI = responseTask.Result;

                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Cliente resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cliente>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        cliente = (ML.Cliente)result.Object;

                        cliente.Contrato.Costo.Pago.MetodoPago.MetodosPago = resultMetodoPago.Objects;
                        cliente.Contrato.EstatusContrato.EstatusContratos = resultEstatusContrato.Objects;
                        cliente.Contrato.Ubicacion.Estatus.Estatuses = resultEstatus.Objects;
                    }
                }
                return View(cliente);
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Cliente cliente)
        {
            int idVendedor = (int)_httpContextAccessor.HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                
                cliente.Vendedor.IdVendedor = idVendedor;
                if (cliente.IdCliente == 0)
                {
                    //add
                    using (var client = new HttpClient())
                    {
                        string urlApi = _configuration["urlWebApi"];
                        client.BaseAddress = new Uri(urlApi);

                        var postTask = client.PostAsJsonAsync<ML.Cliente>("Cliente/Add/", cliente);

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
                        string urlApi = _configuration["urlWebApi"];
                        client.BaseAddress = new Uri(urlApi);

                        var postTask = client.PutAsJsonAsync<ML.Cliente>("Cliente/Update/" + cliente.IdCliente, cliente);
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
            else
            {
                ML.Result resultMetodoPago = BL.MetodoPago.GetAll();
                ML.Result resultEstatusContrato = BL.Estatus_Contrato.GetAll();
                ML.Result resultEstatus = BL.Estatus.GetAll();

                cliente = new ML.Cliente
                {
                    Vendedor = new ML.Vendedor(),
                    Direccion = new ML.Direccion(),
                    Contrato = new ML.Contrato
                    {
                        EstatusContrato = new ML.EstatusContrato(),
                        Costo = new ML.Costo
                        {
                            Pago = new ML.Pago
                            {
                                MetodoPago = new ML.MetodoPago()
                            }
                        },
                        Ubicacion = new ML.Ubicacion
                        {
                            Estatus = new ML.Estatus(),
                            Colaborador = new ML.Colaborador()
                        }
                    }
                };

                cliente.Contrato.Costo.Pago.MetodoPago.MetodosPago = resultMetodoPago.Objects;
                cliente.Contrato.EstatusContrato.EstatusContratos = resultEstatusContrato.Objects;
                cliente.Contrato.Ubicacion.Estatus.Estatuses = resultEstatus.Objects;

                return View(cliente);
            }
        }

        [HttpGet]
        public ActionResult Delete(int idCliente)
        {
            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlWebApi"];
                client.BaseAddress = new Uri(urlApi);

                var postTask = client.GetAsync("Cliente/Delete/" + idCliente);
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
    }
}
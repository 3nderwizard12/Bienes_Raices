using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace SL.Controllers
{
    public class UsuarioController : Controller
    {
        [Route("api/Usuario/GetAll/{usuario}")]
        [HttpGet]
        public IActionResult GetAll(ML.Usuario usuario)
        {
            usuario.Vendedor = new ML.Vendedor();

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else { return NotFound(result); }
        }

        [Route("api/Usuario/Add")]
        [HttpPost]
        public IActionResult Add([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else { return NotFound(result); }
        }

        [Route("api/Usuario/Delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.IdUsuario = id;

            ML.Result result = BL.Usuario.Delete(usuario);

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else { return NotFound(result); }
        }

        [Route("api/Usuario/Update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else { return NotFound(result); }
        }

        [Route("api/Usuario/GetById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            ML.Result result = BL.Usuario.GetById(id);

            if (result.Correct)
            {
                return Ok(result);
            }
            else { return NotFound(); }
        }

        [Route("api/Usuario/GetByUserName/{userName},{password}")]
        [HttpGet]
        public IActionResult GetByUserName(string username, string password)
        {
            ML.Result result = BL.Usuario.GetByUsername(username);

            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (password == usuario.Password)
                {
                    return Ok(result.Object);
                }
                else { return NotFound(); }
            }
            else { return NotFound(); }
        }
    }
}


using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Vendedor = new ML.Vendedor();

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else { return NotFound(result); }
        }

        [HttpGet("GetAny")]
        public IActionResult GetAll([FromQuery] string nombre, [FromQuery] string apellidoPaterno, [FromQuery] string apellidoMaterno)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Vendedor = new ML.Vendedor();

            usuario.Vendedor.Nombre = nombre;
            usuario.Vendedor.ApellidoPaterno = apellidoPaterno;
            usuario.Vendedor.ApellidoMaterno = apellidoMaterno;

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else { return NotFound(result); }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else { return NotFound(result); }
        }

        [HttpGet("Delete/{id}")]
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

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else { return NotFound(result); }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            ML.Result result = BL.Usuario.GetById(id);

            if (result.Correct)
            {
                return Ok(result);
            }
            else { return NotFound(); }
        }

        //[HttpGet("GetByUserName/{userName},{password}")]
        //public IActionResult GetByUserName(string username, string password)
        //{
        //    ML.Result result = BL.Usuario.GetByUsername(username);

        //    if (result.Correct)
        //    {
        //        ML.Usuario usuario = (ML.Usuario)result.Object;
        //        if (password == usuario.Password)
        //        {
        //            return Ok(result.Object);
        //        }
        //        else { return NotFound(); }
        //    }
        //    else { return NotFound(); }
        //}
    }
}


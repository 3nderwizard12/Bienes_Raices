using System.ComponentModel.DataAnnotations;

namespace ML
{
	public class Usuario
	{
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo Username no puede estar vacio")]
        [StringLength(50, ErrorMessage = "El Username no puede ser menor a 8 ni mayor a 20", MinimumLength = 8)]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\d-_\\.]+$", ErrorMessage = "Sin espacion")]
        public string Username { get; set; } = null!;

        //[StringLength(50, ErrorMessage = "El Password no puede ser menor a 10 ni mayor a 50", MinimumLength = 8)]
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,50}$", ErrorMessage = "At least one uppercase \r\n At least one lowercase \r\n At least one digit \r\n At least one special character,")]
        public byte[] Password { get; set; } = null!;

        public bool Estatus { get; set; }

        public ML.Vendedor? Vendedor { get; set; }

        public ML.Rol? Rol { get; set; }

        public List<object>? Usuarios { get; set; }
    }
}


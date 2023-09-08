using System.ComponentModel.DataAnnotations;

namespace ML
{
	public class Colaborador
	{
        public int IdColaborador { get; set; }

        [Required(ErrorMessage = "El campo Nombre no puede estar vacio")]
        [StringLength(50, ErrorMessage = "El Nombre no puede ser mayor a 50")]
        [RegularExpression("^[a-zA-Z\\s\\.]{3,50}$", ErrorMessage = "Solo letras")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo Apellido Paterno no puede estar vacio")]
        [StringLength(50, ErrorMessage = "El Nombre no puede ser mayor a 50")]
        [RegularExpression("^[a-zA-Z_-]{3,50}$", ErrorMessage = "sin espacion y solo letras")]
        public string ApellidoPaterno { get; set; } = null!;

        [Required(ErrorMessage = "El campo Apellido Materno no puede estar vacio")]
        [StringLength(50, ErrorMessage = "El Nombre no puede ser mayor a 50")]
        [RegularExpression("^[a-zA-Z_-]{3,50}$", ErrorMessage = "sin espacion y solo letras")]
        public string ApellidoMaterno { get; set; } = null!;

        public double? Segmento { get; set; }

        public ML.Ubicacion? Ubicacion { get; set; }

        public List<object>? Usuarios { get; set; }
    }
}
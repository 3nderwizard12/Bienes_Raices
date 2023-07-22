using System;
namespace ML
{
	public class Rol
	{
        public Rol()
        {

        }
        public byte IdRol { get; set; }

        public string Nombre { get; set; } = null!;

        public List<object>? Roles { get; set; }
    }
}


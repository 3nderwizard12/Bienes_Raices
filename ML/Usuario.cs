﻿using System;
namespace ML
{
	public class Usuario
	{
        public int IdUsuario { get; set; }

        public string Username { get; set; } = null!;

        public byte[] Password { get; set; } = null!;

        public bool Estatus { get; set; }

        public ML.Vendedor? Vendedor { get; set; }

        public ML.Rol? Rol { get; set; }

        public List<object>? Usuarios { get; set; }
    }
}


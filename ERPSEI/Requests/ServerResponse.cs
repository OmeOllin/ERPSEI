namespace ERPSEI.Requests
{
	public class ServerResponse
	{
		public bool TieneError { get; set; }
		public string? Mensaje { get; set; }
		public object? Datos { get; set; }
		public string[] Errores { get; set; } = Array.Empty<string>();

		public ServerResponse() { 
			TieneError = false;
			Mensaje = null;
			Datos = null;
		}

		public ServerResponse(bool error, string? mensaje)
		{
			TieneError = error;
			Mensaje = mensaje;
			Datos = null;
		}

		public ServerResponse(bool error, string? mensaje, object? datos)
		{
			TieneError = error;
			Mensaje = mensaje;
			Datos = datos;
		}
	}
}

namespace ERPSEI.Requests
{
	public class ServerResponse
	{
		public bool Error { get; set; }
		public string? Mensaje { get; set; }
		public object? Datos { get; set; }

		public ServerResponse() { 
			Error = false;
			Mensaje = null;
			Datos = null;
		}

		public ServerResponse(bool error, string? mensaje)
		{
			Error = error;
			Mensaje = mensaje;
			Datos = null;
		}

		public ServerResponse(bool error, string? mensaje, object? datos)
		{
			Error = error;
			Mensaje = mensaje;
			Datos = datos;
		}
	}
}

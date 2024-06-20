using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
	public class Asistencia 
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)] 
		public string Id { get; set; } = string.Empty;
		public DateTime FechaHora { get; set; }
		public DateOnly Fecha { get; set; }
		public TimeSpan Hora { get; set; }
		public string Direccion { get; set; } = string.Empty;
		public string NombreDispositivo { get; set; } = string.Empty;
		public string SerialDispositivo { get; set; } = string.Empty;
		public string NombreEmpleado { get; set; } = string.Empty;
		public string NoTarjeta { get; set; } = string.Empty;
	}
}

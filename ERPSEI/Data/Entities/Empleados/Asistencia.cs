using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
	public class Asistencia 
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)] 
		public string Id { get; set; } = string.Empty;
		public string Horario { get; set; } = string.Empty;
		public string NombreEmpleado { get; set; } = string.Empty;
		public DateOnly Fecha { get; set; }
		public string Dia { get; set; } = string.Empty;
		public TimeSpan Entrada { get; set; }
		public string ResultadoE { get; set; } = string.Empty;
		public TimeSpan Salida { get; set; }
		public string ResultadoS { get; set; } = string.Empty;
		
	}
}

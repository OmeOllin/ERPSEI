using ERPSEI.Data.Entities.Empleados;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Reportes
{
	public class Asistencia
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public Empleado? Empleado { get; set; }
		public int? EmpleadoId { get; set; }

		public DateOnly? Fecha { get; set; }
		public string? Dia { get; set; }
		public TimeSpan Entrada { get; set; }
		public string? ResultadoE { get; set; }
		public TimeSpan? Salida { get; set; }
		public string? ResultadoS { get; set; }

	}
}

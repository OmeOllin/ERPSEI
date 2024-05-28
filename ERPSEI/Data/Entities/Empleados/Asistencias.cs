using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
	public class Asistencias
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int? Id { get; set; }
		public string? Nombre { get; set; } = string.Empty;
		public DateOnly? Fecha { get; set; }
		public TimeOnly? HoraEntrada { get; set; }
		public TimeOnly? HoraSalida { get; set; }
		public int? Retardo { get; set; }
		public int? Total { get; set; }
		public int? Faltas { get; set; }
	}
}

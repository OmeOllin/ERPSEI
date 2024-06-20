using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
	public class Asistencia 
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		public string? Nombre { get; set; } = string.Empty;
		public DateOnly? Fecha { get; set; }
		public TimeOnly? HoraEntrada { get; set; }
		public TimeOnly? HoraSalida { get; set; }
		public int? Retardo { get; set; }
		public int? Total { get; set; }
		public int? Faltas { get; set; }

		//Relaciones de la entidad
		public int? EmpleadoId { get; set; }
		public Empleado? Empleado { get; set; }

		//[NotMapped]
		//public List<Empleado>? Empleados { get; set; }
		//public ICollection<Empleado>? Empleados { get; set; }
	}
}

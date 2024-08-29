using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Reportes
{
	public class HorarioDetalle
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public int HorarioId { get; set; }
		public Horario? Horario { get; set; }

		public int NumeroDiaSemana { get; set; }
		public TimeSpan Entrada { get; set; }
		public TimeSpan ToleranciaEntrada { get; set; }
		public TimeSpan ToleranciaFalta { get; set; }
		public TimeSpan Salida { get; set; }
		public TimeSpan ToleranciaSalida { get; set; }
		public bool Activado { get; set; } = false;
	}
}

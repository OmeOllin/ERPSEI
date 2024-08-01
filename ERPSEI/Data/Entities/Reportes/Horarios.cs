using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Reportes
{
    public class Horarios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string NombreHorario { get; set; } = string.Empty;
        public TimeSpan Entrada { get; set; }
        public TimeSpan ToleranciaEntrada { get; set; }
        public TimeSpan ToleranciaFalta { get; set; }
        public TimeSpan Salida { get; set; }
        public TimeSpan ToleranciaSalida { get; set; }
		public ICollection<Asistencia>? Asistencias { get; }
	}
}

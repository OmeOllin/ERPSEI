using ERPSEI.Data.Entities.Empleados;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Reportes
{
    public class Horario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;

		public ICollection<HorarioDetalle>? HorarioDetalles { get; }

        public ICollection<Empleado>? Empleados { get; }

        public int? Deshabilitado { get; set; } = 0;
	}
}

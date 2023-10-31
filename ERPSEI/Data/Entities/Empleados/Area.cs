using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
    public class Area
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public ICollection<Subarea>? Subareas { get; set; }

		public ICollection<Empleado>? Empleados { get; set; }
	}
}

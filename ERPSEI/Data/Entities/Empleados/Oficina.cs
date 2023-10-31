using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
    public class Oficina
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

		public ICollection<Empleado>? Empleados { get; set; }
	}
}

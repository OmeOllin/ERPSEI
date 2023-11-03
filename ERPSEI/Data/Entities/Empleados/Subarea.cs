using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
    public class Subarea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int? AreaId {  get; set; }
        public Area? Area { get; set; }

		public ICollection<Empleado>? Empleados { get; set; }
	}
}

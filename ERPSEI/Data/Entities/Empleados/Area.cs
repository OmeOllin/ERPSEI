namespace ERPSEI.Data.Entities.Empleados
{
    public class Area
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

		public ICollection<Empleado>? Empleados { get; set; }
	}
}

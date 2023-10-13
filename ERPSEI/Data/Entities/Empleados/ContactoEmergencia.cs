namespace ERPSEI.Data.Entities.Empleados
{
    public class ContactoEmergencia
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public int EmpleadoId {  get; set; }
        public Empleado? Empleado { get; set; }
    }
}

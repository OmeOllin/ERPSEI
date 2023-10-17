namespace ERPSEI.Data.Entities.Empleados
{
    public class ArchivoEmpleado
    {
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Archivo { get; set; } = new byte[0];

        public int? TipoArchivoId { get; set; }
        public TipoArchivo? TipoArchivo { get; set; }

        public int? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

    }
}

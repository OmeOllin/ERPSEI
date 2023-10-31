namespace ERPSEI.Data.Entities.Empleados
{
    public enum FileTypes
    {
        ImagenPerfil,
        ActaNacimiento,
        CURP,
        CLABE,
        ComprobanteDomicilio,
        ContactosEmergencia,
        CSF,
        INE,
        RFC,
        ComprobanteEstudios,
        NSS,
        Otro
    }
    public class TipoArchivo
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ICollection<ArchivoEmpleado>? ArchivosEmpleado { get; }

        public TipoArchivo(int id, string description)
        {
            Id = id;
            Description = description;
        }

    }
}

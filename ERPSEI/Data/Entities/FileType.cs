namespace ERPSEI.Data.Entities
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
        NSS
    }
    public class FileType
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ICollection<UserFile>? UserFiles { get; }

        public FileType(int id, string description)
        {
            Id = id;
            Description = description;
        }

    }
}

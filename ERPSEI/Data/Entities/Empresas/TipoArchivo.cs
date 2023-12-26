namespace ERPSEI.Data.Entities.Empresas
{
    public enum FileTypes
    {
        ImagenPerfil = 1,
        CSF,
        RFC
    }
    public class TipoArchivoEmpresa
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ICollection<ArchivoEmpresa>? ArchivosEmpresa { get; }

        public TipoArchivoEmpresa(int id, string description)
        {
            Id = id;
            Description = description;
        }

    }
}

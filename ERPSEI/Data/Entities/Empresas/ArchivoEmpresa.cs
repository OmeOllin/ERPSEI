namespace ERPSEI.Data.Entities.Empresas
{
    public class ArchivoEmpresa
    {
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Archivo { get; set; } = new byte[0];

        public int? TipoArchivoId { get; set; }
        public TipoArchivoEmpresa? TipoArchivo { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}

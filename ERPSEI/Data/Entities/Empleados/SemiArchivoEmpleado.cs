using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
    [NotMapped]
    public class SemiArchivoEmpleado
    {
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Archivo { get; set; } = new byte[0];

        public long FileSize { get; set; }

        public int? TipoArchivoId { get; set; }

        public int? EmpleadoId { get; set; }
    }
}

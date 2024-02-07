using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
    [NotMapped]
    public class ProfilePicture
    {
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Archivo { get; set; } = new byte[0];
    }
}

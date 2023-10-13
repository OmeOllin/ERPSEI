namespace ERPSEI.Data.Entities
{
	public class UserFile
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] File { get; set; } = new byte[0];

        public int FileTypeId { get; set; }
        public FileType? FileType { get; set; }

		public int EmpleadoId { get; set; }
        public Empleados.Empleado? Empleado { get; set; }

		public UserFile()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

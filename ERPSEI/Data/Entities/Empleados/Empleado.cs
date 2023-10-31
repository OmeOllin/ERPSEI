namespace ERPSEI.Data.Entities.Empleados
{
    public class Empleado
    {
        //Campos de la entidad
        public int Id { get; set; }

		public byte[] ProfilePicture { get; set; } = new byte[0];

		public string PrimerNombre { get; set; } = string.Empty;

        public string SegundoNombre { get; set; } = string.Empty;

        public string ApellidoPaterno { get; set; } = string.Empty;

        public string ApellidoMaterno { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

		public DateTime FechaIngreso { get; set; }

		public DateTime FechaNacimiento { get; set; }

        public string Direccion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        
        //Relaciones de la entidad
        public int? GeneroId {  get; set; }
		public Genero? Genero { get; set; }

        public int? SubareaId { get; set; }
        public Subarea? Subarea { get; set; }

        public int? OficinaId { get; set; }
        public Oficina? Oficina { get; set; }

        public int? PuestoId { get; set; }
		public Puesto? Puesto { get; set; }

        public int? AreaId { get; set; }
		public Area? Area { get; set; }

        public int? EstadoCivilId {  get; set; }
		public EstadoCivil? EstadoCivil { get; set; }

		public ICollection<ContactoEmergencia>? ContactosEmergencia { get; }

        public int? ArchivoEmpleadoId { get; set; }
        public ICollection<ArchivoEmpleado>? ArchivosEmpleado { get; }

        public int? UserId { get; set; }
        public AppUser? User { get; set; }

    }
}

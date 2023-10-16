namespace ERPSEI.Data.Entities.Empleados
{
    public class Empleado
    {
        //Campos de la entidad
        public int Id { get; set; }

        public string ApellidoPaterno { get; set; } = string.Empty;

        public string ApellidoMaterno { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

		public DateTime FechaIngreso { get; set; }

		public DateTime FechaNacimiento { get; set; }

        public string CURP { get; set; } = string.Empty;

        public string RFC { get; set; } = string.Empty;

        public string NSS { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string TelefonoPersonal { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        
        //Relaciones de la entidad
        public int GeneroId {  get; set; }
		public Genero? Genero { get; set; }

        public int PuestoId { get; set; }
		public Puesto? Puesto { get; set; }

        public int AreaId { get; set; }
		public Area? Area { get; set; }

        public int EstadoCivilId {  get; set; }
		public EstadoCivil? EstadoCivil { get; set; }

		public ICollection<ContactoEmergencia>? ContactosEmergencia { get; }

        public AppUser? User { get; set; }

    }
}

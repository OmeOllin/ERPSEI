﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empleados
{
	public class Empleado
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string NombrePreferido { get; set; } = string.Empty;

		public string ApellidoPaterno { get; set; } = string.Empty;

		public string ApellidoMaterno { get; set; } = string.Empty;

		public string NombreCompleto { get; set; } = string.Empty;

		public DateTime FechaIngreso { get; set; }

		public DateTime FechaNacimiento { get; set; }

		public string Direccion { get; set; } = string.Empty;

		public string Telefono { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string CURP { get; set; } = string.Empty;

		public string RFC { get; set; } = string.Empty;

		public string NSS { get; set; } = string.Empty;

		public int Deshabilitado { get; set; } = 0;


		//Relaciones de la entidad
		public int? GeneroId { get; set; }
		public Genero? Genero { get; set; }

		public int? SubareaId { get; set; }
		public Subarea? Subarea { get; set; }

		public int? OficinaId { get; set; }
		public Oficina? Oficina { get; set; }

		public int? PuestoId { get; set; }
		public Puesto? Puesto { get; set; }

		public int? AreaId { get; set; }
		public Area? Area { get; set; }

		public int? EstadoCivilId { get; set; }
		public EstadoCivil? EstadoCivil { get; set; }

		public string? UserId { get; set; } = string.Empty;

		public ICollection<ContactoEmergencia>? ContactosEmergencia { get; }

		public ICollection<ArchivoEmpleado>? ArchivosEmpleado { get; }

		//Referencia a esta misma tabla para el jefe del empleado
		public int? JefeId { get; set; }

		//Empleados de los que el empleado es jefe
		[NotMapped]
		public List<Empleado>? Empleados { get; set; }

	}
}

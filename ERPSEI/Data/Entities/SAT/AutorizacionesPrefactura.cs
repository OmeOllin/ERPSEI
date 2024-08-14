using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
	public class AutorizacionesPrefactura
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public Prefactura? Prefactura { get; set; }
		public int PrefacturaId { get; set; }

		public AppUser? Usuario { get; set; }
		public string UsuarioId { get; set; } = string.Empty;

		public DateTime? FechaHoraAutorizacion { get; set; }
	}
}

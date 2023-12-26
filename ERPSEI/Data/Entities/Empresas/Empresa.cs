using ERPSEI.Data.Entities.Empleados;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
    public class Empresa
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string RazonSocial { get; set; } = string.Empty;

		public string Origen { get; set; } = string.Empty;

		public string Nivel { get; set; } = string.Empty;

        public string RFC {  get; set; } = string.Empty;

		public string DomicilioFiscal { get; set; } = string.Empty;

		public string Administrador { get; set; } = string.Empty;

		public string Accionista { get; set; } = string.Empty;

		public string CorreoGeneral { get; set; } = string.Empty;

		public string CorreoBancos { get; set; } = string.Empty;

		public string CorreoFiscal { get; set; } = string.Empty;

		public string Telefono { get; set; } = string.Empty;

		public ICollection<ArchivoEmpresa>? ArchivosEmpresa { get; }

	}
}

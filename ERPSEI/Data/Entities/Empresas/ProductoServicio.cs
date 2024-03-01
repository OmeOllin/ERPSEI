using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
    public class ProductoServicio
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Clave { get; set; } = string.Empty;

		public string Descripcion { get; set; } = string.Empty;

		public bool IncluirIVATraslado { get; set; }

		public bool IncluirIEPSTraslado { get; set; }

		public string PalabrasSimilares { get; set; } = string.Empty;

		public int? PerfilId { get; set; }
		public Perfil? Perfil { get; set; }

	}
}

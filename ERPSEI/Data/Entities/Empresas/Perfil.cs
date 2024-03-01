using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
    public class Perfil
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Nombre { get; set; } = string.Empty;

        public ICollection<Empresa>? Empresas { get; }

		public ICollection<ProductoServicio>? ProductosServicios { get; }

	}
}

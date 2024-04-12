using System.ComponentModel.DataAnnotations.Schema;
using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Entities.SAT
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

        public ICollection<ProductoServicioPerfil>? ProductosServiciosPerfil { get; set; }

		public int Deshabilitado { get; set; } = 0;

	}
}

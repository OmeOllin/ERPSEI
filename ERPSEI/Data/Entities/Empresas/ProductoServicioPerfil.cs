using System.ComponentModel.DataAnnotations.Schema;
using ERPSEI.Data.Entities.SAT;

namespace ERPSEI.Data.Entities.Empresas
{
    public class ProductoServicioPerfil
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

		public int? PerfilId { get; set; }
		public Perfil? Perfil { get; set; }

        public int? ProductoServicioId { get; set; }
        public ProductoServicio? ProductoServicio { get; set; }
    }
}

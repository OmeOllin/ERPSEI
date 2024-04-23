using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class EstatusPrefactura
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

		public int Deshabilitado { get; set; } = 0;

        public ICollection<Prefactura> Prefacturas { get; set; } = new HashSet<Prefactura>();

	}
}

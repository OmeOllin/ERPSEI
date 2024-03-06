using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
	public class ActividadEconomicaEmpresa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

		public int? EmpresaId { get; set; }
		public Empresa? Empresa { get; set; }

        public int? ActividadEconomicaId { get; set; }
        public ActividadEconomica? ActividadEconomica { get; set; }
    }
}

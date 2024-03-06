using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
	public class ActividadEconomica
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public ICollection<ActividadEconomicaEmpresa>? ActividadesEconomicasEmpresa { get; set; }
    }
}

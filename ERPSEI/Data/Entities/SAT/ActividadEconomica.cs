using System.ComponentModel.DataAnnotations.Schema;
using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Entities.SAT
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

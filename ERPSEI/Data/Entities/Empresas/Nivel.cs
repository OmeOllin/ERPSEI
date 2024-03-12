using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Empresas
{
    public class Nivel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool PuedeFacturar { get; set; } = true;

        public ICollection<Empresa>? Empresas { get; set; }
    }
}

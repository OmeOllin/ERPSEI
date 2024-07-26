using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT.Catalogos
{
    public class ObjetoImpuesto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public int Deshabilitado { get; set; } = 0;

        public ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
    }
}

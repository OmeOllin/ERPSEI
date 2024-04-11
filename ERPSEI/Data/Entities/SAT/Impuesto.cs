using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class Impuesto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public bool Retencion { get; set; }

        public bool Traslado { get; set; }

        public bool Local {  get; set; }

        public bool Federal { get; set; }

		public ICollection<TasaOCuota>? TasasOCuotas { get; set; }

	}
}

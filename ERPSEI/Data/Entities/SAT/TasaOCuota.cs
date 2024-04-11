using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class TasaOCuota
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

		public bool Rango { get; set; }

        public bool Fijo { get; set; }

        public double ValorMinimo { get; set; }

        public double ValorMaximo { get; set; }

        public Impuesto? Impuesto { get; set; }
        public int ImpuestoId { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public TipoFactor? Factor { get; set; }
        public int FactorId { get; set; }

        public bool Traslado { get; set; }

        public bool Retencion {  get; set; }

	}
}

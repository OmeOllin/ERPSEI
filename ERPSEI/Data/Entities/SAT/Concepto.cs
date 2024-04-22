using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
	public class Concepto
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public ProductoServicio? ProductoServicio { get; set; }
		public int ProductoServicioId { get; set; }

		public int Cantidad { get; set; }

		public decimal PrecioUnitario { get; set; }

		public decimal Descuento { get; set; }

		public UnidadMedida? UnidadMedida { get; set; }
		public int UnidadMedidaId { get; set; }

		public string Descripcion { get; set; } = string.Empty;

		public ObjetoImpuesto? ObjetoImpuesto { get; set; }
		public int ObjetoImpuestoId { get; set; }

		public decimal TasaTraslado { get; set; }

		public decimal TasaRetencion { get; set; }

		public Prefactura? Prefactura { get; set; }
		public int? PrefacturaId { get; set; }
	}
}

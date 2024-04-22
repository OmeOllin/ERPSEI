using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
	public class Prefactura
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Serie { get; set; } = string.Empty;

		public string Folio { get; set; } = string.Empty;

		public int TipoComprobanteId { get; set; }

		public DateTime Fecha { get; set; }

		public int MonedaId { get; set; }

		public decimal TipoCambio { get; set; }

		public int FormaPagoId { get; set; }

		public int MetodoPagoId { get; set; }

		public int UsoCFDIId { get; set; }

		public int? ExportacionId { get; set; }

		public int? NumeroOperacion { get; set; }

		public ICollection<Concepto> Conceptos { get; } = new List<Concepto>();

		public int Deshabilitado { get; set; }
	}
}

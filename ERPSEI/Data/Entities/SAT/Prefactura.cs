using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class Prefactura
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public Empresa? Emisor { get; set; }
		public int EmisorId { get; set; }

		public Empresa? Receptor { get; set; }
		public int ReceptorId { get; set; }

		public string Serie { get; set; } = string.Empty;

		public string Folio { get; set; } = string.Empty;

		public int TipoComprobanteId { get; set; }
		public TipoComprobante? TipoComprobante { get; set; }

		public DateTime Fecha { get; set; }

		public Moneda? Moneda { get; set; }
		public int MonedaId { get; set; }

		public decimal TipoCambio { get; set; }

		public FormaPago? FormaPago { get; set; }
		public int FormaPagoId { get; set; }

		public MetodoPago? MetodoPago { get; set; }
		public int MetodoPagoId { get; set; }

		public UsoCFDI? UsoCFDI { get; set; }
		public int UsoCFDIId { get; set; }

		public Exportacion? Exportacion { get; set; }
		public int? ExportacionId { get; set; }

		public int? NumeroOperacion { get; set; }

		public ICollection<Concepto> Conceptos { get; } = new List<Concepto>();

		public int Deshabilitado { get; set; }

		public EstatusPrefactura? Estatus { get; set;}
		public int? EstatusId { get; set; }

		public AppUser? UsuarioCreador { get; set; }
		public string? UsuarioCreadorId { get; set; }

        public AppUser? UsuarioAutorizador { get; set; }
        public string? UsuarioAutorizadorId { get; set; }

        public AppUser? UsuarioFinalizador { get; set; }
		public string? UsuarioFinalizadorId { get; set; }
	}
}

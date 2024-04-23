using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class FormaPago
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

		public string Bancarizado { get; set; } = string.Empty;

		public string NumeroDeOperacion { get; set; } = string.Empty;

		public string RFCEmisorCuentaOrdenante { get; set; } = string.Empty;

		public string CuentaOrdenante { get; set; } = string.Empty;

		public string PatronCuentaOrdenante { get; set; } = string.Empty;

		public string RFCEmisorCuentaBeneficiario { get; set; } = string.Empty;

		public string CuentaBeneficiario { get; set; } = string.Empty;

		public string PatronCuentaBeneficiario { get; set; } = string.Empty;

		public string TipoCadenaPago { get; set; } = string.Empty;

		public string NombreBancoEmisorCuenta { get; set; } = string.Empty;

		public int Deshabilitado { get; set; } = 0;

		public Prefactura? Prefactura { get; set; }

	}
}

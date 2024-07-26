namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoACuentaTerceros
	{

		private string rfcACuentaTercerosField = string.Empty;

		private string nombreACuentaTercerosField = string.Empty;

		private string regimenFiscalACuentaTercerosField = string.Empty;

		private string domicilioFiscalACuentaTercerosField = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string RfcACuentaTerceros
		{
			get
			{
				return this.rfcACuentaTercerosField;
			}
			set
			{
				this.rfcACuentaTercerosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NombreACuentaTerceros
		{
			get
			{
				return this.nombreACuentaTercerosField;
			}
			set
			{
				this.nombreACuentaTercerosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string RegimenFiscalACuentaTerceros
		{
			get
			{
				return this.regimenFiscalACuentaTercerosField;
			}
			set
			{
				this.regimenFiscalACuentaTercerosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string DomicilioFiscalACuentaTerceros
		{
			get
			{
				return this.domicilioFiscalACuentaTercerosField;
			}
			set
			{
				this.domicilioFiscalACuentaTercerosField = value;
			}
		}
	}
}

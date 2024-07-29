namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteEmisor
	{

		private string rfcField = string.Empty;

		private string nombreField = string.Empty;

		private string regimenFiscalField = string.Empty;

		private string facAtrAdquirenteField = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Rfc
		{
			get
			{
				return this.rfcField;
			}
			set
			{
				this.rfcField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Nombre
		{
			get
			{
				return this.nombreField;
			}
			set
			{
				this.nombreField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string RegimenFiscal
		{
			get
			{
				return this.regimenFiscalField;
			}
			set
			{
				this.regimenFiscalField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string FacAtrAdquirente
		{
			get
			{
				return this.facAtrAdquirenteField;
			}
			set
			{
				this.facAtrAdquirenteField = value;
			}
		}
	}
}

namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteReceptor
	{

		private string rfcField = string.Empty;

		private string nombreField = string.Empty;

		private string domicilioFiscalReceptorField = string.Empty;

		private string residenciaFiscalField = string.Empty;

		private bool residenciaFiscalFieldSpecified;

		private string numRegIdTribField = string.Empty;

		private string regimenFiscalReceptorField = string.Empty;

		private string usoCFDIField = string.Empty;

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
		public string DomicilioFiscalReceptor
		{
			get
			{
				return this.domicilioFiscalReceptorField;
			}
			set
			{
				this.domicilioFiscalReceptorField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ResidenciaFiscal
		{
			get
			{
				return this.residenciaFiscalField;
			}
			set
			{
				this.residenciaFiscalField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ResidenciaFiscalSpecified
		{
			get
			{
				return this.residenciaFiscalFieldSpecified;
			}
			set
			{
				this.residenciaFiscalFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NumRegIdTrib
		{
			get
			{
				return this.numRegIdTribField;
			}
			set
			{
				this.numRegIdTribField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string RegimenFiscalReceptor
		{
			get
			{
				return this.regimenFiscalReceptorField;
			}
			set
			{
				this.regimenFiscalReceptorField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string UsoCFDI
		{
			get
			{
				return this.usoCFDIField;
			}
			set
			{
				this.usoCFDIField = value;
			}
		}
	}
}

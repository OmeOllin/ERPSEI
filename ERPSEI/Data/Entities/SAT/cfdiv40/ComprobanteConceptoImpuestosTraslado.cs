namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoImpuestosTraslado
	{

		private decimal baseField;

		private string impuestoField = string.Empty;

		private string tipoFactorField = string.Empty;

		private decimal tasaOCuotaField;

		private bool tasaOCuotaFieldSpecified;

		private decimal importeField;

		private bool importeFieldSpecified;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Base
		{
			get
			{
				return this.baseField;
			}
			set
			{
				this.baseField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Impuesto
		{
			get
			{
				return this.impuestoField;
			}
			set
			{
				this.impuestoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string TipoFactor
		{
			get
			{
				return this.tipoFactorField;
			}
			set
			{
				this.tipoFactorField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TasaOCuota
		{
			get
			{
				return this.tasaOCuotaField;
			}
			set
			{
				this.tasaOCuotaField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TasaOCuotaSpecified
		{
			get
			{
				return this.tasaOCuotaFieldSpecified;
			}
			set
			{
				this.tasaOCuotaFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Importe
		{
			get
			{
				return this.importeField;
			}
			set
			{
				this.importeField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ImporteSpecified
		{
			get
			{
				return this.importeFieldSpecified;
			}
			set
			{
				this.importeFieldSpecified = value;
			}
		}
	}
}

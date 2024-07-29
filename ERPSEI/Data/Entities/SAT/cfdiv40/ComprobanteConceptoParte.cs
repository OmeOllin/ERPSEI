namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoParte
	{

		private ComprobanteConceptoParteInformacionAduanera[]? informacionAduaneraField;

		private string claveProdServField = string.Empty;

		private string noIdentificacionField = string.Empty;

		private decimal cantidadField;

		private string unidadField = string.Empty;

		private string descripcionField = string.Empty;

		private decimal valorUnitarioField;

		private bool valorUnitarioFieldSpecified;

		private decimal importeField;

		private bool importeFieldSpecified;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("InformacionAduanera")]
		public ComprobanteConceptoParteInformacionAduanera[]? InformacionAduanera
		{
			get
			{
				return this.informacionAduaneraField;
			}
			set
			{
				this.informacionAduaneraField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ClaveProdServ
		{
			get
			{
				return this.claveProdServField;
			}
			set
			{
				this.claveProdServField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NoIdentificacion
		{
			get
			{
				return this.noIdentificacionField;
			}
			set
			{
				this.noIdentificacionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Cantidad
		{
			get
			{
				return this.cantidadField;
			}
			set
			{
				this.cantidadField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Unidad
		{
			get
			{
				return this.unidadField;
			}
			set
			{
				this.unidadField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Descripcion
		{
			get
			{
				return this.descripcionField;
			}
			set
			{
				this.descripcionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal ValorUnitario
		{
			get
			{
				return this.valorUnitarioField;
			}
			set
			{
				this.valorUnitarioField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ValorUnitarioSpecified
		{
			get
			{
				return this.valorUnitarioFieldSpecified;
			}
			set
			{
				this.valorUnitarioFieldSpecified = value;
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

namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConcepto
	{

		private ComprobanteConceptoImpuestos? impuestosField;

		private ComprobanteConceptoACuentaTerceros? aCuentaTercerosField;

		private ComprobanteConceptoInformacionAduanera[]? informacionAduaneraField;

		private ComprobanteConceptoCuentaPredial[]? cuentaPredialField;

		private ComprobanteConceptoComplementoConcepto? complementoConceptoField;

		private ComprobanteConceptoParte[]? parteField;

		private string claveProdServField = string.Empty;

		private string noIdentificacionField = string.Empty;

		private decimal cantidadField;

		private string claveUnidadField = string.Empty;

		private string unidadField = string.Empty;

		private string descripcionField = string.Empty;

		private decimal valorUnitarioField;

		private decimal importeField;

		private decimal descuentoField;

		private bool descuentoFieldSpecified;

		private string objetoImpField = string.Empty;

		/// <remarks/>
		public ComprobanteConceptoImpuestos? Impuestos
		{
			get
			{
				return this.impuestosField;
			}
			set
			{
				this.impuestosField = value;
			}
		}

		/// <remarks/>
		public ComprobanteConceptoACuentaTerceros? ACuentaTerceros
		{
			get
			{
				return this.aCuentaTercerosField;
			}
			set
			{
				this.aCuentaTercerosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("InformacionAduanera")]
		public ComprobanteConceptoInformacionAduanera[]? InformacionAduanera
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
		[System.Xml.Serialization.XmlElementAttribute("CuentaPredial")]
		public ComprobanteConceptoCuentaPredial[]? CuentaPredial
		{
			get
			{
				return this.cuentaPredialField;
			}
			set
			{
				this.cuentaPredialField = value;
			}
		}

		/// <remarks/>
		public ComprobanteConceptoComplementoConcepto? ComplementoConcepto
		{
			get
			{
				return this.complementoConceptoField;
			}
			set
			{
				this.complementoConceptoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("Parte")]
		public ComprobanteConceptoParte[]? Parte
		{
			get
			{
				return this.parteField;
			}
			set
			{
				this.parteField = value;
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
		public string ClaveUnidad
		{
			get
			{
				return this.claveUnidadField;
			}
			set
			{
				this.claveUnidadField = value;
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
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Descuento
		{
			get
			{
				return this.descuentoField;
			}
			set
			{
				this.descuentoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DescuentoSpecified
		{
			get
			{
				return this.descuentoFieldSpecified;
			}
			set
			{
				this.descuentoFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ObjetoImp
		{
			get
			{
				return this.objetoImpField;
			}
			set
			{
				this.objetoImpField = value;
			}
		}
	}
}

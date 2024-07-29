namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/4", IsNullable = false)]
	public partial class Comprobante
	{

		private ComprobanteInformacionGlobal? informacionGlobalField;

		private ComprobanteCfdiRelacionados[]? cfdiRelacionadosField;

		private ComprobanteEmisor? emisorField;

		private ComprobanteReceptor? receptorField;

		private ComprobanteConcepto[]? conceptosField;

		private ComprobanteImpuestos? impuestosField;

		private ComprobanteComplemento? complementoField;

		private ComprobanteAddenda? addendaField;

		private string versionField;

		private string serieField = string.Empty;

		private string folioField = string.Empty;

		private System.DateTime fechaField;

		private string selloField = string.Empty;

		private string formaPagoField = string.Empty;

		private bool formaPagoFieldSpecified;

		private string noCertificadoField = string.Empty;

		private string certificadoField = string.Empty;

		private string condicionesDePagoField = string.Empty;

		private decimal subTotalField;

		private decimal descuentoField;

		private bool descuentoFieldSpecified;

		private string monedaField = string.Empty;

		private decimal tipoCambioField;

		private bool tipoCambioFieldSpecified;

		private decimal totalField;

		private string tipoDeComprobanteField = string.Empty;

		private string exportacionField = string.Empty;

		private string metodoPagoField = string.Empty;

		private bool metodoPagoFieldSpecified;

		private string lugarExpedicionField = string.Empty;

		private string confirmacionField = string.Empty;

		public Comprobante()
		{
			this.versionField = "4.0";
		}

		/// <remarks/>
		public ComprobanteInformacionGlobal? InformacionGlobal
		{
			get
			{
				return this.informacionGlobalField;
			}
			set
			{
				this.informacionGlobalField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("CfdiRelacionados")]
		public ComprobanteCfdiRelacionados[]? CfdiRelacionados
		{
			get
			{
				return this.cfdiRelacionadosField;
			}
			set
			{
				this.cfdiRelacionadosField = value;
			}
		}

		/// <remarks/>
		public ComprobanteEmisor? Emisor
		{
			get
			{
				return this.emisorField;
			}
			set
			{
				this.emisorField = value;
			}
		}

		/// <remarks/>
		public ComprobanteReceptor? Receptor
		{
			get
			{
				return this.receptorField;
			}
			set
			{
				this.receptorField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Concepto", IsNullable = false)]
		public ComprobanteConcepto[]? Conceptos
		{
			get
			{
				return this.conceptosField;
			}
			set
			{
				this.conceptosField = value;
			}
		}

		/// <remarks/>
		public ComprobanteImpuestos? Impuestos
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
		public ComprobanteComplemento? Complemento
		{
			get
			{
				return this.complementoField;
			}
			set
			{
				this.complementoField = value;
			}
		}

		/// <remarks/>
		public ComprobanteAddenda? Addenda
		{
			get
			{
				return this.addendaField;
			}
			set
			{
				this.addendaField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Serie
		{
			get
			{
				return this.serieField;
			}
			set
			{
				this.serieField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Folio
		{
			get
			{
				return this.folioField;
			}
			set
			{
				this.folioField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public System.DateTime Fecha
		{
			get
			{
				return this.fechaField;
			}
			set
			{
				this.fechaField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Sello
		{
			get
			{
				return this.selloField;
			}
			set
			{
				this.selloField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string FormaPago
		{
			get
			{
				return this.formaPagoField;
			}
			set
			{
				this.formaPagoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool FormaPagoSpecified
		{
			get
			{
				return this.formaPagoFieldSpecified;
			}
			set
			{
				this.formaPagoFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NoCertificado
		{
			get
			{
				return this.noCertificadoField;
			}
			set
			{
				this.noCertificadoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Certificado
		{
			get
			{
				return this.certificadoField;
			}
			set
			{
				this.certificadoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string CondicionesDePago
		{
			get
			{
				return this.condicionesDePagoField;
			}
			set
			{
				this.condicionesDePagoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal SubTotal
		{
			get
			{
				return this.subTotalField;
			}
			set
			{
				this.subTotalField = value;
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
		public string Moneda
		{
			get
			{
				return this.monedaField;
			}
			set
			{
				this.monedaField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TipoCambio
		{
			get
			{
				return this.tipoCambioField;
			}
			set
			{
				this.tipoCambioField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TipoCambioSpecified
		{
			get
			{
				return this.tipoCambioFieldSpecified;
			}
			set
			{
				this.tipoCambioFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Total
		{
			get
			{
				return this.totalField;
			}
			set
			{
				this.totalField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string TipoDeComprobante
		{
			get
			{
				return this.tipoDeComprobanteField;
			}
			set
			{
				this.tipoDeComprobanteField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Exportacion
		{
			get
			{
				return this.exportacionField;
			}
			set
			{
				this.exportacionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string MetodoPago
		{
			get
			{
				return this.metodoPagoField;
			}
			set
			{
				this.metodoPagoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MetodoPagoSpecified
		{
			get
			{
				return this.metodoPagoFieldSpecified;
			}
			set
			{
				this.metodoPagoFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string LugarExpedicion
		{
			get
			{
				return this.lugarExpedicionField;
			}
			set
			{
				this.lugarExpedicionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Confirmacion
		{
			get
			{
				return this.confirmacionField;
			}
			set
			{
				this.confirmacionField = value;
			}
		}
	}
}

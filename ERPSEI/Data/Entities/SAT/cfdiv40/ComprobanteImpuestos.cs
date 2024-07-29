namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteImpuestos
	{

		private ComprobanteImpuestosRetencion[]? retencionesField;

		private ComprobanteImpuestosTraslado[]? trasladosField;

		private decimal totalImpuestosRetenidosField;

		private bool totalImpuestosRetenidosFieldSpecified;

		private decimal totalImpuestosTrasladadosField;

		private bool totalImpuestosTrasladadosFieldSpecified;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable = false)]
		public ComprobanteImpuestosRetencion[]? Retenciones
		{
			get
			{
				return this.retencionesField;
			}
			set
			{
				this.retencionesField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable = false)]
		public ComprobanteImpuestosTraslado[]? Traslados
		{
			get
			{
				return this.trasladosField;
			}
			set
			{
				this.trasladosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TotalImpuestosRetenidos
		{
			get
			{
				return this.totalImpuestosRetenidosField;
			}
			set
			{
				this.totalImpuestosRetenidosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TotalImpuestosRetenidosSpecified
		{
			get
			{
				return this.totalImpuestosRetenidosFieldSpecified;
			}
			set
			{
				this.totalImpuestosRetenidosFieldSpecified = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TotalImpuestosTrasladados
		{
			get
			{
				return this.totalImpuestosTrasladadosField;
			}
			set
			{
				this.totalImpuestosTrasladadosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TotalImpuestosTrasladadosSpecified
		{
			get
			{
				return this.totalImpuestosTrasladadosFieldSpecified;
			}
			set
			{
				this.totalImpuestosTrasladadosFieldSpecified = value;
			}
		}
	}
}

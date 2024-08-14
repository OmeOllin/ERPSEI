namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteImpuestos
	{
		[System.Xml.Serialization.XmlIgnore]
		public int Id { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable = false)]
		public ComprobanteImpuestosRetencion[]? Retenciones { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable = false)]
		public ComprobanteImpuestosTraslado[]? Traslados { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TotalImpuestosRetenidos {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TotalImpuestosRetenidosSpecified { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TotalImpuestosTrasladados {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TotalImpuestosTrasladadosSpecified {  get; set; }
	}
}

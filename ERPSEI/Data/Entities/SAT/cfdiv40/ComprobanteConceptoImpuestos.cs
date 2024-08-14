namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoImpuestos
	{
		[System.Xml.Serialization.XmlIgnore]
		public int Id { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable = false)]
		public ComprobanteConceptoImpuestosTraslado[]? Traslados { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable = false)]
		public ComprobanteConceptoImpuestosRetencion[]? Retenciones { get; set; }
	}
}

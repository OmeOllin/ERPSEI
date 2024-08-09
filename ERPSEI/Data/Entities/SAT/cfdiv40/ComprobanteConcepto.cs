namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConcepto
	{
		[System.Xml.Serialization.XmlIgnore]
		public int Id { get; set; }

		/// <remarks/>
		public ComprobanteConceptoImpuestos? Impuestos {  get; set; }

		/// <remarks/>
		public ComprobanteConceptoACuentaTerceros? ACuentaTerceros {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("InformacionAduanera")]
		public ComprobanteConceptoInformacionAduanera[]? InformacionAduanera {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("CuentaPredial")]
		public ComprobanteConceptoCuentaPredial[]? CuentaPredial {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("Parte")]
		public ComprobanteConceptoParte[]? Parte {  get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ClaveProdServ { get; set; } = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NoIdentificacion { get; set; } = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Cantidad { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ClaveUnidad { get; set; } = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Unidad { get; set; } = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Descripcion { get; set; } = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal ValorUnitario { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Importe { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Descuento { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DescuentoSpecified { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ObjetoImp { get; set; } = string.Empty;
	}
}

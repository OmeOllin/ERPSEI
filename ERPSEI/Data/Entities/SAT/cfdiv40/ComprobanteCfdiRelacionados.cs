namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteCfdiRelacionados
	{
		[System.Xml.Serialization.XmlIgnore]
		public int Id { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("CfdiRelacionado")]
		public ComprobanteCfdiRelacionadosCfdiRelacionado[]? CfdiRelacionado { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string TipoRelacion { get; set; } = string.Empty;
	}
}

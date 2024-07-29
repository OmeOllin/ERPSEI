namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteCfdiRelacionados
	{

		private ComprobanteCfdiRelacionadosCfdiRelacionado[]? cfdiRelacionadoField;

		private string tipoRelacionField = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("CfdiRelacionado")]
		public ComprobanteCfdiRelacionadosCfdiRelacionado[]? CfdiRelacionado
		{
			get
			{
				return this.cfdiRelacionadoField;
			}
			set
			{
				this.cfdiRelacionadoField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string TipoRelacion
		{
			get
			{
				return this.tipoRelacionField;
			}
			set
			{
				this.tipoRelacionField = value;
			}
		}
	}
}

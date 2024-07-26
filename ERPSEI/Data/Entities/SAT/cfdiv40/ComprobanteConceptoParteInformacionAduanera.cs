namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoParteInformacionAduanera
	{

		private string numeroPedimentoField = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string NumeroPedimento
		{
			get
			{
				return this.numeroPedimentoField;
			}
			set
			{
				this.numeroPedimentoField = value;
			}
		}
	}
}

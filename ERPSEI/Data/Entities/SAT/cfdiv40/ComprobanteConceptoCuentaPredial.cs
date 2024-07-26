namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteConceptoCuentaPredial
	{

		private string numeroField = string.Empty;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Numero
		{
			get
			{
				return this.numeroField;
			}
			set
			{
				this.numeroField = value;
			}
		}
	}
}

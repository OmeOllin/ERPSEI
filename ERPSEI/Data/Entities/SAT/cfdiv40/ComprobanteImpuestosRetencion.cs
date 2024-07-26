namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteImpuestosRetencion
	{

		private string impuestoField = string.Empty;

		private decimal importeField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Impuesto
		{
			get
			{
				return this.impuestoField;
			}
			set
			{
				this.impuestoField = value;
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
	}
}

namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	public partial class ComprobanteInformacionGlobal
	{

		private string periodicidadField = string.Empty;

		private string mesesField = string.Empty;

		private short añoField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Periodicidad
		{
			get
			{
				return this.periodicidadField;
			}
			set
			{
				this.periodicidadField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Meses
		{
			get
			{
				return this.mesesField;
			}
			set
			{
				this.mesesField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public short Año
		{
			get
			{
				return this.añoField;
			}
			set
			{
				this.añoField = value;
			}
		}
	}
}

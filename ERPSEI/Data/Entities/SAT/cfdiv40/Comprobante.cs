using ERPSEI.Data.Entities.Conciliaciones;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ERPSEI.Data.Entities.SAT.cfdiv40
{
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/4", IsNullable = false)]
	public partial class Comprobante
	{
		[System.Xml.Serialization.XmlIgnore]
		public int Id { get; set; }

		public ComprobanteInformacionGlobal? InformacionGlobal{ get; set; }

		
		[System.Xml.Serialization.XmlElementAttribute("CfdiRelacionados")]
		public ComprobanteCfdiRelacionados[]? CfdiRelacionados { get; set; }

		
		public ComprobanteEmisor? Emisor { get; set; }

		
		public ComprobanteReceptor? Receptor { get; set; }

		
		[System.Xml.Serialization.XmlArrayItemAttribute("Concepto", IsNullable = false)]
		public ComprobanteConcepto[]? Conceptos { get; set; }

		
		public ComprobanteImpuestos? Impuestos { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Version { get; set; } = "4.0";

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Serie { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Folio { get; set; }


		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Fecha { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Sello { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? FormaPago { get; set; }

		
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool FormaPagoSpecified { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? NoCertificado { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Certificado {  get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? CondicionesDePago { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal SubTotal { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Descuento { get; set; }

		
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DescuentoSpecified { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Moneda { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal TipoCambio { get; set; }

		
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TipoCambioSpecified { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public decimal Total { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? TipoDeComprobante { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Exportacion { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? MetodoPago { get; set; }

		
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MetodoPagoSpecified { get; set; }

		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? LugarExpedicion { get; set; }

		[XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
		public string xsiSchemaLocation = "http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
		
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string? Confirmacion { get; set; }

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public ConciliacionDetalleComprobante? ConciliacionDetalleComprobante { get; set; }
    }
}

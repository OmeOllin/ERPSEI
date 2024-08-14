using System.Text;

namespace ERPSEI.Utils
{
	public class StringWriterCustomEncoding : StringWriter
	{
		private readonly Encoding m_Encoding;

		public override Encoding Encoding => this.m_Encoding;

		public StringWriterCustomEncoding(Encoding encoding) : base()
		{
			m_Encoding = encoding;	
		}
	}
}

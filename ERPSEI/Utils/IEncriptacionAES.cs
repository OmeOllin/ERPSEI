namespace ERPSEI.Utils
{
	public interface IEncriptacionAES
	{
		public string PlainTextToBase64AES(string rawString);

		public string Base64AESToPlainText(string encodedString);
	}
}

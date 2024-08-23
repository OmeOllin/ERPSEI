namespace ERPSEI.Utils
{
	public interface IEncriptacionAES
	{
		public string EncriptarString(string rawString);

		public string DesencriptarAES(string encodedString);
	}
}

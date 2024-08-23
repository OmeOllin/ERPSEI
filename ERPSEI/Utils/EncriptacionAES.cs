using System.Security.Cryptography;
using System.Text;

namespace ERPSEI.Utils
{
	public class EncriptacionAES(IConfiguration _configuration) : IEncriptacionAES
	{
		private byte[] ERPSEI_ALPHA { get; set; } = Encoding.UTF8.GetBytes(_configuration["ERPSEI_ALPHA"] ?? string.Empty);
		private byte[] ERPSEI_BRAVO { set; get; } = Encoding.UTF8.GetBytes(_configuration["ERPSEI_BRAVO"] ?? string.Empty);

		public string EncriptarString(string rawString)
		{
			byte[] encrypted;
			using (Aes aes = Aes.Create())
			{
				ICryptoTransform encryptor = aes.CreateEncryptor(ERPSEI_ALPHA, ERPSEI_BRAVO);
				using (MemoryStream ms = new())
				{
					using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter sw = new(cs))
							sw.Write(rawString);
						encrypted = ms.ToArray();
					}
				}
			}
			// Return encrypted data
			return Encoding.UTF8.GetString(encrypted);
		}

		public string DesencriptarAES(string encodedString)
		{
			string plaintext;
			// Create AesManaged
			using (Aes aes = Aes.Create())
			{
				// Create a decryptor
				ICryptoTransform decryptor = aes.CreateDecryptor(ERPSEI_ALPHA, ERPSEI_BRAVO);
				// Create the streams used for decryption.
				using (MemoryStream ms = new(Encoding.UTF8.GetBytes(encodedString)))
				{
					// Create crypto stream
					using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
					{
						// Read crypto stream
						using (StreamReader reader = new(cs))
							plaintext = reader.ReadToEnd();
					}
				}
			}
			return plaintext;
		}
	}
}

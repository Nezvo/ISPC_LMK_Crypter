using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;

namespace Crypter
{
	class CryptoEngine
	{
		public static bool OnlyHexInString(string check)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(check, @"\A\b[0-9a-fA-F]+\b\Z");
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}

		public static byte[] Crypt(byte[] inputBuffer, byte[] key)
		{
			using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
			{
				des.Key = key;
				des.Mode = CipherMode.ECB;
				des.Padding = PaddingMode.None;

				byte[] result;

				using (MemoryStream stream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(inputBuffer, 0, inputBuffer.Length);
						cryptoStream.Flush();
						result = stream.ToArray();
					}
				}

				return result;
			}
		}

		public static byte[] Decrypt(byte[] inputBuffer, byte[] key)
		{
			using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
			{
				des.Key = key;
				des.Mode = CipherMode.ECB;
				des.Padding = PaddingMode.None;

				byte[] result;

				using (MemoryStream stream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(stream, des.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(inputBuffer, 0, inputBuffer.Length);
						cryptoStream.Flush();
						result = stream.ToArray();
					}
				}
				return result;
			}
		}

		public static byte[] GetKCVDES(string key)
		{
			byte[] retVal = null;

			try
			{
				using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
				{
					des.Mode = CipherMode.ECB;
					des.Padding = PaddingMode.None;

					byte[] keyDes = StringToByteArray(key);
					des.Key = keyDes;

					byte[] data = StringToByteArray("00000000000000000000000000000000");

					using (MemoryStream stream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(data, 0, data.Length);
							cryptoStream.Flush();
							retVal = stream.ToArray();
						}
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			return retVal;
		}
	}
}

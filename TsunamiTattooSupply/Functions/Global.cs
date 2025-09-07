namespace TsunamiTattooSupply.Functions
{
	public static class Global
	{ 
		public static string Encrypt(string str)
		{
			byte[] byt = System.Text.Encoding.UTF8.GetBytes(str);
			return Convert.ToBase64String(byt);
		}

		public static string Decrypt(string str)
		{
			byte[] data;
			data = System.Convert.FromBase64String(str);
			return System.Text.ASCIIEncoding.UTF8.GetString(data);
		}

	}
}

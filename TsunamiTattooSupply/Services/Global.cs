﻿
using Microsoft.EntityFrameworkCore;
using System.IO;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.Models;

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

		public static string CategoryWebImagePath = string.Empty;
		public static string CategoryBannerImagePath = string.Empty;
		public static string CategoryADImagePath = string.Empty;
		public static string CategoryMobileImagePath = string.Empty;

	}
	 
}

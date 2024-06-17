using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services.Extentions
{
	public static class CustomExtensions
	{
		public static string SanitizeFile(this string fileName)
		{
			string sanitizedFileName = new string(fileName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c)).ToArray());
			return sanitizedFileName;
		}
	}
}

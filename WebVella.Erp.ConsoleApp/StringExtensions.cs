using System.IO;
using System.Text.RegularExpressions;

namespace WebVella.Erp.ConsoleApp
{
	public static class StringExtensions
	{
		//utility method to get configuration file path
		public static string ToApplicationPath(this string fileName)
		{
			var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
			Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;
			return Path.Combine(appRoot, fileName);
		}
	}
}

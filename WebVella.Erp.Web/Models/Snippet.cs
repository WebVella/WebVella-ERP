using System.IO;
using System.Reflection;

namespace WebVella.Erp.Web.Models
{
	internal class Snippet
	{
		public string Name { get; set; }

		public Assembly Assembly { get; set; }

		public string GetText()
		{
			Stream resource = Assembly.GetManifestResourceStream(Name);
			StreamReader reader = new StreamReader(resource);
			return reader.ReadToEnd();
		}

	}
}

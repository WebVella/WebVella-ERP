using Newtonsoft.Json;

namespace WebVella.ERP.Database
{
	public class DbSystemSettings : DbDocumentBase
    {
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
    }
}

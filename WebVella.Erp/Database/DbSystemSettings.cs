using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbSystemSettings : DbDocumentBase
    {
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
    }
}

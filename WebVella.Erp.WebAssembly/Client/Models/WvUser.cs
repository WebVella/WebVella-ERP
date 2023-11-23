using System.Text.Json.Serialization;

namespace WebVella.Erp.WebAssembly.Models;

public class WvUser
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
}

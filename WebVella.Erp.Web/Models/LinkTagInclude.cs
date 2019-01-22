using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class LinkTagInclude
	{
		[JsonProperty("href")]
		public string Href { get; set; } = "";

		[JsonProperty("hreflang")]
		public string HrefLang { get; set; } = "";

		[JsonProperty("weight")]
		public int Weight { get; set; } = 100;

		//Specifies that the script is executed when the page has finished parsing (only for external scripts)
		[JsonProperty("defer")]
		public bool Defer { get; set; } = false;

		//Specifies that the script is executed asynchronously (only for external scripts)
		[JsonProperty("async")]
		public bool Async { get; set; } = false;

		//Specifies the character encoding used in an external script file
		[JsonProperty("charset")]
		public string Charset { get; set; } = "";

		//specifies the media type of the script
		[JsonProperty("type")]
		public string Type { get; set; } = "text/css";

		[JsonProperty("integrity")]
		public string Integrity { get; set; } = "";

		[JsonProperty("crossorigin")]
		public CrossOriginType CrossOrigin { get; set; } = CrossOriginType.None;

		//Required. Specifies the relationship between the current document and the linked document
		[JsonProperty("rel")]
		public RelType Rel { get; set; } = RelType.Stylesheet;

		[JsonProperty("sizes")]
		public string Sizes { get; set; } = ""; // HeightxWidth  ; Specifies the size of the linked resource. Only for rel="icon"

		//Specifies on what device the linked document will be displayed
		[JsonProperty("media")]
		public string Media { get; set; } = "";

		[JsonProperty("cache_breaker")]
		public string CacheBreaker { get; set; } = "";

		[JsonProperty("attributes_result")]
		public string AttributesResult { get; set; } = ""; //Helper method to precalculate the non required attributes

		[JsonProperty("InlineContent")]
		public string InlineContent { get; set; } = "";

	}

	public enum RelType { 
		Alternate = 1,
		Author = 2,
		DnsPrefetch = 3,
		Help = 4,
		Icon = 5,
		License = 6,
		Next = 7,
		Pingback = 8,
		Preconnect = 9,
		Prefetch = 10,
		Preload = 11,
		Prerender = 12,
		Prev = 13,
		Search = 14,
		Stylesheet = 15
	}
}

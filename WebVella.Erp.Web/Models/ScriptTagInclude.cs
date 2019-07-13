using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class ScriptTagInclude
	{
		[JsonProperty("src")]
		public string Src { get; set; } = "";

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
		public string Type { get; set; } = "text/javascript";

		//Adds nomodule attribute
		[JsonProperty("is_nomodule")]
		public bool IsNomodule { get; set; } = false;

		[JsonProperty("crossorigin")]
		public CrossOriginType CrossOrigin { get; set; } = CrossOriginType.None;

		[JsonProperty("position")]
		public PositionType Position { get; set; } = PositionType.BeforeEndBody;

		[JsonProperty("integrity")]
		public string Integrity { get; set; } = "";

		[JsonProperty("cache_breaker")]
		public string CacheBreaker { get; set; } = "";

		[JsonProperty("attributes_result")]
		public string AttributesResult { get; set; } = ""; //Helper method to precalculate the non required attributes

		[JsonProperty("InlineContent")]
		public string InlineContent { get; set; } = "";
	}

	public enum PositionType { 
		Head = 1,
		BeforeEndBody = 2
	}

	public enum CrossOriginType
	{
		None = 1,
		Anonymous = 2,
		UseCredentials = 3
	}

}

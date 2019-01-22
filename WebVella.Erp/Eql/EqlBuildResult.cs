using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Eql
{
	public class EqlBuildResult
	{
		public List<EqlError> Errors { get; private set; } = new List<EqlError>();
		internal EqlAbstractTree Tree { get; set; } = null;
		public List<EqlFieldMeta> Meta { get; private set; } = new List<EqlFieldMeta>();
		public List<EqlParameter> Parameters { get; private set; } = new List<EqlParameter>();
		public List<string> ExpectedParameters { get; private set; } = new List<string>();
		public Entity FromEntity { get; set; } = null;
		public string Sql { get; set; }
	}
}

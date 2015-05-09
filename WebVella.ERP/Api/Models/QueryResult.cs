using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class QueryResult
    {
		public List<Field> FieldsMeta { get; set; }
		public List<EntityRecord> Data { get; set; }
    }
}

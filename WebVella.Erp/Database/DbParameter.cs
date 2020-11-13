using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Database.Models
{
    public class DbParameter
    {
		public string Name { get; set; }

		public object Value { get; set; }

		public NpgsqlDbType Type { get; set; }
		public string ValueOverride { get; set; }
	}
}

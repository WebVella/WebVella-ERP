using Npgsql;
using NpgsqlTypes;
using System;

namespace WebVella.Erp.Eql
{
	public class EqlParameter
	{
		public string ParameterName { get; private set; }

		public object Value { get; internal set; }

		public EqlParameter(string name, object value)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException(nameof(name));
			
			if( !name.StartsWith("@"))
				ParameterName = "@" + name;
			else
				ParameterName = name;

			Value = value;
		}

		internal NpgsqlParameter ToNpgsqlParameter()
		{
			if (Value is DateTime && ((DateTime)Value).Kind == DateTimeKind.Utc)
			{
				var par = new NpgsqlParameter(ParameterName, NpgsqlDbType.TimestampTz);
				par.Value = Value;
				return par;
			}
			else if ( Value != null && Value is DateTime? && ((DateTime?)Value).Value.Kind == DateTimeKind.Utc)
			{
				var par = new NpgsqlParameter(ParameterName, NpgsqlDbType.TimestampTz);
				par.Value = Value;
				return par;
			}
			else
			{
				return new NpgsqlParameter(ParameterName, (object)Value ?? DBNull.Value);
			}
		}
	}
}

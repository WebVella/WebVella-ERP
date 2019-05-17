using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;

namespace WebVella.Erp.Eql
{
	public class EqlParameter
	{

		[JsonProperty(PropertyName = "name")]
		public string ParameterName { get; private set; }

		[JsonProperty(PropertyName = "value")]
		public object Value { get; internal set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; } = null;

		public EqlParameter(string name, object value) : this(name,value,null)
		{
		}

		public EqlParameter(string name, object value, string type )
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException(nameof(name));
			
			if( !name.StartsWith("@"))
				ParameterName = "@" + name;
			else
				ParameterName = name;

			Value = value;
			Type = type;
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

				if (Value != null)
					return new NpgsqlParameter(ParameterName, Value);
				else
				{
					var par = new NpgsqlParameter(ParameterName, ConvertToNpgsqlType(Type) );
					par.Value = DBNull.Value;
					return par;
				}
			}
		}

		private static NpgsqlDbType ConvertToNpgsqlType(string type)
		{
			if (type == "text")
				return NpgsqlDbType.Text;

			if (type == "bool" )
				return NpgsqlDbType.Boolean;
			
			if (type == "date")
				return NpgsqlDbType.TimestampTz;

			if (type == "int")
				return NpgsqlDbType.Integer;

			if (type == "decimal")
				return NpgsqlDbType.Numeric;

			if (type == "guid")
				return NpgsqlDbType.Uuid;

			return NpgsqlDbType.Text;
		}
	}
}

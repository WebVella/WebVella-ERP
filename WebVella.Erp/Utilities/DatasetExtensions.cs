using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Utilities;

namespace System.Data
{
	public static class DatasetExtensions
	{
		public static List<EntityRecord> AsRecordList(this DataTable table)
		{
			List<EntityRecord> result = new List<EntityRecord>();

			foreach (DataRow row in table.Rows)
			{
				EntityRecord record = new EntityRecord();
				foreach (DataColumn column in table.Columns)
				{
					if(row[column.ColumnName] is DateTime)
					{
						DateTime? date = row[column.ColumnName] as DateTime?;
						if (date != null && date.Value.Kind != DateTimeKind.Utc )
						{
							//because this later will be compared with EntityRecord
							//loaded with RecordManager, date should be always UTC to match
							date = date.Value.ToUniversalTime();

						}
						record[column.ColumnName] = date;

					}
					else
						record[column.ColumnName] = row[column.ColumnName];
				}
				result.Add(record);
			}
			return result;
		}

		public static Dictionary<Guid, EntityRecord> AsRecordDictionary(this List<EntityRecord> list)
		{
			Dictionary<Guid, EntityRecord> result = new Dictionary<Guid, EntityRecord>();

			foreach (EntityRecord rec in list)
				result.Add((Guid)rec["id"], rec);

			return result;
		}

		public static string DataRowToHash(this DataRow row)
		{
			StringBuilder result = new StringBuilder();

			foreach (DataColumn column in row.Table.Columns)
			{
				if (row[column.ColumnName] == null || row[column.ColumnName] == DBNull.Value)
				{
					result.Append(column.ColumnName.ToString() + "NULL");
				}
				else
				{
					result.Append(column.ColumnName.ToString() + row[column.ColumnName].ToString());
				}
			}

			return CryptoUtility.ComputeMD5Hash(result.ToString());
		}
	}
}

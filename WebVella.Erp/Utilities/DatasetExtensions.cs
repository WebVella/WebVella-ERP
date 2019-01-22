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
					record[column.ColumnName] = row[column.ColumnName];
				}
				result.Add(record);
			}
			return result;
		}

		public static string DataRowToHash(this DataRow row) 
		{
			StringBuilder result = new StringBuilder();

			foreach (DataColumn column in row.Table.Columns)
			{
				if(row[column.ColumnName] == null || row[column.ColumnName] == DBNull.Value)
				{
					result.Append(column.ColumnName.ToString() + "NULL");
				}
				else {
					result.Append(column.ColumnName.ToString() + row[column.ColumnName].ToString());
				}
			}

			return CryptoUtility.ComputeMD5Hash(result.ToString());
		}
	}
}

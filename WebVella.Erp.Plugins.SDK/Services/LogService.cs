using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Plugins.SDK.Services
{
    public class LogService
    {
        public void ClearJobAndErrorLogs()
        {
            //clear system logs older than 30 days and if there is more than 1000 records
            string logSql = "SELECT id, created_on FROM system_log ORDER BY created_on ASC";
            var logTable = ExecuteQuerySqlCommand(logSql);
            var logRows = logTable.Rows;
            DateTime logTreshold = DateTime.UtcNow.AddDays(-30);
            if (logRows.Count > 1000 && (DateTime)logRows[0]["created_on"] < logTreshold)
            {
                var logsToDelete = logRows.OfType<DataRow>().OrderByDescending(r => r["created_on"]).Select(r => (Guid)r["id"]).Skip(1000).ToList();
                foreach (var logId in logsToDelete)
                {
                    string deleteSql = $"DELETE FROM system_log WHERE id = @id";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                    parameters.Add(new NpgsqlParameter("id", logId) { NpgsqlDbType = NpgsqlDbType.Uuid });
                    ExecuteNonQuerySqlCommand(deleteSql, parameters);
                }
            }

            //clear Canceled, Failed, Finished and Aborted jobs older than 30 days and if there is more than 1000 records
            string sql = "SELECT id, created_on FROM jobs WHERE status = 3 OR status = 4 OR status = 5 OR status = 6 ORDER BY created_on ASC";
            var jobTable = ExecuteQuerySqlCommand(sql);
            var jobRows = jobTable.Rows;
            DateTime jobTreshold = DateTime.UtcNow.AddDays(-30);
            if (jobRows.Count > 1000 && (DateTime)jobRows[0]["created_on"] < jobTreshold)
            {
                var jobsToDelete = jobRows.OfType<DataRow>().OrderByDescending(r => r["created_on"]).Select(r => (Guid)r["id"]).Skip(1000).ToList();
                foreach (var jobId in jobsToDelete)
                {
                    string deleteSql = $"DELETE FROM jobs WHERE id = @id";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                    parameters.Add(new NpgsqlParameter("id", jobId) { NpgsqlDbType = NpgsqlDbType.Uuid });
                    ExecuteNonQuerySqlCommand(deleteSql, parameters);
                }
            }
        }

        public void ClearJobLogs()
        {
            //clear Canceled, Failed, Finished and Aborted jobs older than 30 days and if there is more than 1000 records
            string sql = "SELECT id, created_on FROM jobs WHERE status = 3 OR status = 4 OR status = 5 OR status = 6";
            var jobTable = ExecuteQuerySqlCommand(sql);
            var jobRows = jobTable.Rows;
            var jobsToDelete = jobRows.OfType<DataRow>().Select(r => (Guid)r["id"]).ToList();
            foreach (var jobId in jobsToDelete)
            {
                string deleteSql = $"DELETE FROM jobs WHERE id = @id";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                parameters.Add(new NpgsqlParameter("id", jobId) { NpgsqlDbType = NpgsqlDbType.Uuid });
                ExecuteNonQuerySqlCommand(deleteSql, parameters);
            }
        }

        public void ClearErrorLogs()
        {
            //clear system logs older than 30 days and if there is more than 1000 records
            string logSql = "SELECT id FROM system_log";
            var logTable = ExecuteQuerySqlCommand(logSql);
            var logRows = logTable.Rows;

            var logsToDelete = logRows.OfType<DataRow>().Select(r => (Guid)r["id"]).ToList();
            foreach (var logId in logsToDelete)
            {
                string deleteSql = $"DELETE FROM system_log WHERE id = @id";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                parameters.Add(new NpgsqlParameter("id", logId) { NpgsqlDbType = NpgsqlDbType.Uuid });
                ExecuteNonQuerySqlCommand(deleteSql, parameters);
            }

        }


        #region << Helper methods >>

        private bool ExecuteNonQuerySqlCommand(string sql, List<NpgsqlParameter> parameters = null)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sql, con);
                    command.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Count > 0)
                        command.Parameters.AddRange(parameters.ToArray());
                    return command.ExecuteNonQuery() > 0;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private DataTable ExecuteQuerySqlCommand(string sql, List<NpgsqlParameter> parameters = null)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sql, con);
                    command.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Count > 0)
                        command.Parameters.AddRange(parameters.ToArray());

                    DataTable resultTable = new DataTable();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(resultTable);
                    return resultTable;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion
    }
}

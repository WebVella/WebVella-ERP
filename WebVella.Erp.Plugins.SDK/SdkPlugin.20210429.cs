using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Npgsql;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;

namespace WebVella.Erp.Plugins.SDK
{
    public partial class SdkPlugin : ErpPlugin
    {
        private static void Patch20210429(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
        {
            List<Entity> entities = entMan.ReadEntities().Object;
            List<Field> fields = new List<Field>();

            using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT table_name, column_name, column_default FROM information_schema.columns WHERE table_schema = 'public' and column_default = 'now()'", con);
                    DataTable dt = new DataTable();
                    new NpgsqlDataAdapter(command).Fill(dt);
                    var records = dt.AsRecordList();
                    foreach (var rec in records)
                    {
                        string entityName = ((string)rec["table_name"]).Substring(4);
                        string fieldName = (string)rec["column_name"];
                        var entity = entities.SingleOrDefault(x => x.Name == entityName);
                        if (entity != null)
                            fields.Add(entity.Fields.Single(x => x.Name == fieldName));
                    }
                }
                finally
                {
                    con.Close();
                }
            }


            foreach (Field field in fields)
            {
                bool overrideNulls = field.Required && field.GetFieldDefaultValue() != null;
                DbRepository.SetColumnDefaultValue("rec_" + field.EntityName, field, overrideNulls);
            }
        }
    }
}

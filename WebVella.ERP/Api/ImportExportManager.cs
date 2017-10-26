using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database;
using WebVella.ERP.WebHooks;

namespace WebVella.ERP.Api
{
    public class ImportExportManager
    {
        private const char RELATION_SEPARATOR = '.';
        private const char RELATION_NAME_RESULT_SEPARATOR = '$';
        RecordManager recMan;
        EntityManager entMan;
        EntityRelationManager relMan;
        SecurityManager secMan;
        IWebHookService hooksService;

        public ImportExportManager(IWebHookService hooksService)
        {
            recMan = new RecordManager();
            secMan = new SecurityManager();
            entMan = new EntityManager();
            relMan = new EntityRelationManager();
            this.hooksService = hooksService;
        }

        public ResponseModel ExportListRecordsToCsv(string entityName, string listName, Dictionary<string, string> queries, int count = 10)
        {
            ResponseModel response = new ResponseModel();
            response.Message = "Records successfully exported";
            response.Timestamp = DateTime.UtcNow;
            response.Success = true;
            response.Object = null;

            EntityListResponse entitiesResponse = entMan.ReadEntities();
            List<Entity> entities = entitiesResponse.Object;
            Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

            if (entity == null)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "Export failed! Entity with such name does not exist!";
                response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
                return response;
            }

            bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Read, entity);
            if (!hasPermisstion)
            {
                response.Success = false;
                response.Message = "Export failed! Trying to read records from entity '" + entity.Name + "' with no read access.";
                response.Errors.Add(new ErrorModel { Message = "Access denied." });
                return response;
            }

            List<EntityRelation> relations = new List<EntityRelation>();
            var relationResponse = relMan.Read();
            if (relationResponse.Success)
                relations = relationResponse.Object;

            try
            {
                RecordList listMeta = entity.RecordLists.FirstOrDefault(l => l.Name == listName);

                QueryObject queryObj = null;
                if (queries.Count > 0)
                {
                    List<QueryObject> queryObjList = new List<QueryObject>();

                    if (listMeta != null)
                    {
                        foreach (var query in queries)
                        {
                            if (listMeta.Columns.Any(c => c.DataName == query.Key))
                            {
                                queryObjList.Add(EntityQuery.QueryContains(query.Key, query.Value));
                            }
                        }
                    }

                    if (queryObjList.Count == 1)
                        queryObj = queryObjList[0];
                    else if (queryObjList.Count > 1)
                        queryObj = EntityQuery.QueryAND(queryObjList.ToArray());
                }

                var stream = new MemoryStream();
                var textWriter = new StreamWriter(stream);

                List<EntityRecord> records = null;
                if (count == -1)
                    records = recMan.GetListRecords(entities, entity, listName, null, queryObj, null, true, returnAllRecords: true);
                else
                    records = recMan.GetListRecords(entities, entity, listName, 1, queryObj, count, true);

                if (records.Count > 0)
                {
                    var csv = new CsvWriter(textWriter);
                    csv.Configuration.QuoteAllFields = true;

                    //write header
                    foreach (var prop in records[0].Properties)
                    {
                        var listItem = listMeta.Columns.FirstOrDefault(c => c.DataName == prop.Key);
                        if (prop.Key.StartsWith("$field") && listItem == null)
                            continue;// remove id field from relation that are not inserted as columns
                        string name = prop.Key;
                        if (prop.Key.StartsWith("$field$"))
                        {
                            name = prop.Key.Remove(0, 7);
                            name = "$" + name.Replace('$', '.');
                        }
                        csv.WriteField(name);
                    }
                    csv.NextRecord();


                    foreach (var record in records)
                    {
                        foreach (var prop in record.Properties)
                        {
                            if (prop.Value != null)
                            {
                                if (prop.Value is List<object>)
                                {
                                    var listItem = (RecordListRelationFieldItem)listMeta.Columns.FirstOrDefault(c => c.DataName == prop.Key);
                                    if (prop.Key.StartsWith("$field") && listItem == null)
                                    {
                                        continue;// remove id field from relation that are not inserted as columns
                                    }
                                    var type = listItem != null ? listItem.Meta.GetFieldType() : FieldType.GuidField;
                                    if (prop.Key.StartsWith("$field"))
                                    {
                                        var relationData = prop.Key.Split('$').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                        var relation = relations.FirstOrDefault(r => r.Name == relationData[1]);
                                        if (relation.RelationType == EntityRelationType.ManyToMany ||
                                            (relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == entity.Id))
                                            csv.WriteField(JsonConvert.SerializeObject(prop.Value).ToString());
                                        else if (((List<object>)prop.Value).Count > 0)
                                            csv.WriteField(((List<object>)prop.Value)[0] ?? "");
                                        else
                                            csv.WriteField("");
                                    }
                                    else if (type != FieldType.MultiSelectField && type != FieldType.TreeSelectField)
                                    {
                                        if (((List<object>)prop.Value).Count > 0)
                                            csv.WriteField(((List<object>)prop.Value)[0]);
                                        else
                                            csv.WriteField("");
                                    }
                                    else
                                    {
                                        csv.WriteField(JsonConvert.SerializeObject(prop.Value).ToString());
                                    }
                                }
                                else
                                {
                                    csv.WriteField(prop.Value);
                                }
                            }
                            else
                                csv.WriteField("");
                        }
                        csv.NextRecord();
                        textWriter.Flush();
                    }
                }

                byte[] buffer = stream.ToArray();

                response.Object = buffer;

                textWriter.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public ResponseModel ImportEntityRecordsFromCsv(string entityName, string fileTempPath)
        {
            //The import CSV should have column names matching the names of the imported fields. The first column should be "id" matching the id of the record to be updated. 
            //If the 'id' of a record equals 'null', a new record will be created with the provided columns and default values for the missing ones.

            ResponseModel response = new ResponseModel();
            response.Message = "Records successfully imported";
            response.Timestamp = DateTime.UtcNow;
            response.Success = true;
            response.Object = null;

            //string fileTempPath = @"D:\csv\test.csv";
            //FileInfo fileInfo = new FileInfo(fileTempPath);

            //if (!fileInfo.Exists)
            //	throw new Exception("FILE_NOT_EXIST");

            //FileStream fileStream = fileInfo.OpenRead();
            //TextReader reader = new StreamReader(fileStream);

            if (string.IsNullOrWhiteSpace(fileTempPath))
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "Import failed! fileTempPath parameter cannot be empty or null!";
                response.Errors.Add(new ErrorModel("fileTempPath", fileTempPath, "Import failed! File does not exist!"));
                return response;
            }

            if (fileTempPath.StartsWith("/fs"))
                fileTempPath = fileTempPath.Remove(0, 3);

            if (!fileTempPath.StartsWith("/"))
                fileTempPath = "/" + fileTempPath;

            fileTempPath = fileTempPath.ToLowerInvariant();

            using (DbConnection connection = DbContext.Current.CreateConnection())
            {
                List<EntityRelation> relations = relMan.Read().Object;
                EntityListResponse entitiesResponse = entMan.ReadEntities();
                List<Entity> entities = entitiesResponse.Object;
                Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

                if (entity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Import failed! Entity with such name does not exist!";
                    response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
                    return response;
                }

                DbFileRepository fs = new DbFileRepository();
                DbFile file = fs.Find(fileTempPath);

                if (file == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Import failed! File does not exist!";
                    response.Errors.Add(new ErrorModel("fileTempPath", fileTempPath, "Import failed! File does not exist!"));
                    return response;
                }

                byte[] fileBytes = file.GetBytes();
                MemoryStream fileStream = new MemoryStream(fileBytes);
                //fileStream.Write(fileBytes, 0, fileBytes.Length);
                //fileStream.Flush();
                TextReader reader = new StreamReader(fileStream);

                CsvReader csvReader = new CsvReader(reader);
                csvReader.Configuration.HasHeaderRecord = true;
                //csvReader.Configuration.IsHeaderCaseSensitive = false;

                csvReader.Read();

                csvReader.ReadHeader();
                var headerRecord = csvReader.GetRecord<dynamic>();
                List<string> columns = new List<string>();
                foreach (var name in headerRecord)
                {
                    columns.Add(name.Key.ToString());
                }

                List<dynamic> fieldMetaList = new List<dynamic>();

                foreach (var column in columns)
                {
                    Field field;
                    if (column.Contains(RELATION_SEPARATOR))
                    {
                        var relationData = column.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                        if (relationData.Count > 2)
                            throw new Exception(string.Format("The specified field name '{0}' is incorrect. Only first level relation can be specified.", column));

                        string relationName = relationData[0];
                        string relationFieldName = relationData[1];

                        if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
                            throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", column));
                        else if (!relationName.StartsWith("$"))
                            throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", column));
                        else
                            relationName = relationName.Substring(1);

                        //check for target priority mark $$
                        if (relationName.StartsWith("$"))
                        {
                            relationName = relationName.Substring(1);
                        }

                        if (string.IsNullOrWhiteSpace(relationFieldName))
                            throw new Exception(string.Format("Invalid relation '{0}'. The relation field name is not specified.", column));

                        var relation = relations.SingleOrDefault(x => x.Name == relationName);
                        if (relation == null)
                            throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", column));

                        if (relation.TargetEntityId != entity.Id && relation.OriginEntityId != entity.Id)
                            throw new Exception(string.Format("Invalid relation '{0}'. The relation field belongs to entity that does not relate to current entity.", column));

                        Entity relationEntity = null;

                        if (relation.OriginEntityId == entity.Id)
                        {
                            relationEntity = entities.FirstOrDefault(e => e.Id == relation.TargetEntityId);
                            field = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                        }
                        else
                        {
                            relationEntity = entities.FirstOrDefault(e => e.Id == relation.OriginEntityId);
                            field = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                        }
                    }
                    else
                    {
                        field = entity.Fields.FirstOrDefault(f => f.Name == column);
                    }

                    dynamic fieldMeta = new ExpandoObject();
                    fieldMeta.ColumnName = column;
                    fieldMeta.FieldType = field.GetFieldType();

                    fieldMetaList.Add(fieldMeta);
                }

                connection.BeginTransaction();

                try
                {
                    do
                    {
                        EntityRecord newRecord = new EntityRecord();
                        foreach (var fieldMeta in fieldMetaList)
                        {
                            string columnName = fieldMeta.ColumnName.ToString();
                            string value = csvReader.GetField<string>(columnName);

                            if (value.StartsWith("[") && value.EndsWith("]"))
                            {
                                newRecord[columnName] = JsonConvert.DeserializeObject<List<string>>(value);
                            }
                            else
                            {
                                switch ((FieldType)fieldMeta.FieldType)
                                {
                                    case FieldType.AutoNumberField:
                                    case FieldType.CurrencyField:
                                    case FieldType.NumberField:
                                    case FieldType.PercentField:
                                        {
                                            decimal decValue;
                                            if (decimal.TryParse(value, out decValue))
                                                newRecord[columnName] = decValue;
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    case FieldType.CheckboxField:
                                        {
                                            bool bValue;
                                            if (bool.TryParse(value, out bValue))
                                                newRecord[columnName] = bValue;
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    case FieldType.DateField:
                                    case FieldType.DateTimeField:
                                        {
                                            DateTime dtValue;
                                            if (DateTime.TryParse(value, out dtValue))
                                                newRecord[columnName] = dtValue;
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    case FieldType.MultiSelectField:
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                                newRecord[columnName] = new List<string>(new string[] { value });
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    case FieldType.TreeSelectField:
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                                newRecord[columnName] = new List<string>(new string[] { value });
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    case FieldType.GuidField:
                                        {
                                            Guid gValue;
                                            if (Guid.TryParse(value, out gValue))
                                                newRecord[columnName] = gValue;
                                            else
                                                newRecord[columnName] = null;
                                        }
                                        break;
                                    default:
                                        {
                                            newRecord[columnName] = value;
                                        }
                                        break;
                                }
                            }
                        }

                        QueryResponse result;
                        if (!newRecord.GetProperties().Any(x => x.Key == "id") || newRecord["id"] == null || string.IsNullOrEmpty(newRecord["id"].ToString()))
                        {
                            newRecord["id"] = Guid.NewGuid();
                            result = recMan.CreateRecord(entityName, newRecord);
                        }
                        else
                        {
                            result = recMan.UpdateRecord(entityName, newRecord);
                        }
                        if (!result.Success)
                        {
                            string message = result.Message;
                            if (result.Errors.Count > 0)
                            {
                                foreach (ErrorModel error in result.Errors)
                                    message += " " + error.Message;
                            }
                            throw new Exception(message);
                        }
                    } while (csvReader.Read());
                    connection.CommitTransaction();
                }
                catch (Exception e)
                {
                    connection.RollbackTransaction();

                    response.Success = false;
                    response.Object = null;
                    response.Timestamp = DateTime.UtcNow;
#if DEBUG
                    response.Message = e.Message + e.StackTrace;
#else
							response.Message = "Import failed! An internal error occurred!";
#endif
                }
                finally
                {
                    reader.Close();
                    fileStream.Close();
                }

                return response;
            }
        }

        public ResponseModel EvaluateImportEntityRecordsFromCsv(string entityName, JObject postObject, bool enableWebHooks = true, object controller = null)
        {
            ResponseModel response = new ResponseModel();
            response.Message = "Records successfully evaluated";
            response.Timestamp = DateTime.UtcNow;
            response.Success = true;
            response.Object = null;

            List<EntityRelation> relations = relMan.Read().Object;
            EntityListResponse entitiesResponse = entMan.ReadEntities();
            List<Entity> entities = entitiesResponse.Object;
            Entity entity = entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Entity not found";
                return response;
            }

            var entityFields = entity.Fields;
            string fileTempPath = "";
            string clipboard = "";
            string generalCommand = "evaluate";
            EntityRecord commands = new EntityRecord();
            if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "fileTempPath"))
            {
                fileTempPath = postObject["fileTempPath"].ToString();
            }

            if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "clipboard"))
            {
                clipboard = postObject["clipboard"].ToString();
            }

            if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "general_command"))
            {
                generalCommand = postObject["general_command"].ToString(); //could be "evaluate" & "evaluate-import" the first will just evaluate, the second one will evaluate and import if all is fine
            }

            if (!postObject.IsNullOrEmpty() && generalCommand == "evaluate-import" &&
                postObject.Properties().Any(p => p.Name == "commands") && !((JToken)postObject["commands"]).IsNullOrEmpty())
            {
                var commandsObject = postObject["commands"].Value<JObject>();
                if (!commandsObject.IsNullOrEmpty() && commandsObject.Properties().Any())
                {
                    foreach (var property in commandsObject.Properties())
                    {
                        commands[property.Name] = ((JObject)property.Value).ToObject<EntityRecord>();
                    }
                }
            }

            //VALIDATE:
            if (fileTempPath == "" && clipboard == "")
            {
                response.Success = false;
                response.Message = "Both clipboard and file CSV sources are empty!";
                return response;
            }

            CsvReader csvReader = null;
            string csvContent = "";
            bool usingClipboard = false;
            //CASE: 1 If fileTempPath != "" -> get the csv from the file
            if (fileTempPath != "")
            {
                if (fileTempPath.StartsWith("/fs"))
                    fileTempPath = fileTempPath.Remove(0, 3);

                if (!fileTempPath.StartsWith("/"))
                    fileTempPath = "/" + fileTempPath;

                fileTempPath = fileTempPath.ToLowerInvariant();

                DbFileRepository fs = new DbFileRepository();
                DbFile file = fs.Find(fileTempPath);

                if (file == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Import failed! File does not exist!";
                    response.Errors.Add(new ErrorModel("fileTempPath", fileTempPath, "Import failed! File does not exist!"));
                    return response;
                }

                byte[] fileBytes = file.GetBytes();
                MemoryStream fileStream = new MemoryStream(fileBytes);
                TextReader reader = new StreamReader(fileStream);
                csvReader = new CsvReader(reader);
            }
            //CASE: 2 If fileTempPath == "" -> get the csv from the clipboard
            else
            {
                csvContent = clipboard;
                usingClipboard = true;
                csvReader = new CsvReader(new StringReader(csvContent));
            }

            csvReader.Configuration.HasHeaderRecord = true;
            //csvReader.Configuration.IsHeaderCaseSensitive = false;
            if (usingClipboard)
            {
                csvReader.Configuration.Delimiter = "\t";
            }
            //The evaluation object has two properties - errors and warnings. Both are objects
            //The error validation object should return arrays by field name ex. {field_name:[null,null,"error message"]}
            //The warning validation object should return arrays by field name ex. {field_name:[null,null,"warning message"]}
            var evaluationObj = new EntityRecord();
            evaluationObj["errors"] = new EntityRecord();
            evaluationObj["warnings"] = new EntityRecord();
            evaluationObj["records"] = new List<EntityRecord>();
            evaluationObj["commands"] = new EntityRecord(); // the commands is object with properties the fieldNames and the following object as value {command: "to_create" | "no_import" | "to_update", fieldType: 14, fieldName: "name", fieldLabel: "label"}
            var statsObject = new EntityRecord();
            statsObject["to_create"] = 0;
            statsObject["no_import"] = 0;
            statsObject["to_update"] = 0;
            statsObject["errors"] = 0;
            statsObject["warnings"] = 0;
            evaluationObj["stats"] = statsObject;
            evaluationObj["general_command"] = generalCommand;

            csvReader.Read();

            csvReader.ReadHeader();
            var headerRecord = csvReader.GetRecord<dynamic>();
            List<string> columnNames = new List<string>();
            foreach (var name in headerRecord)
            {
                columnNames.Add(name.Key.ToString());
            }

            foreach (var columnName in columnNames)
            {
                //Init the error list for this field
                if (!((EntityRecord)evaluationObj["errors"]).GetProperties().Any(p => p.Key == columnName))
                {
                    ((EntityRecord)evaluationObj["errors"])[columnName] = new List<string>();
                }
                //Init the warning list for this field
                var warningList = new List<string>();
                if (!((EntityRecord)evaluationObj["warnings"]).GetProperties().Any(p => p.Key == columnName))
                {
                    ((EntityRecord)evaluationObj["warnings"])[columnName] = new List<string>();
                }

                bool existingField = false;

                Field currentFieldMeta = null;
                Field relationEntityFieldMeta = null;
                Field relationFieldMeta = null;
                Entity relationEntity = null;
                string direction = "origin-target";
                EntityRelationType relationType = EntityRelationType.OneToMany;
                string fieldEnityName = entity.Name;
                string fieldRelationName = string.Empty;

                if (!commands.GetProperties().Any(p => p.Key == columnName))
                {
                    commands[columnName] = new EntityRecord();
                    ((EntityRecord)commands[columnName])["command"] = "no_import";
                    ((EntityRecord)commands[columnName])["entityName"] = fieldEnityName;
                    ((EntityRecord)commands[columnName])["fieldType"] = FieldType.TextField;
                    ((EntityRecord)commands[columnName])["fieldName"] = columnName;
                    ((EntityRecord)commands[columnName])["fieldLabel"] = columnName;
                }

                if (columnName.Contains(RELATION_SEPARATOR))
                {
                    var relationData = columnName.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    if (relationData.Count > 2)
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("The specified field name '{0}' is incorrect. Only first level relation can be specified.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    string relationName = relationData[0];
                    string relationFieldName = relationData[1];

                    if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation name is not specified.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }
                    else if (!relationName.StartsWith("$"))
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation name is not correct.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }
                    else
                        relationName = relationName.Substring(1);

                    //check for target priority mark $$
                    if (relationName.StartsWith("$"))
                    {
                        relationName = relationName.Substring(1);
                        direction = "target-origin";
                    }

                    if (string.IsNullOrWhiteSpace(relationFieldName))
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation field name is not specified.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    var relation = relations.SingleOrDefault(x => x.Name == relationName);
                    if (relation == null)
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation does not exist.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    if (relation.TargetEntityId != entity.Id && relation.OriginEntityId != entity.Id)
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation field belongs to entity that does not relate to current entity.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    if (relation.OriginEntityId == relation.TargetEntityId)
                    {
                        if (direction == "origin-target")
                        {
                            relationEntity = entity;
                            relationEntityFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
                            currentFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                            relationFieldMeta = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
                        }
                        else
                        {
                            relationEntity = entity;
                            relationEntityFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
                            currentFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                            relationFieldMeta = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
                        }
                    }
                    else if (relation.OriginEntityId == entity.Id)
                    {
                        //direction doesn't matter
                        relationEntity = entities.FirstOrDefault(e => e.Id == relation.TargetEntityId);
                        relationEntityFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
                        currentFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                        relationFieldMeta = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
                    }
                    else
                    {
                        //direction doesn't matter
                        relationEntity = entities.FirstOrDefault(e => e.Id == relation.OriginEntityId);
                        relationEntityFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
                        currentFieldMeta = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
                        relationFieldMeta = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
                    }

                    if (currentFieldMeta == null)
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. Fields with such name does not exist.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    if (currentFieldMeta.GetFieldType() == FieldType.MultiSelectField || currentFieldMeta.GetFieldType() == FieldType.TreeSelectField)
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. Fields from Multiselect and Treeselect types can't be used as relation fields.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    if (relation.RelationType == EntityRelationType.OneToOne &&
                        ((relation.TargetEntityId == entity.Id && relationFieldMeta.Name == "id") || (relation.OriginEntityId == entity.Id && relationEntityFieldMeta.Name == "id")))
                    {
                        ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. Can't use relations when relation field is id field.", columnName));
                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        continue;
                    }

                    fieldEnityName = relationEntity.Name;
                    fieldRelationName = relationName;
                    relationType = relation.RelationType;
                }
                else
                {
                    currentFieldMeta = entity.Fields.FirstOrDefault(f => f.Name == columnName);
                }

                if (currentFieldMeta != null)
                {
                    existingField = true;
                }

                if (!existingField && !string.IsNullOrWhiteSpace(fieldRelationName))
                {
                    ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Creation of a new relation field is not allowed.", columnName));
                    ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                }

                #region << Commands >>
                //we need to init the command for this column - if it is new field the default is do nothing, if it is existing the default is update
                if (existingField)
                {
                    if (generalCommand == "evaluate")
                    {
                        ((EntityRecord)commands[columnName])["command"] = "to_update";
                        ((EntityRecord)commands[columnName])["relationName"] = fieldRelationName;
                        ((EntityRecord)commands[columnName])["relationDirection"] = direction;
                        ((EntityRecord)commands[columnName])["relationType"] = relationType;
                        ((EntityRecord)commands[columnName])["entityName"] = fieldEnityName;
                        ((EntityRecord)commands[columnName])["fieldType"] = currentFieldMeta.GetFieldType();
                        ((EntityRecord)commands[columnName])["fieldName"] = currentFieldMeta.Name;
                        ((EntityRecord)commands[columnName])["fieldLabel"] = currentFieldMeta.Label;

                        bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Update, entity);
                        if (!hasPermisstion)
                        {
                            ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add($"Access denied. Trying to update record in entity '{entity.Name}' with no update access.");
                            ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        }
                    }

                    ((EntityRecord)commands[columnName])["currentFieldMeta"] = currentFieldMeta;
                    ((EntityRecord)commands[columnName])["relationEntityFieldMeta"] = relationEntityFieldMeta;
                    ((EntityRecord)commands[columnName])["relationFieldMeta"] = relationFieldMeta;
                }
                else
                {
                    if (generalCommand == "evaluate")
                    {
                        //we need to check wheather the property of the command match the fieldName
                        ((EntityRecord)commands[columnName])["command"] = "to_create";
                        ((EntityRecord)commands[columnName])["entityName"] = fieldEnityName;
                        ((EntityRecord)commands[columnName])["fieldType"] = FieldType.TextField;
                        ((EntityRecord)commands[columnName])["fieldName"] = columnName;
                        ((EntityRecord)commands[columnName])["fieldLabel"] = columnName;

                        bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Create, entity);
                        if (!hasPermisstion)
                        {
                            ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add($"Access denied. Trying to create record in entity '{entity.Name}' with no create access.");
                            ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        }
                    }
                }
                #endregion
            }

            evaluationObj["commands"] = commands;

            while (csvReader.Read())
            {
                Dictionary<string, EntityRecord> fieldsFromRelationList = new Dictionary<string, EntityRecord>();
                Dictionary<string, string> rowRecordData = new Dictionary<string, string>();

                foreach (var columnName in columnNames)
                {
                    string fieldValue = csvReader.GetField<string>(columnName);
                    rowRecordData[columnName] = fieldValue;

                    EntityRecord commandRecords = ((EntityRecord)commands[columnName]);
                    Field currentFieldMeta = new TextField();
                    if (commandRecords.GetProperties().Any(p => p.Key == "currentFieldMeta"))
                        currentFieldMeta = (Field)commandRecords["currentFieldMeta"];

                    if (columnName.Contains(RELATION_SEPARATOR))
                    {
                        string relationName = (string)((EntityRecord)commands[columnName])["relationName"];
                        string relationDirection = (string)((EntityRecord)commands[columnName])["relationDirection"];
                        string relationEntityName = (string)((EntityRecord)commands[columnName])["entityName"];

                        EntityRelationType relationType = (EntityRelationType)Enum.Parse(typeof(EntityRelationType), (((EntityRecord)commands[columnName])["relationType"]).ToString());
                        Field relationEntityFieldMeta = (Field)((EntityRecord)commands[columnName])["relationEntityFieldMeta"];
                        Field relationFieldMeta = (Field)((EntityRecord)commands[columnName])["relationFieldMeta"];

                        var relation = relations.SingleOrDefault(x => x.Name == relationName);

                        string relationFieldValue = "";
                        if (columnNames.Any(c => c == relationFieldMeta.Name))
                            relationFieldValue = csvReader.GetField<string>(relationFieldMeta.Name);

                        QueryObject filter = null;
                        if ((relationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && relationDirection == "origin-target") ||
                            (relationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId == entity.Id) ||
                            relationType == EntityRelationType.ManyToMany)
                        {
                            //expect array of values
                            if (!columnNames.Any(c => c == relationFieldMeta.Name) || string.IsNullOrEmpty(relationFieldValue))
                            {
                                ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", columnName));
                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                            }

                            List<string> values = new List<string>();
                            if (relationFieldValue.StartsWith("[") && relationFieldValue.EndsWith("]"))
                            {
                                values = JsonConvert.DeserializeObject<List<string>>(relationFieldValue);
                            }
                            if (values.Count < 1)
                                continue;

                            List<QueryObject> queries = new List<QueryObject>();
                            foreach (var val in values)
                            {
                                queries.Add(EntityQuery.QueryEQ(currentFieldMeta.Name, val));
                            }

                            filter = EntityQuery.QueryOR(queries.ToArray());
                        }
                        else
                        {
                            filter = EntityQuery.QueryEQ(currentFieldMeta.Name, DbRecordRepository.ExtractFieldValue(fieldValue, currentFieldMeta, true));
                        }

                        EntityRecord fieldsFromRelation = new EntityRecord();

                        if (fieldsFromRelationList.Any(r => r.Key == relation.Name))
                        {
                            fieldsFromRelation = fieldsFromRelationList[relationName];
                        }
                        else
                        {
                            fieldsFromRelation["columns"] = new List<string>();
                            fieldsFromRelation["queries"] = new List<QueryObject>();
                            fieldsFromRelation["direction"] = relationDirection;
                            fieldsFromRelation["relationEntityName"] = relationEntityName;
                        }

                        ((List<string>)fieldsFromRelation["columns"]).Add(columnName);
                        ((List<QueryObject>)fieldsFromRelation["queries"]).Add(filter);
                        fieldsFromRelationList[relationName] = fieldsFromRelation;
                    }
                }

                foreach (var fieldsFromRelation in fieldsFromRelationList)
                {
                    EntityRecord fieldsFromRelationValue = fieldsFromRelation.Value;
                    List<string> columnList = (List<string>)fieldsFromRelationValue["columns"];
                    List<QueryObject> queries = (List<QueryObject>)fieldsFromRelationValue["queries"];
                    string relationDirection = (string)fieldsFromRelationValue["direction"];
                    string relationEntityName = (string)fieldsFromRelationValue["relationEntityName"];
                    QueryObject filter = EntityQuery.QueryAND(queries.ToArray());

                    var relation = relations.SingleOrDefault(r => r.Name == fieldsFromRelation.Key);

                    //get related records
                    QueryResponse relatedRecordResponse = recMan.Find(new EntityQuery(relationEntityName, "*", filter, null, null, null));

                    if (!relatedRecordResponse.Success || relatedRecordResponse.Object.Data.Count < 1)
                    {
                        foreach (var columnName in columnList)
                        {
                            ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. The relation record does not exist.", columnName));
                            ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        }
                    }
                    else if (relatedRecordResponse.Object.Data.Count > 1 && ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && relationDirection == "target-origin") ||
                        (relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.TargetEntityId == entity.Id) ||
                        relation.RelationType == EntityRelationType.OneToOne))
                    {
                        //there can be no more than 1 records
                        foreach (var columnName in columnList)
                        {
                            ((List<string>)((EntityRecord)evaluationObj["errors"])[columnName]).Add(string.Format("Invalid relation '{0}'. There are multiple relation records matching this value.", columnName));
                            ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                        }
                    }

                    fieldsFromRelationList[fieldsFromRelation.Key]["relatedRecordResponse"] = relatedRecordResponse;
                }

                var rowRecord = new EntityRecord();
                if ((int)statsObject["errors"] == 0)
                {
                    foreach (var columnName in columnNames)
                    {
                        string fieldValue = rowRecordData[columnName];
                        EntityRecord commandRecords = ((EntityRecord)commands[columnName]);
                        Field currentFieldMeta = new TextField();
                        if (commandRecords.GetProperties().Any(p => p.Key == "currentFieldMeta"))
                            currentFieldMeta = (Field)commandRecords["currentFieldMeta"];
                        string fieldEnityName = (string)commandRecords["entityName"];
                        string command = (string)commandRecords["command"];

                        bool existingField = false;
                        if (command == "to_update")
                            existingField = true;

                        if (existingField)
                        {
                            #region << Validation >>

                            var errorsList = (List<string>)((EntityRecord)evaluationObj["errors"])[columnName];
                            var warningList = (List<string>)((EntityRecord)evaluationObj["warnings"])[columnName];

                            if (columnName.Contains(RELATION_SEPARATOR))
                            {
                                string relationName = (string)((EntityRecord)commands[columnName])["relationName"];
                                string relationDirection = (string)((EntityRecord)commands[columnName])["relationDirection"];
                                string relationEntityName = (string)((EntityRecord)commands[columnName])["entityName"];

                                EntityRelationType relationType = (EntityRelationType)Enum.Parse(typeof(EntityRelationType), (((EntityRecord)commands[columnName])["relationType"]).ToString());
                                Field relationEntityFieldMeta = (Field)((EntityRecord)commands[columnName])["relationEntityFieldMeta"];
                                Field relationFieldMeta = (Field)((EntityRecord)commands[columnName])["relationFieldMeta"];

                                var relation = relations.SingleOrDefault(x => x.Name == relationName);

                                QueryResponse relatedRecordResponse = (QueryResponse)fieldsFromRelationList[relationName]["relatedRecordResponse"];

                                var relatedRecords = relatedRecordResponse.Object.Data;
                                List<Guid> relatedRecordValues = new List<Guid>();
                                foreach (var relatedRecord in relatedRecords)
                                {
                                    relatedRecordValues.Add((Guid)relatedRecord[relationEntityFieldMeta.Name]);
                                }

                                string relationFieldValue = "";
                                if (columnNames.Any(c => c == relationFieldMeta.Name))
                                    relationFieldValue = rowRecordData[relationFieldMeta.Name];

                                if (relation.RelationType == EntityRelationType.OneToOne &&
                                    ((relation.OriginEntityId == relation.TargetEntityId && relationDirection == "origin-target") || relation.OriginEntityId == entity.Id))
                                {
                                    if (!columnNames.Any(c => c == relationFieldMeta.Name) || string.IsNullOrWhiteSpace(relationFieldValue))
                                    {
                                        errorsList.Add(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", columnName));
                                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                    }
                                }
                                else if (relation.RelationType == EntityRelationType.OneToMany &&
                                    ((relation.OriginEntityId == relation.TargetEntityId && relationDirection == "origin-target") || relation.OriginEntityId == entity.Id))
                                {
                                    if (!columnNames.Any(c => c == relationFieldMeta.Name) || string.IsNullOrWhiteSpace(relationFieldValue))
                                    {
                                        errorsList.Add(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", columnName));
                                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                    }
                                }
                                else if (relation.RelationType == EntityRelationType.ManyToMany)
                                {
                                    foreach (Guid relatedRecordIdValue in relatedRecordValues)
                                    {
                                        Guid relRecordId = Guid.Empty;
                                        if (!Guid.TryParse(relationFieldValue, out relRecordId))
                                        {
                                            errorsList.Add("Invalid record value for field: '" + columnName + "'. Invalid value: '" + fieldValue + "'");
                                            ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                        }
                                    }
                                }

                            }
                            if (string.IsNullOrWhiteSpace(fieldValue))
                            {
                                if (currentFieldMeta.Required && currentFieldMeta.Name != "id")
                                {
                                    errorsList.Add("Field is required. Value can not be empty!");
                                    ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                }
                            }
                            else if (!(fieldValue.StartsWith("[") && fieldValue.EndsWith("]")))
                            {
                                FieldType fType = (FieldType)currentFieldMeta.GetFieldType();
                                switch (fType)
                                {
                                    case FieldType.AutoNumberField:
                                    case FieldType.CurrencyField:
                                    case FieldType.NumberField:
                                    case FieldType.PercentField:
                                        {
                                            decimal decValue;
                                            if (!decimal.TryParse(fieldValue, out decValue))
                                            {
                                                errorsList.Add("Value have to be of decimal type!");
                                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                            }
                                        }
                                        break;
                                    case FieldType.CheckboxField:
                                        {
                                            bool bValue;
                                            if (!bool.TryParse(fieldValue, out bValue))
                                            {
                                                errorsList.Add("Value have to be of boolean type!");
                                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                            }
                                        }
                                        break;
                                    case FieldType.DateField:
                                    case FieldType.DateTimeField:
                                        {
                                            DateTime dtValue;
                                            if (!DateTime.TryParse(fieldValue, out dtValue))
                                            {
                                                errorsList.Add("Value have to be of datetime type!");
                                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                            }
                                        }
                                        break;
                                    case FieldType.MultiSelectField:
                                        {

                                        }
                                        break;
                                    case FieldType.SelectField:
                                        {
                                            if (!((SelectField)currentFieldMeta).Options.Any(o => o.Key == fieldValue))
                                            {
                                                errorsList.Add("Value does not exist in select field options!");
                                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                            }
                                        }
                                        break;
                                    case FieldType.TreeSelectField:
                                        {

                                        }
                                        break;
                                    case FieldType.GuidField:
                                        {
                                            Guid gValue;
                                            if (!Guid.TryParse(fieldValue, out gValue))
                                            {
                                                errorsList.Add("Value have to be of guid type!");
                                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                            }

                                        }
                                        break;
                                }
                            }

                            ((EntityRecord)evaluationObj["errors"])[columnName] = errorsList;

                            //validate the value for warnings
                            ((EntityRecord)evaluationObj["warnings"])[columnName] = warningList;
                            #endregion
                        }

                        #region << Data >>
                        //Submit row data

                        if (!(command == "no_import" && generalCommand == "evaluate-import"))
                            rowRecord[columnName] = fieldValue;
                        #endregion
                    }

                    if (enableWebHooks && generalCommand == "evaluate-import")
                    {
                        #region << WebHook Filters >>

                        Guid? recordId = null;
                        if (rowRecord.GetProperties().Any(p => p.Key == "id") && !string.IsNullOrWhiteSpace((string)rowRecord["id"]))
                        {
                            Guid id;
                            if (Guid.TryParse((string)rowRecord["id"], out id))
                                recordId = id;
                        }
                        //////////////////////////////////////////////////////////////////////////////////////
                        //WEBHOOK FILTERS << create_record_input_filter >> AND << update_record_input_filter >>
                        //////////////////////////////////////////////////////////////////////////////////////
                        #region
                        try
                        {
                            dynamic hookFilterObj = new ExpandoObject();
                            hookFilterObj.record = rowRecord;
                            hookFilterObj.recordId = recordId;
                            hookFilterObj.controller = controller;
                            string webHookName = (!recordId.HasValue || recordId.Value == Guid.Empty) ? SystemWebHookNames.CreateRecordInput : SystemWebHookNames.UpdateRecordInput;
                            hookFilterObj = hooksService.ProcessFilters(webHookName, entityName, hookFilterObj);
                            rowRecord = hookFilterObj.record;
                        }
                        catch (Exception ex)
                        {
                            response.Success = false;
                            response.Object = evaluationObj;
                            response.Timestamp = DateTime.UtcNow;
                            response.Message = (!recordId.HasValue || recordId.Value == Guid.Empty) ?
                                "Plugin error in web hook create_record_input_filter: " + ex.Message :
                                "Plugin error in web hook update_record_input_filter: " + ex.Message;
                            return response;
                            //throw new Exception("Plugin error in web hook update_record_input_filter: " + ex.Message);
                        }// <<<	
                        #endregion

                        var validationErrors = new List<ErrorModel>();

                        //////////////////////////////////////////////////////////////////////////////////////
                        //WEBHOOK FILTER << update_record_validation_errors_filter >>
                        //////////////////////////////////////////////////////////////////////////////////////
                        #region
                        try
                        {
                            dynamic hookFilterObj = new ExpandoObject();
                            hookFilterObj.errors = validationErrors;
                            hookFilterObj.record = rowRecord;
                            hookFilterObj.recordId = recordId;
                            hookFilterObj.controller = controller;
                            string webHookName = (!recordId.HasValue || recordId.Value == Guid.Empty) ? SystemWebHookNames.CreateRecordValidationErrors : SystemWebHookNames.UpdateRecordValidationErrors;
                            hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.UpdateRecordValidationErrors, entityName, hookFilterObj);
                            validationErrors = hookFilterObj.errors;
                        }
                        catch (Exception ex)
                        {
                            response.Success = false;
                            response.Object = evaluationObj;
                            response.Timestamp = DateTime.UtcNow;
                            response.Message = (!recordId.HasValue || recordId.Value == Guid.Empty) ?
                                "Plugin error in web hook create_record_validation_errors_filter: " + ex.Message :
                                "Plugin error in web hook update_record_validation_errors_filter: " + ex.Message;
                            return response;
                            //throw new Exception ("Plugin error in web hook update_record_validation_errors_filter: " + ex.Message));
                        }// <<<
                        #endregion

                        if (validationErrors.Count > 0)
                        {
                            List<string> errorsList = (List<string>)((EntityRecord)evaluationObj["errors"])["id"];
                            foreach (var validationError in validationErrors)
                            {
                                errorsList.Add(validationError.Message);
                                ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;
                                continue;
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    foreach (var columnName in columnNames)
                    {
                        EntityRecord commandRecords = ((EntityRecord)commands[columnName]);
                        string command = (string)commandRecords["command"];

                        string fieldValue = csvReader.GetField<string>(columnName);
                        if (!(command == "no_import" && generalCommand == "evaluate-import"))
                            rowRecord[columnName] = fieldValue;
                    }
                }

                ((List<EntityRecord>)evaluationObj["records"]).Add(rowRecord);

            }

            foreach (var columnName in columnNames)
            {
                if (commands.GetProperties().Any(p => p.Key == columnName))
                {
                    ((EntityRecord)commands[columnName]).Properties.Remove("currentFieldMeta");
                    ((EntityRecord)commands[columnName]).Properties.Remove("relationEntityFieldMeta");
                    ((EntityRecord)commands[columnName]).Properties.Remove("relationFieldMeta");
                }
            }

            if ((int)statsObject["errors"] > 0)
            {
                if (generalCommand == "evaluate-import")
                {
                    response.Success = false;
                    //evaluationObj["general_command"] = "evaluate";
                }
                response.Object = evaluationObj;
                return response;
            }

            if (generalCommand == "evaluate-import")
            {
                using (DbConnection connection = DbContext.Current.CreateConnection())
                {
                    connection.BeginTransaction();

                    try
                    {
                        int fieldCreated = 0;
                        foreach (var columnName in columnNames)
                        {
                            string command = (string)((EntityRecord)commands[columnName])["command"];

                            if (command == "to_create")
                            {
                                FieldType fieldType = (FieldType)Enum.Parse(typeof(FieldType), (((EntityRecord)commands[columnName])["fieldType"]).ToString());
                                string fieldName = (string)((EntityRecord)commands[columnName])["fieldName"];
                                string fieldLabel = (string)((EntityRecord)commands[columnName])["fieldLabel"];
                                var result = entMan.CreateField(entity.Id, fieldType, null, fieldName, fieldLabel);

                                if (!result.Success)
                                {
                                    string message = result.Message;
                                    if (result.Errors.Count > 0)
                                    {
                                        foreach (ErrorModel error in result.Errors)
                                            message += " " + error.Message;
                                    }
                                    throw new Exception(message);
                                }
                                fieldCreated++;
                            }
                        }

                        int successfullyCreatedRecordsCount = 0;
                        int successfullyUpdatedRecordsCount = 0;

                        List<EntityRecord> records = (List<EntityRecord>)evaluationObj["records"];
                        foreach (EntityRecord record in records)
                        {
                            EntityRecord newRecord = record;
                            QueryResponse result;
                            if (!newRecord.GetProperties().Any(x => x.Key == "id") || newRecord["id"] == null || string.IsNullOrEmpty(newRecord["id"].ToString()))
                            {
                                newRecord["id"] = Guid.NewGuid();
                                if (enableWebHooks)
                                {
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    //WEBHOOK FILTER << create_record_pre_save_filter >>
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    #region
                                    try
                                    {
                                        dynamic hookFilterObj = new ExpandoObject();
                                        hookFilterObj.record = newRecord;
                                        hookFilterObj.recordId = (Guid)newRecord["id"];
                                        hookFilterObj.controller = controller;
                                        hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.CreateRecordPreSave, entityName, hookFilterObj);
                                        newRecord = hookFilterObj.record;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Plugin error in web hook create_record_pre_save_filter: " + ex.Message);
                                    }// <<<
                                    #endregion
                                }


                                result = recMan.CreateRecord(entityName, newRecord);

                                if (result.Success)
                                    successfullyCreatedRecordsCount++;

                                if (result.Success && enableWebHooks)
                                {
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    //WEBHOOK ACTION << create_record >>
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    #region
                                    try
                                    {
                                        dynamic hookActionObj = new ExpandoObject();
                                        hookActionObj.record = result.Object.Data[0];
                                        hookActionObj.result = result;
                                        hookActionObj.controller = controller;
                                        hooksService.ProcessActions(SystemWebHookNames.CreateRecordAction, entityName, hookActionObj);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Plugin error in web hook create_record_success_action: " + ex.Message);
                                    }// <<<
                                    #endregion
                                }
                            }
                            else
                            {
                                if (enableWebHooks)
                                {
                                    Guid id;
                                    if (Guid.TryParse((string)newRecord["id"], out id))
                                        newRecord["id"] = id;
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    //WEBHOOK FILTER << update_record_pre_save_filter >>
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    #region
                                    try
                                    {
                                        dynamic hookFilterObj = new ExpandoObject();
                                        hookFilterObj.record = newRecord;
                                        hookFilterObj.recordId = new Guid(newRecord["id"].ToString());
                                        hookFilterObj.controller = controller;
                                        hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.UpdateRecordPreSave, entityName, hookFilterObj);
                                        newRecord = hookFilterObj.record;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Plugin error in web hook update_record_pre_save_filter: " + ex.Message);
                                    }// <<<
                                    #endregion
                                }
                                result = recMan.UpdateRecord(entityName, newRecord);

                                if (result.Success)
                                    successfullyUpdatedRecordsCount++;

                                if (result.Success && enableWebHooks)
                                {
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    //WEBHOOK ACTION << update_record_success_action >>
                                    //////////////////////////////////////////////////////////////////////////////////////
                                    #region
                                    try
                                    {
                                        dynamic hookActionObj = new ExpandoObject();
                                        hookActionObj.record = result.Object.Data[0];
                                        hookActionObj.oldRecord = newRecord;
                                        hookActionObj.result = result;
                                        hookActionObj.recordId = result.Object.Data[0]["id"];
                                        hookActionObj.controller = controller;
                                        hooksService.ProcessActions(SystemWebHookNames.UpdateRecordAction, entityName, hookActionObj);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Plugin error in web hook update_record_success_action: " + ex.Message);
                                    }// <<<
                                    #endregion
                                }
                            }

                            if (!result.Success)
                            {
                                string message = result.Message;
                                if (result.Errors.Count > 0)
                                {
                                    foreach (ErrorModel error in result.Errors)
                                        message += " " + error.Message;
                                }
                                throw new Exception(message);
                            }
                        }

                        ((EntityRecord)evaluationObj["stats"])["to_create"] = successfullyCreatedRecordsCount;
                        ((EntityRecord)evaluationObj["stats"])["to_update"] = successfullyUpdatedRecordsCount;
                        ((EntityRecord)evaluationObj["stats"])["total_records"] = records.Count;
                        ((EntityRecord)evaluationObj["stats"])["fields_created"] = fieldCreated;

                        connection.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        //WebVella.ERP.Api.Cache.ClearEntities();

                        connection.RollbackTransaction();

                        ((EntityRecord)evaluationObj["stats"])["errors"] = (int)((EntityRecord)evaluationObj["stats"])["errors"] + 1;

                        response.Success = false;
                        response.Object = evaluationObj;
                        response.Timestamp = DateTime.UtcNow;
#if DEBUG
                        response.Message = e.Message + e.StackTrace;
#else
							response.Message = "Import failed! An internal error occurred!";
#endif
                    }
                }

                Cache.ClearEntities();
                response.Object = evaluationObj;
                return response;
            }

            response.Object = evaluationObj;
            return response;
        }
    }
}

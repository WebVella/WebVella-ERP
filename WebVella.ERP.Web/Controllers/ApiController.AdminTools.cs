using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebVella.ERP.Api.Models;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Database;
using System.Collections.Generic;
using Newtonsoft.Json;

using Npgsql;
using System.Data;

namespace WebVella.ERP.Web.Controllers
{
	public partial class ApiController
	{
		public string ViewListTreeCode = "";	

		#region << Admin Tools >>
		[AcceptVerbs(new[] { "POST" }, Route = "/admin/tools/evaluate-changes")]
		public IActionResult Index([FromBody]JObject connectionStringObject)
		{

			var connectionString = (string)connectionStringObject["connectionString"];
			var includeEntityMeta = (bool)connectionStringObject["includeEntityMeta"];
			var includeEntityRelations = (bool)connectionStringObject["includeEntityRelations"];
			var includeAreas = (bool)connectionStringObject["includeAreas"];
			var includeRoles = (bool)connectionStringObject["includeRoles"];

			var response = new MetaChangeResponseModel();
			if (string.IsNullOrEmpty(connectionString))
			{
				response.Success = false;
				response.Message = "Connection string is required";
				return Json(response);
			}

			string OLD_DB_CONNECTION_STRING = connectionString;

			try
			{

				var changeRow = new MetaChangeModel();
				//Entity
				var currentEntityList = new List<DbEntity>();
				var oldEntityList = new List<DbEntity>();
				var oldEntityDictionary = new Dictionary<Guid, DbEntity>();
				var oldEntityProcessedDictionary = new Dictionary<Guid, bool>();
				//Field
				var oldEntityFieldsList = new List<DbBaseField>();
				var oldEntityFieldsDictionary = new Dictionary<Guid, DbBaseField>();
				var oldEntityFieldsProcessedDictionary = new Dictionary<Guid, bool>();
				//View
				var oldEntityRecordViewList = new List<DbRecordView>();
				var oldEntityRecordViewDictionary = new Dictionary<Guid, DbRecordView>();
				var oldEntityRecordViewProcessedDictionary = new Dictionary<Guid, bool>();
				//List
				var oldEntityRecordListList = new List<DbRecordList>();
				var oldEntityRecordListDictionary = new Dictionary<Guid, DbRecordList>();
				var oldEntityRecordListProcessedDictionary = new Dictionary<Guid, bool>();
				//Tree
				var oldEntityRecordTreeList = new List<DbRecordTree>();
				var oldEntityRecordTreeDictionary = new Dictionary<Guid, DbRecordTree>();
				var oldEntityRecordTreeProcessedDictionary = new Dictionary<Guid, bool>();

				//Relations
				var currentRelationsList = new List<DbEntityRelation>();
				var oldRelationsList = new List<DbEntityRelation>();
				var oldRelationsDictionary = new Dictionary<Guid, DbEntityRelation>();
				var oldRelationsProcessedDictionary = new Dictionary<Guid, bool>();
				var relationsNameDictionary = new Dictionary<string, DbEntityRelation>();
				//Area
				var currentAreaList = new List<EntityRecord>();
				var oldAreaList = new List<EntityRecord>();
				var oldAreasDictionary = new Dictionary<Guid, EntityRecord>();
				var oldAreasProcessedDictionary = new Dictionary<Guid, bool>();

				//Roles
				var currentRoleList = new List<EntityRecord>();
				var oldRoleList = new List<EntityRecord>();
				var oldRolesDictionary = new Dictionary<Guid, EntityRecord>();
				var oldRolesProcessedDictionary = new Dictionary<Guid, bool>();

				var query = new EntityQuery("area");
				var queryRole = new EntityQuery("role");

				#region << Get elements >>
				currentEntityList = DbContext.Current.EntityRepository.Read();
				currentAreaList = DbContext.Current.RecordRepository.Find(query);
				currentRelationsList = DbContext.Current.RelationRepository.Read();
				currentRoleList = DbContext.Current.RecordRepository.Find(queryRole);

				oldEntityList = ReadOldEntities(OLD_DB_CONNECTION_STRING);
				oldAreaList = ReadOldAreas(OLD_DB_CONNECTION_STRING);
				oldRelationsList = ReadOldRelations(OLD_DB_CONNECTION_STRING);
				oldRoleList = ReadOldRoles(OLD_DB_CONNECTION_STRING);
				#endregion

				#region << Generate relations list >>
				foreach (var relation in currentRelationsList)
				{
					relationsNameDictionary[relation.Name] = relation;
				}

				#endregion

				if (includeAreas)
				{
					#region << Process areas >>

					#region << Init >>
					foreach (var area in oldAreaList)
					{
						oldAreasDictionary[(Guid)area["id"]] = area;
					}
					#endregion

					#region << Logic >>
					foreach (var area in currentAreaList)
					{
						if (!oldAreasDictionary.ContainsKey((Guid)area["id"]))
						{
							//// CREATED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "area";
							changeRow.Type = "created";
							changeRow.Name = (string)area["name"];
							response.Changes.Add(changeRow);
							response.Code += CreateAreaCode(area);
						}
						else
						{
							//// POSSIBLE UPDATE
							/////////////////////////////////////////////////////
							var changeCheckResponse = UpdateAreaCode(area, oldAreasDictionary[(Guid)area["id"]]);
							if (changeCheckResponse.HasUpdate)
							{
								//1.1 Updated
								changeRow = new MetaChangeModel();
								changeRow.Element = "area";
								changeRow.Type = "updated";
								changeRow.Name = (string)area["name"];
								changeRow.ChangeList = changeCheckResponse.ChangeList;
								response.Changes.Add(changeRow);
								response.Code += changeCheckResponse.Code;
							}

							// MARK ID AS PROCESSED
							/////////////////////////////////////////////////////
							oldAreasProcessedDictionary[(Guid)area["id"]] = true;
						}

					}

					foreach (var area in oldAreaList)
					{
						if (!oldAreasProcessedDictionary.ContainsKey((Guid)area["id"]))
						{
							//// DELETED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "area";
							changeRow.Type = "deleted";
							changeRow.Name = (string)area["name"];
							response.Changes.Add(changeRow);
							response.Code += DeleteAreaCode(area);
						}
					}

					#endregion

					#endregion
				}
				if (includeEntityMeta)
				{
					#region << Process entity >>

					#region << Init >>
					foreach (var entity in oldEntityList)
					{
						oldEntityDictionary[entity.Id] = entity;
						foreach (var field in entity.Fields)
						{
							oldEntityFieldsList.Add(field);
							oldEntityFieldsDictionary[field.Id] = field;
						}
						foreach (var view in entity.RecordViews)
						{
							oldEntityRecordViewList.Add(view);
							oldEntityRecordViewDictionary[view.Id] = view;
						}
						foreach (var list in entity.RecordLists)
						{
							oldEntityRecordListList.Add(list);
							oldEntityRecordListDictionary[list.Id] = list;
						}
						foreach (var tree in entity.RecordTrees)
						{
							oldEntityRecordTreeList.Add(tree);
							oldEntityRecordTreeDictionary[tree.Id] = tree;
						}
					}
					#endregion

					#region << Logic >>
					foreach (var entity in currentEntityList)
					{
						if (!oldEntityDictionary.ContainsKey(entity.Id))
						{
							//// CREATED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "entity";
							changeRow.Type = "created";
							changeRow.Name = entity.Name;
							response.Changes.Add(changeRow);
							string entityCode = "";
							string viewListTreeCode = "";
							CreateEntityCode(entity, relationsNameDictionary,out entityCode,out viewListTreeCode);
							response.Code += entityCode;
							ViewListTreeCode += viewListTreeCode;
						}
						else
						{
							//// POSSIBLE UPDATE
							/////////////////////////////////////////////////////
							var changeCheckResponse = UpdateEntityCode(entity, oldEntityDictionary[entity.Id]);
							if (changeCheckResponse.HasUpdate)
							{
								//1.1 Updated
								changeRow = new MetaChangeModel();
								changeRow.Element = "entity";
								changeRow.Type = "updated";
								changeRow.Name = entity.Name;
								changeRow.ChangeList = changeCheckResponse.ChangeList;
								response.Changes.Add(changeRow);
								response.Code += changeCheckResponse.Code;
							}


							// MARK ID AS PROCESSED
							/////////////////////////////////////////////////////
							oldEntityProcessedDictionary[entity.Id] = true;
						}

					}

					foreach (var entity in oldEntityList)
					{
						if (!oldEntityProcessedDictionary.ContainsKey(entity.Id))
						{
							//// DELETED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "entity";
							changeRow.Type = "deleted";
							changeRow.Name = entity.Name;
							response.Changes.Add(changeRow);
							response.Code += DeleteEntityCode(entity);
						}
					}
					#endregion

					#endregion
				}
				if (includeEntityRelations)
				{
					#region << Process relations >>

					#region << Init >>
					foreach (var relation in oldRelationsList)
					{
						oldRelationsDictionary[relation.Id] = relation;
					}
					#endregion

					#region << Logic >>
					foreach (var relation in currentRelationsList)
					{
						if (!oldRelationsDictionary.ContainsKey(relation.Id))
						{
							//// CREATED
							/////////////////////////////////////////////////////
							if (!relation.Name.EndsWith("created_by") && !relation.Name.EndsWith("modified_by"))
							{
								//the creation of system fields and relations is handled in the create entity script
								var changeCode = CreateRelationCode(relation);
								changeRow = new MetaChangeModel();
								changeRow.Element = "relation";
								changeRow.Type = "created";
								changeRow.Name = relation.Name;
								changeRow.ChangeList = new List<string>();
								if (changeCode == string.Empty)
								{
									changeRow.ChangeList.Add(@"<span class='go-gray'>No code will be generated. It is automatically created, in the entity creation process</span>");
								}
								response.Changes.Add(changeRow);
								response.Code += changeCode;
							}
						}
						else
						{
							//// POSSIBLE UPDATE
							/////////////////////////////////////////////////////
							var changeCheckResponse = UpdateRelationCode(relation, oldRelationsDictionary[relation.Id]);
							if (changeCheckResponse.HasUpdate)
							{
								//1.1 Updated
								changeRow = new MetaChangeModel();
								changeRow.Element = "relation";
								changeRow.Type = "updated";
								changeRow.Name = relation.Name;
								changeRow.ChangeList = changeCheckResponse.ChangeList;
								response.Changes.Add(changeRow);
								response.Code += changeCheckResponse.Code;
							}

							// MARK ID AS PROCESSED
							/////////////////////////////////////////////////////
							oldRelationsProcessedDictionary[relation.Id] = true;
						}

					}

					foreach (var relation in oldRelationsList)
					{
						if (!oldRelationsProcessedDictionary.ContainsKey(relation.Id))
						{
							//// DELETED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "relation";
							changeRow.Type = "deleted";
							changeRow.Name = relation.Name;
							response.Changes.Add(changeRow);
							response.Code += DeleteRelationCode(relation);
						}
					}

					#endregion

					#endregion
				}
				if (includeRoles)
				{
					#region << Process roles >>

					#region << Init >>
					foreach (var role in oldRoleList)
					{
						oldRolesDictionary[(Guid)role["id"]] = role;
					}
					#endregion

					#region << Logic >>
					foreach (var role in currentRoleList)
					{
						if (!oldRolesDictionary.ContainsKey((Guid)role["id"]))
						{
							//// CREATED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "role";
							changeRow.Type = "created";
							changeRow.Name = (string)role["name"];
							response.Changes.Add(changeRow);
							response.Code += CreateRoleCode(role);
						}
						else
						{
							//// POSSIBLE UPDATE
							/////////////////////////////////////////////////////
							var changeCheckResponse = UpdateRoleCode(role, oldRolesDictionary[(Guid)role["id"]]);
							if (changeCheckResponse.HasUpdate)
							{
								//1.1 Updated
								changeRow = new MetaChangeModel();
								changeRow.Element = "role";
								changeRow.Type = "updated";
								changeRow.Name = (string)role["name"];
								changeRow.ChangeList = changeCheckResponse.ChangeList;
								response.Changes.Add(changeRow);
								response.Code += changeCheckResponse.Code;
							}

							// MARK ID AS PROCESSED
							/////////////////////////////////////////////////////
							oldRolesProcessedDictionary[(Guid)role["id"]] = true;
						}

					}

					foreach (var role in oldRoleList)
					{
						if (!oldRolesProcessedDictionary.ContainsKey((Guid)role["id"]))
						{
							//// DELETED
							/////////////////////////////////////////////////////
							changeRow = new MetaChangeModel();
							changeRow.Element = "role";
							changeRow.Type = "deleted";
							changeRow.Name = (string)role["name"];
							response.Changes.Add(changeRow);
							response.Code += DeleteRoleCode(role);
						}
					}

					#endregion

					#endregion
				}

				response.Code += ViewListTreeCode;

				return Json(response);

			}
			catch (Exception ex)
			{
				var jsonReponse = new MetaChangeResponseModel();
				jsonReponse.Success = false;
				jsonReponse.Message = ex.Message;
				return Json(jsonReponse);
			}
		}

		#region << Read OLD data >>

		private List<DbEntity> ReadOldEntities(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entities;", con);

					using (NpgsqlDataReader reader = command.ExecuteReader())
					{

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
						List<DbEntity> entities = new List<DbEntity>();
						while (reader.Read())
						{
							DbEntity entity = JsonConvert.DeserializeObject<DbEntity>(reader[0].ToString(), settings);
							entities.Add(entity);
						}



						reader.Close();
						return entities;
					}
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<EntityRecord> ReadOldAreas(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM rec_area;", con);
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
					DataTable table = new DataTable();
					adapter.Fill(table);
					return table.AsRecordList();
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<DbEntityRelation> ReadOldRelations(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entity_relations;", con);
					using (NpgsqlDataReader reader = command.ExecuteReader())
					{

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
						List<DbEntityRelation> relations = new List<DbEntityRelation>();
						while (reader.Read())
						{
							DbEntityRelation relation = JsonConvert.DeserializeObject<DbEntityRelation>(reader[0].ToString(), settings);
							relations.Add(relation);
						}



						reader.Close();
						return relations;
					}
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<EntityRecord> ReadOldRoles(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM rec_role;", con);
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
					DataTable table = new DataTable();
					adapter.Fill(table);
					return table.AsRecordList();
				}
				finally
				{
					con.Close();
				}
			}
		}

		#endregion

		#region << Area >>
		private string CreateAreaCode(EntityRecord area)
		{

			var response = "" +
$"#region << ***Create area*** Area name: {(string)area["name"]} >>\n" +
"{\n" +
	"\tvar area = new EntityRecord();\n" +
	$"\tarea[\"id\"] = new Guid(\"{(Guid)area["id"]}\");\n" +
	$"\tarea[\"name\"] = \"{(string)area["name"]}\";\n" +
	$"\tarea[\"label\"] = \"{(string)area["label"]}\";\n" +
	$"\tarea[\"icon_name\"] = \"{(string)area["icon_name"]}\";\n" +
	$"\tarea[\"color\"] = \"{(string)area["color"]}\";\n" +
	$"\tarea[\"folder\"] = \"{(string)area["folder"]}\";\n" +
	$"\tarea[\"weight\"] = {(decimal)area["weight"]};\n" +
	$"\tarea[\"roles\"] = \"{((string)area["roles"]).Replace("\"", "\\\"")}\";\n" +
	$"\tarea[\"attachments\"] = \"{((string)area["attachments"]).Replace("\"", "\\\"")}\";\n" +
	"\tvar createAreaResult = recMan.CreateRecord(\"area\", area);\n" +
	"\tif (!createAreaResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Area create with name : {(string)area["name"]}. Message:\" + createAreaResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private string DeleteAreaCode(EntityRecord area)
		{
			var response = "" +
$"#region << ***Delete area*** Area name: {(string)area["name"]} >>\n" +
"{\n" +
	$"\tvar deleteAreaResult = recMan.DeleteRecord(\"area\", new Guid(\"{(Guid)area["id"]}\"));\n" +
	"\tif (!deleteAreaResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Area delete with name : {(string)area["name"]}. Message:\" + deleteAreaResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateAreaCode(EntityRecord currentArea, EntityRecord oldArea)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			code =
$"#region << ***Update area***  Area name: {(string)currentArea["name"]} >>\n" +
"{\n" +
	"\tvar patchObject = new EntityRecord();\n" +
	$"\tpatchObject[\"id\"] = new Guid(\"{(Guid)currentArea["id"]}\");\n";

			//name
			if ((string)currentArea["name"] != (string)oldArea["name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"name\"] = \"{(string)currentArea["name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{(string)oldArea["name"]}</span> to <span class='go-red'>{(string)currentArea["name"]}</span>");
			}
			//label	
			if ((string)currentArea["label"] != (string)oldArea["label"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"label\"] = \"{(string)currentArea["label"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>label</span> from <span class='go-red'>{(string)oldArea["label"]}</span> to <span class='go-red'>{(string)currentArea["label"]}</span>");
			}
			//color	
			if ((string)currentArea["color"] != (string)oldArea["color"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"color\"] = \"{(string)currentArea["color"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>color</span> from <span class='go-red'>{(string)oldArea["color"]}</span> to <span class='go-red'>{(string)currentArea["color"]}</span>");
			}
			//icon_name	
			if ((string)currentArea["icon_name"] != (string)oldArea["icon_name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"icon_name\"] = \"{(string)currentArea["icon_name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>icon_name</span> from <span class='go-red'>{(string)oldArea["icon_name"]}</span> to <span class='go-red'>{(string)currentArea["icon_name"]}</span>");
			}
			//weight	
			if ((decimal)currentArea["weight"] != (decimal)oldArea["weight"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"weight\"] = \"{(decimal)currentArea["weight"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>weight</span> from <span class='go-red'>{(decimal)oldArea["weight"]}</span> to <span class='go-red'>{(decimal)currentArea["weight"]}</span>");
			}
			//attachments	
			if ((string)currentArea["attachments"] != (string)oldArea["attachments"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"attachments\"] = \"{((string)currentArea["attachments"]).Replace("\"", "\\\"")}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>attachments</span> attachments code was changed</span>");
			}
			//roles	
			if ((string)currentArea["roles"] != (string)oldArea["roles"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"roles\"] = \"{((string)currentArea["roles"]).Replace("\"", "\\\"")}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>roles</span> from <span class='go-red'>{(string)oldArea["roles"]}</span> to <span class='go-red'>{(string)currentArea["roles"]}</span>");
			}
			//folder	
			if ((string)currentArea["folder"] != (string)oldArea["folder"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"folder\"] = \"{(string)currentArea["folder"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>folder</span> from <span class='go-red'>{(string)oldArea["folder"]}</span> to <span class='go-red'>{(string)currentArea["folder"]}</span>");
			}

			code +=
				"\tvar updateAreaResult = recMan.UpdateRecord(\"area\", patchObject);\n" +
				"\tif (!updateAreaResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Area update with name : {(string)currentArea["name"]}. Message:\" + updateAreaResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion

		#region << Entity >>


		private void CreateEntityCode(DbEntity entity, Dictionary<string, DbEntityRelation> relationsNameDictionary,out string entityResponse, out string viewListTreeResponse)
		{
			entityResponse = "";
			viewListTreeResponse = "";
			//escape some possible quotes
			if (entity.Label != null)
				entity.Label = entity.Label.Replace("\"", "\\\"");

			var response = "" +
$"#region << ***Create entity*** Entity name: {entity.Name} >>\n" +
"{\n" +
	"\t#region << entity >>\n" +
	"\t{\n" +
		"\t\tvar entity = new InputEntity();\n" +
		"\t\tvar systemFieldIdDictionary = new Dictionary<string,Guid>();\n" +
		//Generate system fields
		$"\t\tsystemFieldIdDictionary[\"id\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "id").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"created_on\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "created_on").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"created_by\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "created_by").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"last_modified_on\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "last_modified_on").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"last_modified_by\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "last_modified_by").Id}\");\n";
			//Generating system relations
			var createdBySystemRelationName = "user_" + entity.Name + "_created_by";
			var modifiedBySystemRelationName = "user_" + entity.Name + "_modified_by";
			response +=
			$"\t\tsystemFieldIdDictionary[\"{createdBySystemRelationName}\"] = new Guid(\"{relationsNameDictionary[createdBySystemRelationName].Id}\");\n" +
			$"\t\tsystemFieldIdDictionary[\"{modifiedBySystemRelationName}\"] = new Guid(\"{relationsNameDictionary[modifiedBySystemRelationName].Id}\");\n" +

			$"\t\tentity.Id = new Guid(\"{entity.Id}\");\n" +
			$"\t\tentity.Name = \"{entity.Name}\";\n" +
			$"\t\tentity.Label = \"{entity.Label}\";\n" +
			$"\t\tentity.LabelPlural = \"{entity.LabelPlural}\";\n" +
			$"\t\tentity.System = {(entity.System).ToString().ToLowerInvariant()};\n" +
			$"\t\tentity.IconName = \"{entity.IconName}\";\n" +
			$"\t\tentity.Weight = (decimal){entity.Weight};\n" +
			"\t\tentity.RecordPermissions = new RecordPermissions();\n" +
			"\t\tentity.RecordPermissions.CanCreate = new List<Guid>();\n" +
			"\t\tentity.RecordPermissions.CanRead = new List<Guid>();\n" +
			"\t\tentity.RecordPermissions.CanUpdate = new List<Guid>();\n" +
			"\t\tentity.RecordPermissions.CanDelete = new List<Guid>();\n" +
			"\t\t//Create\n";
			foreach (var permId in entity.RecordPermissions.CanCreate)
			{
				response += $"\t\tentity.RecordPermissions.CanCreate.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//READ\n";
			foreach (var permId in entity.RecordPermissions.CanRead)
			{
				response += $"\t\tentity.RecordPermissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//UPDATE\n";
			foreach (var permId in entity.RecordPermissions.CanUpdate)
			{
				response += $"\t\tentity.RecordPermissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//DELETE\n";
			foreach (var permId in entity.RecordPermissions.CanDelete)
			{
				response += $"\t\tentity.RecordPermissions.CanDelete.Add(new Guid(\"{permId}\"));\n";
			}

			response += "\t\t{\n" +
				"\t\t\tvar response = entMan.CreateEntity(entity, false, false,systemFieldIdDictionary);\n" +
				"\t\t\tif (!response.Success)\n" +
					$"\t\t\t\tthrow new Exception(\"System error 10050. Entity: {entity.Name} creation Message: \" + response.Message);\n" +
			"\t\t}\n" +
		"\t}\n" +
		"\t#endregion\n" +
	"}\n" +
	"#endregion\n\n";

			//foreach field generate createField and add

			foreach (var field in entity.Fields)
			{
				if (field.Name != "id" && field.Name != "created_on" && field.Name != "created_by" && field.Name != "last_modified_on" && field.Name != "last_modified_by")
				{
					//System field and relations will be created on entity creation
					response += CreateFieldCode(field, entity.Id, entity.Name);
				}
			}

			//foreach view generate create view and add
			foreach (var view in entity.RecordViews)
			{
				viewListTreeResponse += CreateViewCode(view, entity.Id, entity.Name);
			}

			//foreach list generate createList and add
			foreach (var list in entity.RecordLists)
			{
				viewListTreeResponse += CreateListCode(list, entity.Id, entity.Name);
			}

			//foreach tree generate createTree and add
			foreach (var tree in entity.RecordTrees)
			{
				viewListTreeResponse += CreateTreeCode(tree, entity.Id, entity.Name);
			}


			entityResponse = response;
		}

		private UpdateCheckResponse UpdateEntityCode(DbEntity currentEntity, DbEntity oldEntity)
		{
			var response = new UpdateCheckResponse();

			#region << General >>
			var changeGeneralResponse = UpdateGeneralEntityCode(currentEntity, oldEntity);
			if (changeGeneralResponse.HasUpdate)
			{
				response.HasUpdate = true;
				foreach (var change in changeGeneralResponse.ChangeList)
				{
					response.ChangeList.Add(change);
				}
				response.Code += changeGeneralResponse.Code;
			}
			#endregion

			#region << fields >>
			//prepare the old fields dictionary
			var entityOldFieldsDictionary = new Dictionary<Guid, DbBaseField>();
			var entityProcessedFieldsDictionary = new Dictionary<Guid, bool>();
			foreach (var field in oldEntity.Fields)
			{
				entityOldFieldsDictionary[field.Id] = field;
			}

			foreach (var field in currentEntity.Fields)
			{
				if (!entityOldFieldsDictionary.ContainsKey(field.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					if (field.Name != "id" && field.Name != "created_by" && field.Name != "created_on" && field.Name != "last_modified_by" && field.Name != "last_modified_on")
					{
						//the creation of system fields and relations is managed during the entity creation process
						response.HasUpdate = true;
						response.ChangeList.Add($"<span class='go-green label-block'>field</span>  new field <span class='go-red'>{field.Name}</span> was created.</span>");
						response.Code += CreateFieldCode(field, currentEntity.Id, currentEntity.Name);
					}
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateFieldCode(field, entityOldFieldsDictionary[field.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						response.Code += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedFieldsDictionary[field.Id] = true;
				}
			}
			foreach (var field in oldEntity.Fields)
			{
				if (!entityProcessedFieldsDictionary.ContainsKey(field.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>field</span>  field <span class='go-red'>{field.Name}</span> was deleted.</span>");
					response.Code += DeleteFieldCode(field, currentEntity.Id, currentEntity.Name);
				}
			}
			#endregion

			#region << RecordViews >>
			//prepare the old fields dictionary
			var entityOldViewsDictionary = new Dictionary<Guid, DbRecordView>();
			var entityProcessedViewsDictionary = new Dictionary<Guid, bool>();
			foreach (var view in oldEntity.RecordViews)
			{
				entityOldViewsDictionary[view.Id] = view;
			}

			foreach (var view in currentEntity.RecordViews)
			{
				if (!entityOldViewsDictionary.ContainsKey(view.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>view</span>  new view <span class='go-red'>{view.Name}</span> was created.</span>");
					//response.Code += CreateViewCode(view, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode += CreateViewCode(view, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateViewCode(view, entityOldViewsDictionary[view.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						//response.Code += changeCheckResponse.Code;
						ViewListTreeCode += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedViewsDictionary[view.Id] = true;
				}

			}
			foreach (var view in oldEntity.RecordViews)
			{
				if (!entityProcessedViewsDictionary.ContainsKey(view.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>view</span>  view <span class='go-red'>{view.Name}</span> was deleted.</span>");
					//response.Code += DeleteViewCode(view, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode += DeleteViewCode(view, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			#region << RecordLists >>
			//prepare the old fields dictionary
			var entityOldListsDictionary = new Dictionary<Guid, DbRecordList>();
			var entityProcessedListsDictionary = new Dictionary<Guid, bool>();
			foreach (var list in oldEntity.RecordLists)
			{
				entityOldListsDictionary[list.Id] = list;
			}

			foreach (var list in currentEntity.RecordLists)
			{
				if (!entityOldListsDictionary.ContainsKey(list.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>list</span>  new list <span class='go-red'>{list.Name}</span> was created.</span>");
					//response.Code += CreateListCode(list, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode += CreateListCode(list, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateListCode(list, entityOldListsDictionary[list.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						//response.Code += changeCheckResponse.Code;
						ViewListTreeCode += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedListsDictionary[list.Id] = true;
				}

			}
			foreach (var list in oldEntity.RecordLists)
			{
				if (!entityProcessedListsDictionary.ContainsKey(list.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>list</span>  list <span class='go-red'>{list.Name}</span> was deleted.</span>");
					//response.Code += DeleteListCode(list, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode  += DeleteListCode(list, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			#region << RecordTrees >>
			//prepare the old fields dictionary
			var entityOldTreesDictionary = new Dictionary<Guid, DbRecordTree>();
			var entityProcessedTreesDictionary = new Dictionary<Guid, bool>();
			foreach (var tree in oldEntity.RecordTrees)
			{
				entityOldTreesDictionary[tree.Id] = tree;
			}

			foreach (var tree in currentEntity.RecordTrees)
			{
				if (!entityOldTreesDictionary.ContainsKey(tree.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  new tree <span class='go-red'>{tree.Name}</span> was created.</span>");
					//response.Code += CreateTreeCode(tree, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode += CreateTreeCode(tree, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateTreeCode(tree, entityOldTreesDictionary[tree.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						//response.Code += changeCheckResponse.Code;
						ViewListTreeCode += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedTreesDictionary[tree.Id] = true;
				}

			}
			foreach (var tree in oldEntity.RecordTrees)
			{
				if (!entityProcessedTreesDictionary.ContainsKey(tree.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  tree <span class='go-red'>{tree.Name}</span> was deleted.</span>");
					//response.Code += DeleteTreeCode(tree, currentEntity.Id, currentEntity.Name);
					ViewListTreeCode += DeleteTreeCode(tree, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			return response;
		}

		private string DeleteEntityCode(DbEntity entity)
		{
			var response =
		$"#region << ***Delete entity*** Entity Name: {entity.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteEntity(new Guid(\"{entity.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entity.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateGeneralEntityCode(DbEntity currentEntity, DbEntity oldEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentEntity.Label != null)
				currentEntity.Label = currentEntity.Label.Replace("\"", "\\\"");

			//escape the old entity the same way so the update check is correct
			if (oldEntity.Label != null)
				oldEntity.Label = oldEntity.Label.Replace("\"", "\\\"");

			#region << General details >>
			code =
			$"#region << ***Update entity*** Entity name: {currentEntity.Name} >>\n" +
			"{\n" +
				"\tvar updateObject = new InputEntity();\n" +
				$"\tupdateObject.Id = new Guid(\"{currentEntity.Id}\");\n";
			//name
			if (currentEntity.Name != oldEntity.Name)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldEntity.Name}</span> to <span class='go-red'>{currentEntity.Name}</span>");
			}
			code += $"\tupdateObject.Name = \"{currentEntity.Name}\";\n";

			//label
			if (currentEntity.Label != oldEntity.Label)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldEntity.Label}</span> to <span class='go-red'>{currentEntity.Label}</span>");
			}
			code += $"\tupdateObject.Label = \"{currentEntity.Label}\";\n";

			//LabelPlural
			if (currentEntity.LabelPlural != oldEntity.LabelPlural)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>LabelPlural</span>  from <span class='go-red'>{oldEntity.LabelPlural}</span> to <span class='go-red'>{currentEntity.LabelPlural}</span>");
			}
			code += $"\tupdateObject.LabelPlural = \"{currentEntity.LabelPlural}\";\n";

			//System
			if (currentEntity.System != oldEntity.System)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>System</span>  from <span class='go-red'>{oldEntity.System}</span> to <span class='go-red'>{currentEntity.System}</span>");
			}
			code += $"\tupdateObject.System = {(currentEntity.System).ToString().ToLowerInvariant()};\n";

			//IconName
			if (currentEntity.IconName != oldEntity.IconName)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>IconName</span>  from <span class='go-red'>{oldEntity.IconName}</span> to <span class='go-red'>{currentEntity.IconName}</span>");
			}
			code += $"\tupdateObject.IconName = \"{currentEntity.IconName}\";\n";

			//Weight
			if (currentEntity.Weight != oldEntity.Weight)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>Weight</span>  from <span class='go-red'>{oldEntity.Weight}</span> to <span class='go-red'>{currentEntity.Weight}</span>");
			}
			code += $"\tupdateObject.Weight = Decimal.Parse(\"{currentEntity.Weight}\");\n";

			//RecordPermissions
			var recordPermissionsChanged = false;
			var oldCanReadDictionary = new Dictionary<Guid, bool>();
			var oldCanCreateDictionary = new Dictionary<Guid, bool>();
			var oldCanUpdateDictionary = new Dictionary<Guid, bool>();
			var oldCanDeleteDictionary = new Dictionary<Guid, bool>();

			#region << prepare old status dictionaries >>
			foreach (var permission in oldEntity.RecordPermissions.CanRead)
			{
				oldCanReadDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanCreate)
			{
				oldCanCreateDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanUpdate)
			{
				oldCanUpdateDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanDelete)
			{
				oldCanDeleteDictionary[permission] = true;
			}
			#endregion

			#region << Check for change and generate permissions code >>
			code += $"\tupdateObject.RecordPermissions = new RecordPermissions();\n" +
			$"\tupdateObject.RecordPermissions.CanRead = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanCreate = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanUpdate = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanDelete = new List<Guid>();\n";

			foreach (var permission in currentEntity.RecordPermissions.CanRead)
			{
				if (!oldCanReadDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanRead.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanCreate)
			{
				if (!oldCanCreateDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanCreate.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanUpdate)
			{
				if (!oldCanUpdateDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanUpdate.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanDelete)
			{
				if (!oldCanDeleteDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanDelete.Add(new Guid(\"{permission}\"));\n";
			}
			if (recordPermissionsChanged)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>permissions</span>  record permissions were changes</span>");
			}

			#endregion

			code +=
				"\tvar updateEntityResult = entMan.UpdateEntity(updateObject);\n" +
				"\tif (!updateEntityResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Entity update with name : {oldEntity.Name}. Message:\" + updateEntityResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";

			#endregion

			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;
		}
		#endregion

		#region << Field >>

		private string CreateFieldCode(DbBaseField field, Guid entityId, string entityName)
		{
			var response = "";
			//escape quotes where they can possible be
			if (field.HelpText != null)
				field.HelpText = field.HelpText.Replace("\"", "\\\"");
			if (field.PlaceholderText != null)
				field.PlaceholderText = field.PlaceholderText.Replace("\"", "\\\"");
			if (field.Label != null)
				field.Label = field.Label.Replace("\"", "\\\"");
			if (field.Description != null)
				field.Description = field.Description.Replace("\"", "\\\"");
			switch (field.GetFieldType())
			{
				case FieldType.AutoNumberField:
					response += CreateAutoNumberFieldCode(field as DbAutoNumberField, entityId, entityName);
					break;
				case FieldType.CheckboxField:
					response += CreateCheckboxFieldCode(field as DbCheckboxField, entityId, entityName);
					break;
				case FieldType.CurrencyField:
					response += CreateCurrencyFieldCode(field as DbCurrencyField, entityId, entityName);
					break;
				case FieldType.DateField:
					response += CreateDateFieldCode(field as DbDateField, entityId, entityName);
					break;
				case FieldType.DateTimeField:
					response += CreateDateTimeFieldCode(field as DbDateTimeField, entityId, entityName);
					break;
				case FieldType.EmailField:
					response += CreateEmailFieldCode(field as DbEmailField, entityId, entityName);
					break;
				case FieldType.FileField:
					response += CreateFileFieldCode(field as DbFileField, entityId, entityName);
					break;
				case FieldType.ImageField:
					response += CreateImageFieldCode(field as DbImageField, entityId, entityName);
					break;
				case FieldType.HtmlField:
					response += CreateHtmlFieldCode(field as DbHtmlField, entityId, entityName);
					break;
				case FieldType.MultiLineTextField:
					response += CreateMultiLineTextFieldCode(field as DbMultiLineTextField, entityId, entityName);
					break;
				case FieldType.MultiSelectField:
					response += CreateMultiSelectFieldCode(field as DbMultiSelectField, entityId, entityName);
					break;
				case FieldType.NumberField:
					response += CreateNumberFieldCode(field as DbNumberField, entityId, entityName);
					break;
				case FieldType.PasswordField:
					response += CreatePasswordFieldCode(field as DbPasswordField, entityId, entityName);
					break;
				case FieldType.PercentField:
					response += CreatePercentFieldCode(field as DbPercentField, entityId, entityName);
					break;
				case FieldType.PhoneField:
					response += CreatePhoneFieldCode(field as DbPhoneField, entityId, entityName);
					break;
				case FieldType.GuidField:
					response += CreateGuidFieldCode(field as DbGuidField, entityId, entityName);
					break;
				case FieldType.SelectField:
					response += CreateSelectFieldCode(field as DbSelectField, entityId, entityName);
					break;
				case FieldType.TextField:
					response += CreateTextFieldCode(field as DbTextField, entityId, entityName);
					break;
				case FieldType.UrlField:
					response += CreateUrlFieldCode(field as DbUrlField, entityId, entityName);
					break;
				case FieldType.TreeSelectField:
					response += CreateTreeSelectFieldCode(field as DbTreeSelectField, entityId, entityName);
					break;
			}

			return response;
		}

		private string CreateAutoNumberFieldCode(DbAutoNumberField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field*** Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
			"\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
			$"\tautonumberField.Id = new Guid(\"{field.Id}\");\n" +
			$"\tautonumberField.Name = \"{field.Name}\";\n" +
			$"\tautonumberField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tautonumberField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tautonumberField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tautonumberField.Description = null;\n";
			}
			else
			{
				response += $"\tautonumberField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tautonumberField.HelpText = null;\n";
			}
			else
			{
				response += $"\tautonumberField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tautonumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tautonumberField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tautonumberField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.DisplayFormat == null)
			{
				response += $"\tautonumberField.DisplayFormat = null;\n";
			}
			else
			{
				response += $"\tautonumberField.DisplayFormat = \"{field.DisplayFormat}\";\n";
			}
			if (field.StartingNumber == null)
			{
				response += $"\tautonumberField.StartingNumber = null;\n";
			}
			else
			{
				response += $"\tautonumberField.StartingNumber = Decimal.Parse(\"{field.StartingNumber}\");\n";
			}

			response +=
			$"\tautonumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tautonumberField.Permissions = new FieldPermissions();\n" +
			"\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
			"\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), autonumberField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateCheckboxFieldCode(DbCheckboxField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
				"\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
				$"\tcheckboxField.Id = new Guid(\"{field.Id}\");\n" +
				$"\tcheckboxField.Name = \"{field.Name}\";\n" +
				$"\tcheckboxField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tcheckboxField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tcheckboxField.Description = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tcheckboxField.HelpText = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tcheckboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.DefaultValue = {(field.DefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tcheckboxField.Permissions = new FieldPermissions();\n" +
			"\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
			"\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), checkboxField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateCurrencyFieldCode(DbCurrencyField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
			"\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
			$"\tcurrencyField.Id = new Guid(\"{field.Id}\");\n" +
			$"\tcurrencyField.Name = \"{field.Name}\";\n" +
			$"\tcurrencyField.Label =  \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tcurrencyField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tcurrencyField.Description = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tcurrencyField.HelpText = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tcurrencyField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tcurrencyField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.MinValue == null)
			{
				response += $"\tcurrencyField.MinValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
			}
			if (field.MaxValue == null)
			{
				response += $"\tcurrencyField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
			}

			response +=
			$"\tcurrencyField.Currency = WebVella.ERP.Utilities.Helpers.GetCurrencyTypeObject(\"{field.Currency.Code}\");\n" +
			$"\tcurrencyField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tcurrencyField.Permissions = new FieldPermissions();\n" +
			"\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
			"\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), currencyField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateDateFieldCode(DbDateField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

		$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
		"{\n" +
			"\tInputDateField dateField = new InputDateField();\n" +
			$"\tdateField.Id =  new Guid(\"{field.Id}\");\n" +
			$"\tdateField.Name = \"{field.Name}\";\n" +
			$"\tdateField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tdateField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdateField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tdateField.Description = null;\n";
			}
			else
			{
				response += $"\tdateField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tdateField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdateField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tdateField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tdateField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdateField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.Format == null)
			{
				response += $"\tdateField.Format = null;\n";
			}
			else
			{
				response += $"\tdateField.Format = \"{field.Format}\";\n";
			}

			response +=
			$"\tdateField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tdateField.Permissions = new FieldPermissions();\n" +
			"\tdateField.Permissions.CanRead = new List<Guid>();\n" +
			"\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dateField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateDateTimeFieldCode(DbDateTimeField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	"\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
	$"\tdatetimeField.Id =  new Guid(\"{field.Id}\");\n" +
	$"\tdatetimeField.Name = \"{field.Name}\";\n" +
	$"\tdatetimeField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tdatetimeField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tdatetimeField.Description = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tdatetimeField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tdatetimeField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tdatetimeField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.Format == null)
			{
				response += $"\tdatetimeField.Format = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.Format = \"{field.Format}\";\n";
			}

			response +=
			$"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tdatetimeField.Permissions = new FieldPermissions();\n" +
			"\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
			"\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), datetimeField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateEmailFieldCode(DbEmailField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	"\tInputEmailField emailField = new InputEmailField();\n" +
	$"\temailField.Id = new Guid(\"{field.Id}\");\n" +
	$"\temailField.Name = \"{field.Name}\";\n" +
	$"\temailField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\temailField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\temailField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\temailField.Description = null;\n";
			}
			else
			{
				response += $"\temailField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\temailField.HelpText = null;\n";
			}
			else
			{
				response += $"\temailField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\temailField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\temailField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\temailField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\temailField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\temailField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\temailField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\temailField.DefaultValue = \"{field.DefaultValue}\";\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\temailField.MaxLength = null;\n";
			}
			else
			{
				response += $"\temailField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			response +=
			$"\temailField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\temailField.Permissions = new FieldPermissions();\n" +
			$"\temailField.Permissions.CanRead = new List<Guid>();\n" +
			$"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), emailField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateFileFieldCode(DbFileField field, Guid entityId, string entityName)
		{
			var response = "";

			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputFileField fileField = new InputFileField();\n" +
	$"\tfileField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tfileField.Name = \"{field.Name}\";\n" +
	$"\tfileField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tfileField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tfileField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tfileField.Description = null;\n";
			}
			else
			{
				response += $"\tfileField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tfileField.HelpText = null;\n";
			}
			else
			{
				response += $"\tfileField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tfileField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tfileField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tfileField.DefaultValue =\"{field.DefaultValue}\";\n";
			}
			response +=
			$"\tfileField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.Permissions = new FieldPermissions();\n" +
			$"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), fileField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateHtmlFieldCode(DbHtmlField field, Guid entityId, string entityName)
		{
			var response = "";

			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputHtmlField htmlField = new InputHtmlField();\n" +
	$"\thtmlField.Id = new Guid(\"{field.Id}\");\n" +
	$"\thtmlField.Name = \"{field.Name}\";\n" +
	$"\thtmlField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\thtmlField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\thtmlField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\thtmlField.Description = null;\n";
			}
			else
			{
				response += $"\thtmlField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\thtmlField.HelpText = null;\n";
			}
			else
			{
				response += $"\thtmlField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\thtmlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\thtmlField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\thtmlField.DefaultValue = \"{field.DefaultValue}\";\n";
			}
			response +=
			$"\thtmlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.Permissions = new FieldPermissions();\n" +
			$"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), htmlField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateImageFieldCode(DbImageField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
			$"\tInputImageField imageField = new InputImageField();\n" +
			$"\timageField.Id = new Guid(\"{field.Id}\");\n" +
			$"\timageField.Name = \"{field.Name}\";\n" +
			$"\timageField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\timageField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\timageField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\timageField.Description = null;\n";
			}
			else
			{
				response += $"\timageField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\timageField.HelpText = null;\n";
			}
			else
			{
				response += $"\timageField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\timageField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Auditable =  {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\timageField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\timageField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\timageField.DefaultValue =\"{field.DefaultValue}\";\n";
			}
			response +=
			$"\timageField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Permissions = new FieldPermissions();\n" +
			$"\timageField.Permissions.CanRead = new List<Guid>();\n" +
			$"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), imageField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateMultiLineTextFieldCode(DbMultiLineTextField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
	$"\ttextareaField.Id = new Guid(\"{field.Id}\");\n" +
	$"\ttextareaField.Name = \"{field.Name}\";\n" +
	$"\ttextareaField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\ttextareaField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttextareaField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\ttextareaField.Description = null;\n";
			}
			else
			{
				response += $"\ttextareaField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\ttextareaField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttextareaField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\ttextareaField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\ttextareaField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\ttextareaField.DefaultValue = \"{field.DefaultValue}\";\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\ttextareaField.MaxLength = null;\n";
			}
			else
			{
				response += $"\ttextareaField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			if (field.VisibleLineNumber == null)
			{
				response += $"\ttextareaField.VisibleLineNumber = null;\n";
			}
			else
			{
				response += $"\ttextareaField.VisibleLineNumber = Int32.Parse(\"{field.VisibleLineNumber}\");\n";
			}
			response +=
			$"\ttextareaField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Permissions = new FieldPermissions();\n" +
			$"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textareaField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateMultiSelectFieldCode(DbMultiSelectField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
	$"\tmultiSelectField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tmultiSelectField.Name = \"{field.Name}\";\n" +
	$"\tmultiSelectField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tmultiSelectField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tmultiSelectField.Description = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tmultiSelectField.HelpText = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
	$"\tmultiSelectField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	"\tmultiSelectField.DefaultValue = ";
			var defaultValues = field.DefaultValue.ToList();
			if (defaultValues.Count > 0)
			{
				response += "new List<string>() {";

				for (int i = 0; i < defaultValues.Count; i++)
				{
					response += "\"" + defaultValues[i] + "\"";
					if (i < defaultValues.Count - 1)
					{
						response += ",";
					}
				}
				response += "};\n";
			}
			else
			{
				response += "null;\n";
			}
			response += "\tmultiSelectField.Options = ";
			var fieldOptions = field.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<MultiSelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew MultiSelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=

			$"\tmultiSelectField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
			$"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), multiSelectField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateNumberFieldCode(DbNumberField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
				$"\tInputNumberField numberField = new InputNumberField();\n" +
				$"\tnumberField.Id = new Guid(\"{field.Id}\");\n" +
				$"\tnumberField.Name = \"{field.Name}\";\n" +
				$"\tnumberField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tnumberField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tnumberField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tnumberField.Description = null;\n";
			}
			else
			{
				response += $"\tnumberField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tnumberField.HelpText = null;\n";
			}
			else
			{
				response += $"\tnumberField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tnumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tnumberField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.MinValue == null)
			{
				response += $"\tnumberField.MinValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
			}
			if (field.MaxValue == null)
			{
				response += $"\tnumberField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
			}
			response += $"\tnumberField.DecimalPlaces = byte.Parse(\"{field.DecimalPlaces}\");\n";
			response +=

			$"\tnumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Permissions = new FieldPermissions();\n" +
			$"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), numberField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreatePasswordFieldCode(DbPasswordField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
				$"\tInputPasswordField passwordField = new InputPasswordField();\n" +
				$"\tpasswordField.Id = new Guid(\"{field.Id}\");\n" +
				$"\tpasswordField.Name = \"{field.Name}\";\n" +
				$"\tpasswordField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tpasswordField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tpasswordField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tpasswordField.Description = null;\n";
			}
			else
			{
				response += $"\tpasswordField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tpasswordField.HelpText = null;\n";
			}
			else
			{
				response += $"\tpasswordField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tpasswordField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.MinLength == null)
			{
				response += $"\tpasswordField.MinLength = null;\n";
			}
			else
			{
				response += $"\tpasswordField.MinLength = Int32.Parse(\"{field.MinLength}\");\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\tpasswordField.MaxLength = null;\n";
			}
			else
			{
				response += $"\tpasswordField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			response +=
			$"\tpasswordField.Encrypted = {(field.Encrypted).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Permissions = new FieldPermissions();\n" +
			$"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), passwordField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreatePercentFieldCode(DbPercentField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputPercentField percentField = new InputPercentField();\n" +
	$"\tpercentField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tpercentField.Name = \"{field.Name}\";\n" +
	$"\tpercentField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tpercentField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tpercentField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tpercentField.Description = null;\n";
			}
			else
			{
				response += $"\tpercentField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tpercentField.HelpText = null;\n";
			}
			else
			{
				response += $"\tpercentField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tpercentField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tpercentField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
			}
			if (field.MinValue == null)
			{
				response += $"\tpercentField.MinValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
			}
			if (field.MaxValue == null)
			{
				response += $"\tpercentField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
			}
			response += $"\tpercentField.DecimalPlaces = byte.Parse(\"{field.DecimalPlaces}\");\n";
			response +=
			$"\tpercentField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Permissions = new FieldPermissions();\n" +
			$"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), percentField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreatePhoneFieldCode(DbPhoneField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
		$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
		"{\n" +
			$"\tInputPhoneField phoneField = new InputPhoneField();\n" +
			$"\tphoneField.Id = new Guid(\"{field.Id}\");\n" +
			$"\tphoneField.Name = \"{field.Name}\";\n" +
			$"\tphoneField.Label =  \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tphoneField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tphoneField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tphoneField.Description = null;\n";
			}
			else
			{
				response += $"\tphoneField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tphoneField.HelpText = null;\n";
			}
			else
			{
				response += $"\tphoneField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tphoneField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tphoneField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tphoneField.DefaultValue = \"{field.DefaultValue}\";\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\tphoneField.MaxLength = null;\n";
			}
			else
			{
				response += $"\tphoneField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			if (field.Format == null)
			{
				response += $"\tphoneField.Format = null;\n";
			}
			else
			{
				response += $"\tphoneField.Format = \"{field.Format}\";\n";
			}
			response +=
			$"\tphoneField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Permissions = new FieldPermissions();\n" +
			$"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), phoneField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateGuidFieldCode(DbGuidField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputGuidField guidField = new InputGuidField();\n" +
	$"\tguidField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tguidField.Name = \"{field.Name}\";\n" +
	$"\tguidField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tguidField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tguidField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tguidField.Description = null;\n";
			}
			else
			{
				response += $"\tguidField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tguidField.HelpText = null;\n";
			}
			else
			{
				response += $"\tguidField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tguidField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tguidField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tguidField.DefaultValue = Guid.Parse(\"{field.DefaultValue}\");\n";
			}

			response +=

			$"\tguidField.GenerateNewId = {(field.GenerateNewId).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Permissions = new FieldPermissions();\n" +
			$"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), guidField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateSelectFieldCode(DbSelectField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputSelectField dropdownField = new InputSelectField();\n" +
	$"\tdropdownField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tdropdownField.Name = \"{field.Name}\";\n" +
	$"\tdropdownField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\tdropdownField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdropdownField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\tdropdownField.Description = null;\n";
			}
			else
			{
				response += $"\tdropdownField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\tdropdownField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdropdownField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\tdropdownField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\tdropdownField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdropdownField.DefaultValue = \"{field.DefaultValue}\";\n";
			}

			response += "\tdropdownField.Options = ";
			var fieldOptions = field.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<SelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew SelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=
			$"\tdropdownField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Permissions = new FieldPermissions();\n" +
			$"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dropdownField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateTextFieldCode(DbTextField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
			"{\n" +
				$"\tInputTextField textboxField = new InputTextField();\n" +
			$"\ttextboxField.Id = new Guid(\"{field.Id}\");\n" +
			$"\ttextboxField.Name = \"{field.Name}\";\n" +
			$"\ttextboxField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\ttextboxField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttextboxField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\ttextboxField.Description = null;\n";
			}
			else
			{
				response += $"\ttextboxField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\ttextboxField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttextboxField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\ttextboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\ttextboxField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\ttextboxField.DefaultValue = \"{field.DefaultValue?.Replace("\"","\\\"")}\";\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\ttextboxField.MaxLength = null;\n";
			}
			else
			{
				response += $"\ttextboxField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			response +=

			$"\ttextboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Permissions = new FieldPermissions();\n" +
			$"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textboxField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateUrlFieldCode(DbUrlField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

		$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
		"{\n" +
			$"\tInputUrlField urlField = new InputUrlField();\n" +
			$"\turlField.Id = new Guid(\"{field.Id}\");\n" +
			$"\turlField.Name = \"{field.Name}\";\n" +
			$"\turlField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\turlField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\turlField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\turlField.Description = null;\n";
			}
			else
			{
				response += $"\turlField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\turlField.HelpText = null;\n";
			}
			else
			{
				response += $"\turlField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\turlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.System = {(field.System).ToString().ToLowerInvariant()};\n";
			if (field.DefaultValue == null)
			{
				response += $"\turlField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\turlField.DefaultValue = \"{field.DefaultValue}\";\n";
			}
			if (field.MaxLength == null)
			{
				response += $"\turlField.MaxLength = null;\n";
			}
			else
			{
				response += $"\turlField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
			}
			response +=
			$"\turlField.OpenTargetInNewWindow = {(field.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
			$"\turlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Permissions = new FieldPermissions();\n" +
			$"\turlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), urlField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateTreeSelectFieldCode(DbTreeSelectField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
	$"\tInputTreeSelectField treeSelectField = new InputTreeSelectField();\n" +
	$"\ttreeSelectField.Id = new Guid(\"{field.Id}\");\n" +
	$"\ttreeSelectField.Name = \"{field.Name}\";\n" +
	$"\ttreeSelectField.Label = \"{field.Label}\";\n";
			if (field.PlaceholderText == null)
			{
				response += $"\ttreeSelectField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.PlaceholderText = \"{field.PlaceholderText}\";\n";
			}
			if (field.Description == null)
			{
				response += $"\ttreeSelectField.Description = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.Description = \"{field.Description}\";\n";
			}
			if (field.HelpText == null)
			{
				response += $"\ttreeSelectField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.HelpText = \"{field.HelpText}\";\n";
			}

			response +=
			$"\ttreeSelectField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.System = {(field.System).ToString().ToLowerInvariant()};\n";

			response +=
			$"\ttreeSelectField.RelatedEntityId = new Guid(\"{field.RelatedEntityId}\");\n" +
			$"\ttreeSelectField.RelationId = new Guid(\"{field.RelationId}\");\n" +
			$"\ttreeSelectField.SelectedTreeId = new Guid(\"{field.SelectedTreeId}\");\n" +
			$"\ttreeSelectField.SelectionTarget = \"{field.SelectionTarget}\";\n" +
			$"\ttreeSelectField.SelectionType = \"{field.SelectionType}\";\n" +


			$"\ttreeSelectField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Permissions = new FieldPermissions();\n" +
			$"\ttreeSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttreeSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\ttreeSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\ttreeSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), treeSelectField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}


		private string DeleteFieldCode(DbBaseField field, Guid entityId, string entityName)
		{
			var response =

		$"#region << ***Delete field*** Entity: {entityName} Field Name: {field.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteField(new Guid(\"{entityId}\"),new Guid(\"{field.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Delete field failed for Field: {field.Name} Entity: {entityName}. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateFieldCode(DbBaseField currentField, DbBaseField oldField, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;

			//escape possible double quotes
			if (currentField.HelpText != null)
				currentField.HelpText = currentField.HelpText.Replace("\"", "\\\"");
			if (currentField.PlaceholderText != null)
				currentField.PlaceholderText = currentField.PlaceholderText.Replace("\"", "\\\"");
			if (currentField.Label != null)
				currentField.Label = currentField.Label.Replace("\"", "\\\"");
			if (currentField.Description != null)
				currentField.Description = currentField.Description.Replace("\"", "\\\"");
			//escape the old field values the same way so the update check is correct later
			if (oldField.HelpText != null)
				oldField.HelpText = oldField.HelpText.Replace("\"", "\\\"");
			if (oldField.PlaceholderText != null)
				oldField.PlaceholderText = oldField.PlaceholderText.Replace("\"", "\\\"");
			if (oldField.Label != null)
				oldField.Label = oldField.Label.Replace("\"", "\\\"");
			if (oldField.Description != null)
				oldField.Description = oldField.Description.Replace("\"", "\\\"");

			//Check if field type is updated
			if (currentField.GetFieldType() != oldField.GetFieldType())
			{
				response.Code = "";
				response.HasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>field</span>  with name <span class='go-red'>{oldField.Name}</span> has its type updated from <span class='go-red'>{oldField.GetFieldType()}</span> to <span class='go-red'>{currentField.GetFieldType()}</span>");
				return response;
			}

			switch (currentField.GetFieldType())
			{
				case FieldType.AutoNumberField:
					{
						var responseCode = UpdateAutoNumberFieldCode(currentField as DbAutoNumberField, oldField as DbAutoNumberField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.CheckboxField:
					{
						var responseCode = UpdateCheckboxFieldCode(currentField as DbCheckboxField, oldField as DbCheckboxField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.CurrencyField:
					{
						var responseCode = UpdateCurrencyFieldCode(currentField as DbCurrencyField, oldField as DbCurrencyField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.DateField:
					{
						var responseCode = UpdateDateFieldCode(currentField as DbDateField, oldField as DbDateField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.DateTimeField:
					{
						var responseCode = UpdateDateTimeFieldCode(currentField as DbDateTimeField, oldField as DbDateTimeField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.EmailField:
					{
						var responseCode = UpdateEmailFieldCode(currentField as DbEmailField, oldField as DbEmailField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.FileField:
					{
						var responseCode = UpdateFileFieldCode(currentField as DbFileField, oldField as DbFileField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.ImageField:
					{
						var responseCode = UpdateImageFieldCode(currentField as DbImageField, oldField as DbImageField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.HtmlField:
					{
						var responseCode = UpdateHtmlFieldCode(currentField as DbHtmlField, oldField as DbHtmlField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.MultiLineTextField:
					{
						var responseCode = UpdateMultiLineTextFieldCode(currentField as DbMultiLineTextField, oldField as DbMultiLineTextField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.MultiSelectField:
					{
						var responseCode = UpdateMultiSelectFieldCode(currentField as DbMultiSelectField, oldField as DbMultiSelectField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.NumberField:
					{
						var responseCode = UpdateNumberFieldCode(currentField as DbNumberField, oldField as DbNumberField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PasswordField:
					{
						var responseCode = UpdatePasswordFieldCode(currentField as DbPasswordField, oldField as DbPasswordField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PercentField:
					{
						var responseCode = UpdatePercentFieldCode(currentField as DbPercentField, oldField as DbPercentField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PhoneField:
					{
						var responseCode = UpdatePhoneFieldCode(currentField as DbPhoneField, oldField as DbPhoneField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.GuidField:
					{
						var responseCode = UpdateGuidFieldCode(currentField as DbGuidField, oldField as DbGuidField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.SelectField:
					{
						var responseCode = UpdateSelectFieldCode(currentField as DbSelectField, oldField as DbSelectField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.TextField:
					{
						var responseCode = UpdateTextFieldCode(currentField as DbTextField, oldField as DbTextField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.UrlField:
					{
						var responseCode = UpdateUrlFieldCode(currentField as DbUrlField, oldField as DbUrlField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.TreeSelectField:
					{
						var responseCode = UpdateTreeSelectFieldCode(currentField as DbTreeSelectField, oldField as DbTreeSelectField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
			}

			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>field</span>  with name <span class='go-red'>{oldField.Name}</span> was updated");
			return response;
		}

		private string UpdateAutoNumberFieldCode(DbAutoNumberField currentField, DbAutoNumberField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***   Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
			$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			"\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
			$"\tautonumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\tautonumberField.Name = \"{currentField.Name}\";\n" +
			$"\tautonumberField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tautonumberField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tautonumberField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tautonumberField.Description = null;\n";
			}
			else
			{
				response += $"\tautonumberField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tautonumberField.HelpText = null;\n";
			}
			else
			{
				response += $"\tautonumberField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tautonumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tautonumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tautonumberField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tautonumberField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.DisplayFormat == null)
			{
				response += $"\tautonumberField.DisplayFormat = null;\n";
			}
			else
			{
				response += $"\tautonumberField.DisplayFormat = \"{currentField.DisplayFormat}\";\n";
			}
			if (currentField.StartingNumber == null)
			{
				response += $"\tautonumberField.StartingNumber = null;\n";
			}
			else
			{
				response += $"\tautonumberField.StartingNumber = Decimal.Parse(\"{currentField.StartingNumber}\");\n";
			}
			response +=
			$"\tautonumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tautonumberField.Permissions = new FieldPermissions();\n" +
			"\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
			"\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), autonumberField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion
			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.DisplayFormat != oldField.DisplayFormat)
			{
				hasUpdate = true;
			}
			else if (currentField.StartingNumber != oldField.StartingNumber)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateCheckboxFieldCode(DbCheckboxField currentField, DbCheckboxField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
			$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			"\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
			$"\tcheckboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\tcheckboxField.Name = \"{currentField.Name}\";\n" +
			$"\tcheckboxField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tcheckboxField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tcheckboxField.Description = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tcheckboxField.HelpText = null;\n";
			}
			else
			{
				response += $"\tcheckboxField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tcheckboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.DefaultValue = {(currentField.DefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tcheckboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tcheckboxField.Permissions = new FieldPermissions();\n" +
			"\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
			"\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), checkboxField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateCurrencyFieldCode(DbCurrencyField currentField, DbCurrencyField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
				$"\tcurrencyField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tcurrencyField.Name = \"{currentField.Name}\";\n" +
				$"\tcurrencyField.Label =  \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tcurrencyField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tcurrencyField.Description = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tcurrencyField.HelpText = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tcurrencyField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tcurrencyField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tcurrencyField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.MinValue == null)
			{
				response += $"\tcurrencyField.MinValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
			}
			if (currentField.MaxValue == null)
			{
				response += $"\tcurrencyField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tcurrencyField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
			}

			response +=
			$"\tcurrencyField.Currency = WebVella.ERP.Utilities.Helpers.GetCurrencyTypeObject(\"{currentField.Currency.Code}\");\n" +
			$"\tcurrencyField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tcurrencyField.Permissions = new FieldPermissions();\n" +
			"\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
			"\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), currencyField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			else if (currentField.Currency.Code != oldField.Currency.Code)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}



			return response;
		}

		private string UpdateDateFieldCode(DbDateField currentField, DbDateField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputDateField dateField = new InputDateField();\n" +
				$"\tdateField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tdateField.Name = \"{currentField.Name}\";\n" +
				$"\tdateField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tdateField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdateField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tdateField.Description = null;\n";
			}
			else
			{
				response += $"\tdateField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tdateField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdateField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\tdateField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tdateField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdateField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.Format == null)
			{
				response += $"\tdateField.Format = null;\n";
			}
			else
			{
				response += $"\tdateField.Format = \"{currentField.Format}\";\n";
			}

			response +=
			$"\tdateField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tdateField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tdateField.Permissions = new FieldPermissions();\n" +
			"\tdateField.Permissions.CanRead = new List<Guid>();\n" +
			"\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dateField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			else if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateDateTimeFieldCode(DbDateTimeField currentField, DbDateTimeField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
				$"\tdatetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tdatetimeField.Name = \"{currentField.Name}\";\n" +
				$"\tdatetimeField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tdatetimeField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tdatetimeField.Description = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tdatetimeField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\tdatetimeField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tdatetimeField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.Format == null)
			{
				response += $"\tdatetimeField.Format = null;\n";
			}
			else
			{
				response += $"\tdatetimeField.Format = \"{currentField.Format}\";\n";
			}

			response +=
			$"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
			$"\tdatetimeField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			"\tdatetimeField.Permissions = new FieldPermissions();\n" +
			"\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
			"\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), datetimeField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			else if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateEmailFieldCode(DbEmailField currentField, DbEmailField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			response =

			#region << Code >>
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputEmailField emailField = new InputEmailField();\n" +
				$"\temailField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\temailField.Name = \"{currentField.Name}\";\n" +
				$"\temailField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\temailField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\temailField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\temailField.Description = null;\n";
			}
			else
			{
				response += $"\temailField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\temailField.HelpText = null;\n";
			}
			else
			{
				response += $"\temailField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\temailField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\temailField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\temailField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\temailField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\temailField.MaxLength = null;\n";
			}
			else
			{
				response += $"\temailField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			response +=
			$"\temailField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\temailField.Permissions = new FieldPermissions();\n" +
			$"\temailField.Permissions.CanRead = new List<Guid>();\n" +
			$"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), emailField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateFileFieldCode(DbFileField currentField, DbFileField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputFileField fileField = new InputFileField();\n" +
				$"\tfileField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tfileField.Name = \"{currentField.Name}\";\n" +
				$"\tfileField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tfileField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tfileField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tfileField.Description = null;\n";
			}
			else
			{
				response += $"\tfileField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tfileField.HelpText = null;\n";
			}
			else
			{
				response += $"\tfileField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\tfileField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tfileField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tfileField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			response +=
			$"\tfileField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tfileField.Permissions = new FieldPermissions();\n" +
			$"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), fileField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{

				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateImageFieldCode(DbImageField currentField, DbImageField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputImageField imageField = new InputImageField();\n" +
				$"\timageField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\timageField.Name = \"{currentField.Name}\";\n" +
				$"\timageField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\timageField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\timageField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\timageField.Description = null;\n";
			}
			else
			{
				response += $"\timageField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\timageField.HelpText = null;\n";
			}
			else
			{
				response += $"\timageField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\timageField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Auditable =  {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\timageField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\timageField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\timageField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			response +=
			$"\timageField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\timageField.Permissions = new FieldPermissions();\n" +
			$"\timageField.Permissions.CanRead = new List<Guid>();\n" +
			$"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), imageField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateHtmlFieldCode(DbHtmlField currentField, DbHtmlField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputHtmlField htmlField = new InputHtmlField();\n" +
				$"\thtmlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\thtmlField.Name = \"{currentField.Name}\";\n" +
				$"\thtmlField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\thtmlField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\thtmlField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\thtmlField.Description = null;\n";
			}
			else
			{
				response += $"\thtmlField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\thtmlField.HelpText = null;\n";
			}
			else
			{
				response += $"\thtmlField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\thtmlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\thtmlField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\thtmlField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			response +=
			$"\thtmlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\thtmlField.Permissions = new FieldPermissions();\n" +
			$"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), htmlField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateMultiLineTextFieldCode(DbMultiLineTextField currentField, DbMultiLineTextField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
				$"\ttextareaField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\ttextareaField.Name = \"{currentField.Name}\";\n" +
				$"\ttextareaField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\ttextareaField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttextareaField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\ttextareaField.Description = null;\n";
			}
			else
			{
				response += $"\ttextareaField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\ttextareaField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttextareaField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\ttextareaField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\ttextareaField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\ttextareaField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\ttextareaField.MaxLength = null;\n";
			}
			else
			{
				response += $"\ttextareaField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			if (currentField.VisibleLineNumber == null)
			{
				response += $"\ttextareaField.VisibleLineNumber = null;\n";
			}
			else
			{
				response += $"\ttextareaField.VisibleLineNumber = Int32.Parse(\"{currentField.VisibleLineNumber}\");\n";
			}

			response +=
			$"\ttextareaField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextareaField.Permissions = new FieldPermissions();\n" +
			$"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textareaField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.VisibleLineNumber != oldField.VisibleLineNumber)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateMultiSelectFieldCode(DbMultiSelectField currentField, DbMultiSelectField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
				$"\tmultiSelectField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tmultiSelectField.Name = \"{currentField.Name}\";\n" +
				$"\tmultiSelectField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tmultiSelectField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tmultiSelectField.Description = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tmultiSelectField.HelpText = null;\n";
			}
			else
			{
				response += $"\tmultiSelectField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
				$"\tmultiSelectField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				"\tmultiSelectField.DefaultValue = ";
			var defaultValues = currentField.DefaultValue.ToList();
			if (defaultValues.Count > 0)
			{
				response += "new List<string>() {";

				for (int i = 0; i < defaultValues.Count; i++)
				{
					response += "\"" + defaultValues[i] + "\"";
					if (i < defaultValues.Count - 1)
					{
						response += ",";
					}
				}
				response += "};\n";
			}
			else
			{
				response += "null;\n";
			}
			response += "\tmultiSelectField.Options = ";
			var fieldOptions = currentField.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<MultiSelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew MultiSelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=

			$"\tmultiSelectField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
			$"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), multiSelectField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else if (currentField.Options.Count != oldField.Options.Count)
			{
				hasUpdate = true;
			}
			else
			{
				var oldDefaultValuesDictionary = new Dictionary<string, bool>();
				var newOptionsDictionary = new Dictionary<string, string>();

                if (oldField.DefaultValue == null)
                    oldField.DefaultValue = new List<string>();
                //create dictionary
                foreach (var value in oldField.DefaultValue.ToList())
				{
					oldDefaultValuesDictionary[value] = true;
				}
				foreach (var value in currentField.DefaultValue.ToList())
				{
					if (!oldDefaultValuesDictionary.ContainsKey(value))
					{
						hasUpdate = true;
					}
				}

				var oldOptionsDictionary = new Dictionary<string, string>();
				//create dictionary
				foreach (var value in oldField.Options.ToList())
				{
					oldOptionsDictionary[value.Key] = value.Value;
				}
				foreach (var value in currentField.Options.ToList())
				{
					newOptionsDictionary[value.Key] = value.Value;
					if (!oldOptionsDictionary.ContainsKey(value.Key) || oldOptionsDictionary[value.Key] != value.Value)
					{
						hasUpdate = true;
					}
				}
				foreach (var value in oldField.Options.ToList())
				{
					if (!newOptionsDictionary.ContainsKey(value.Key) || newOptionsDictionary[value.Key] != value.Value)
					{
						hasUpdate = true;
					}
				}

				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateNumberFieldCode(DbNumberField currentField, DbNumberField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputNumberField numberField = new InputNumberField();\n" +
				$"\tnumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tnumberField.Name = \"{currentField.Name}\";\n" +
				$"\tnumberField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tnumberField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tnumberField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tnumberField.Description = null;\n";
			}
			else
			{
				response += $"\tnumberField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tnumberField.HelpText = null;\n";
			}
			else
			{
				response += $"\tnumberField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tnumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tnumberField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.MinValue == null)
			{
				response += $"\tnumberField.MinValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
			}
			if (currentField.MaxValue == null)
			{
				response += $"\tnumberField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tnumberField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
			}
			response += $"\tnumberField.DecimalPlaces = byte.Parse(\"{currentField.DecimalPlaces}\");\n";
			response +=
			$"\tnumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tnumberField.Permissions = new FieldPermissions();\n" +
			$"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), numberField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			else if (currentField.DecimalPlaces != oldField.DecimalPlaces)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdatePasswordFieldCode(DbPasswordField currentField, DbPasswordField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputPasswordField passwordField = new InputPasswordField();\n" +
	$"\tpasswordField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tpasswordField.Name = \"{currentField.Name}\";\n" +
	$"\tpasswordField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tpasswordField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tpasswordField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tpasswordField.Description = null;\n";
			}
			else
			{
				response += $"\tpasswordField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tpasswordField.HelpText = null;\n";
			}
			else
			{
				response += $"\tpasswordField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tpasswordField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

			if (currentField.MinLength == null)
			{
				response += $"\tpasswordField.MinLength = null;\n";
			}
			else
			{
				response += $"\tpasswordField.MinLength = Int32.Parse(\"{currentField.MinLength}\");\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\tpasswordField.MaxLength = null;\n";
			}
			else
			{
				response += $"\tpasswordField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			response +=

			$"\tpasswordField.Encrypted = {(currentField.Encrypted).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tpasswordField.Permissions = new FieldPermissions();\n" +
			$"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), passwordField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.MinLength != oldField.MinLength)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.Encrypted != oldField.Encrypted)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}
			return response;
		}

		private string UpdatePercentFieldCode(DbPercentField currentField, DbPercentField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputPercentField percentField = new InputPercentField();\n" +
				$"\tpercentField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tpercentField.Name = \"{currentField.Name}\";\n" +
				$"\tpercentField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tpercentField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tpercentField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tpercentField.Description = null;\n";
			}
			else
			{
				response += $"\tpercentField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tpercentField.HelpText = null;\n";
			}
			else
			{
				response += $"\tpercentField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tpercentField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tpercentField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
			}
			if (currentField.MinValue == null)
			{
				response += $"\tpercentField.MinValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
			}
			if (currentField.MaxValue == null)
			{
				response += $"\tpercentField.MaxValue = null;\n";
			}
			else
			{
				response += $"\tpercentField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
			}
			response += $"\tpercentField.DecimalPlaces = byte.Parse(\"{currentField.DecimalPlaces}\");\n";
			response +=
			$"\tpercentField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tpercentField.Permissions = new FieldPermissions();\n" +
			$"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), percentField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			else if (currentField.DecimalPlaces != oldField.DecimalPlaces)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdatePhoneFieldCode(DbPhoneField currentField, DbPhoneField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
		$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
		"{\n" +
			$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			$"\tInputPhoneField phoneField = new InputPhoneField();\n" +
			$"\tphoneField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\tphoneField.Name = \"{currentField.Name}\";\n" +
			$"\tphoneField.Label =  \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tphoneField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tphoneField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tphoneField.Description = null;\n";
			}
			else
			{
				response += $"\tphoneField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tphoneField.HelpText = null;\n";
			}
			else
			{
				response += $"\tphoneField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tphoneField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tphoneField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tphoneField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\tphoneField.MaxLength = null;\n";
			}
			else
			{
				response += $"\tphoneField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			if (currentField.Format == null)
			{
				response += $"\tphoneField.Format = null;\n";
			}
			else
			{
				response += $"\tphoneField.Format = \"{currentField.Format}\";\n";
			}
			response +=
			$"\tphoneField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tphoneField.Permissions = new FieldPermissions();\n" +
			$"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), phoneField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateGuidFieldCode(DbGuidField currentField, DbGuidField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputGuidField guidField = new InputGuidField();\n" +
	$"\tguidField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tguidField.Name = \"{currentField.Name}\";\n" +
	$"\tguidField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tguidField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tguidField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tguidField.Description = null;\n";
			}
			else
			{
				response += $"\tguidField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tguidField.HelpText = null;\n";
			}
			else
			{
				response += $"\tguidField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tguidField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tguidField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tguidField.DefaultValue = Guid.Parse(\"{currentField.DefaultValue}\");\n";
			}

			response +=
			$"\tguidField.GenerateNewId = {(currentField.GenerateNewId).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tguidField.Permissions = new FieldPermissions();\n" +
			$"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), guidField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.GenerateNewId != oldField.GenerateNewId)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateSelectFieldCode(DbSelectField currentField, DbSelectField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputSelectField dropdownField = new InputSelectField();\n" +
	$"\tdropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tdropdownField.Name = \"{currentField.Name}\";\n" +
	$"\tdropdownField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\tdropdownField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\tdropdownField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\tdropdownField.Description = null;\n";
			}
			else
			{
				response += $"\tdropdownField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\tdropdownField.HelpText = null;\n";
			}
			else
			{
				response += $"\tdropdownField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\tdropdownField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\tdropdownField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\tdropdownField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}

			response += "\tdropdownField.Options = ";
			var fieldOptions = currentField.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<SelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew SelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=
			$"\tdropdownField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Permissions = new FieldPermissions();\n" +
			$"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dropdownField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else if (currentField.Options.Count != oldField.Options.Count) {
				hasUpdate = true;
			}
			else
			{
				var oldOptionsDictionary = new Dictionary<string, string>();
				var newOptionsDictionary = new Dictionary<string, string>();
				//create dictionary
				foreach (var value in oldField.Options.ToList())
				{
					oldOptionsDictionary[value.Key] = value.Value;
				}
				foreach (var value in currentField.Options.ToList())
				{
					newOptionsDictionary[value.Key] = value.Value;
					if (!oldOptionsDictionary.ContainsKey(value.Key) || oldOptionsDictionary[value.Key] != value.Value)
					{
						hasUpdate = true;
					}
				}
				foreach (var value in oldField.Options.ToList())
				{
					if (!newOptionsDictionary.ContainsKey(value.Key) || newOptionsDictionary[value.Key] != value.Value)
					{
						hasUpdate = true;
					}
				}

				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateTextFieldCode(DbTextField currentField, DbTextField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>

			response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
		$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputTextField textboxField = new InputTextField();\n" +
			$"\ttextboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\ttextboxField.Name = \"{currentField.Name}\";\n" +
			$"\ttextboxField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\ttextboxField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttextboxField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\ttextboxField.Description = null;\n";
			}
			else
			{
				response += $"\ttextboxField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\ttextboxField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttextboxField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\ttextboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\ttextboxField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\ttextboxField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\ttextboxField.MaxLength = null;\n";
			}
			else
			{
				response += $"\ttextboxField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			response +=
			$"\ttextboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Permissions = new FieldPermissions();\n" +
			$"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textboxField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) { 
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateUrlFieldCode(DbUrlField currentField, DbUrlField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

		$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
		"{\n" +
			$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			$"\tInputUrlField urlField = new InputUrlField();\n" +
			$"\turlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\turlField.Name = \"{currentField.Name}\";\n" +
			$"\turlField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\turlField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\turlField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\turlField.Description = null;\n";
			}
			else
			{
				response += $"\turlField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\turlField.HelpText = null;\n";
			}
			else
			{
				response += $"\turlField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\turlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
			if (currentField.DefaultValue == null)
			{
				response += $"\turlField.DefaultValue = null;\n";
			}
			else
			{
				response += $"\turlField.DefaultValue = \"{currentField.DefaultValue}\";\n";
			}
			if (currentField.MaxLength == null)
			{
				response += $"\turlField.MaxLength = null;\n";
			}
			else
			{
				response += $"\turlField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
			}
			response +=
			$"\turlField.OpenTargetInNewWindow = {(currentField.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
			$"\turlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Permissions = new FieldPermissions();\n" +
			$"\turlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), urlField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			else if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			else if (currentField.OpenTargetInNewWindow != oldField.OpenTargetInNewWindow)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateTreeSelectFieldCode(DbTreeSelectField currentField, DbTreeSelectField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputTreeSelectField treeSelectField = new InputTreeSelectField();\n" +
	$"\ttreeSelectField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\ttreeSelectField.Name = \"{currentField.Name}\";\n" +
	$"\ttreeSelectField.Label = \"{currentField.Label}\";\n";
			if (currentField.PlaceholderText == null)
			{
				response += $"\ttreeSelectField.PlaceholderText = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
			}
			if (currentField.Description == null)
			{
				response += $"\ttreeSelectField.Description = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.Description = \"{currentField.Description}\";\n";
			}
			if (currentField.HelpText == null)
			{
				response += $"\ttreeSelectField.HelpText = null;\n";
			}
			else
			{
				response += $"\ttreeSelectField.HelpText = \"{currentField.HelpText}\";\n";
			}

			response +=
			$"\ttreeSelectField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

			response +=
			$"\ttreeSelectField.RelatedEntityId = new Guid(\"{currentField.RelatedEntityId}\");\n" +
			$"\ttreeSelectField.RelationId = new Guid(\"{currentField.RelationId}\");\n" +
			$"\ttreeSelectField.SelectedTreeId = new Guid(\"{currentField.SelectedTreeId}\");\n" +
			$"\ttreeSelectField.SelectionTarget = \"{currentField.SelectionTarget}\";\n" +
			$"\ttreeSelectField.SelectionType = \"{currentField.SelectionType}\";\n" +


			$"\ttreeSelectField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttreeSelectField.Permissions = new FieldPermissions();\n" +
			$"\ttreeSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttreeSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\ttreeSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\ttreeSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), treeSelectField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (oldField == null) //oldField is null where its field type is different from currentField
			{
				hasUpdate = true;
			}
			else if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			else if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			else if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			else if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			else if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			else if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			else if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			else if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			else if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			else if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			else if (currentField.RelatedEntityId != oldField.RelatedEntityId)
			{
				hasUpdate = true;
			}
			else if (currentField.RelationId != oldField.RelationId)
			{
				hasUpdate = true;
			}
			else if (currentField.SelectedTreeId != oldField.SelectedTreeId)
			{
				hasUpdate = true;
			}
			else if (currentField.SelectionTarget != oldField.SelectionTarget)
			{
				hasUpdate = true;
			}
			else if (currentField.SelectionType != oldField.SelectionType)
			{
				hasUpdate = true;
			}
			else if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}
			else
			{
				// Permissions change check
				if(CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions)) {
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		#endregion // End field region

		#region << View >>
		private string CreateViewCode(DbRecordView view, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (view.Label != null)
				view.Label = view.Label.Replace("\"", "\\\"");
			if (view.Title != null)
				view.Title = view.Title.Replace("\"", "\\\"");
			response +=
		   $"#region << View  Entity: {entityName} name: {view.Name} >>\n" +
		   "{\n" +
			   $"\tvar createViewEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			   $"\tvar createViewInput = new InputRecordView();\n\n" +
			   $"\t#region << details >>\n" +
			   $"\tcreateViewInput.Id = new Guid(\"{view.Id}\");\n" +
			   $"\tcreateViewInput.Type = \"{view.Type}\";\n" +
			   $"\tcreateViewInput.Name = \"{view.Name}\";\n" +
			   $"\tcreateViewInput.Label = \"{view.Label}\";\n";
			   if(view.Title == null) {
				response += $"\tcreateViewInput.Title = null;\n";
			   }
			   else {
			   response += $"\tcreateViewInput.Title = \"{view.Title}\";\n";
			   }
			   response +=
			   $"\tcreateViewInput.Default = {(view.Default).ToString().ToLowerInvariant()};\n" +
			   $"\tcreateViewInput.System = {(view.System).ToString().ToLowerInvariant()};\n";
			if (view.Weight == null)
			{
				response += $"\tcreateViewInput.Weight = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.Weight = Decimal.Parse(\"{view.Weight}\");\n";
			}
			if (view.CssClass == null)
			{
				response += $"\tcreateViewInput.CssClass = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.CssClass = \"{view.CssClass}\";\n";
			}
			if (view.IconName == null)
			{
				response += $"\tcreateViewInput.IconName = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.IconName = \"{view.IconName}\";\n";
			}
			if (view.DynamicHtmlTemplate == null)
			{
				response += $"\tcreateViewInput.DynamicHtmlTemplate = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.DynamicHtmlTemplate = \"{view.DynamicHtmlTemplate}\";\n";
			}
			if (view.DataSourceUrl == null)
			{
				response += $"\tcreateViewInput.DataSourceUrl = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.DataSourceUrl = \"{view.DataSourceUrl}\";\n";
			}
			if (view.ServiceCode == null)
			{
				response += $"\tcreateViewInput.ServiceCode = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.ServiceCode = @\"{view.ServiceCode.Replace("\"","\"\"")}\";\n";
			}

			response += $"\t#endregion\n\n" +
			//Region
			$"\t#region << regions >>\n" +
			$"\tcreateViewInput.Regions = new List<InputRecordViewRegion>();\n\n";
			foreach (var region in view.Regions)
			{
				response += CreateViewRegionCode(region, entityId, entityName);
			}
			response += $"\t#endregion\n\n";

			//Relation options
			response +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in view.RelationOptions)
			{
				response += CreateRelationOptionCode(relationOption, entityId, entityName,"view");
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			response +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in view.ActionItems)
			{
				response += CreateViewActionItemCode(actionItem, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";
			//Sidebar
			response += CreateViewSidebarCode(view.Sidebar, entityId, entityName);
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordView(new Guid(\"{entityId}\"), createViewInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Updated view: {view.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}
		//ViewRegion
		private string CreateViewRegionCode(DbRecordViewRegion region, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"\t\t#region << Region: {region.Name} >>\n" +
			"\t\t{\n" +
			$"\t\t\tvar viewRegion = new InputRecordViewRegion();\n" +
			$"\t\t\tviewRegion.Name = \"{region.Name}\";\n" +
			$"\t\t\tviewRegion.Label = \"{region.Label}\";\n" +
			$"\t\t\tviewRegion.Render = {(region.Render).ToString().ToLowerInvariant()};\n";
			if (region.Weight == null)
			{
				response += $"\t\t\tviewRegion.Weight = null;\n";
			}
			else
			{
				response += $"\t\t\tviewRegion.Weight = Decimal.Parse(\"{region.Weight}\");\n";
			}
			if (region.CssClass == null)
			{
				response += $"\t\t\tviewRegion.CssClass = null;\n";
			}
			else
			{
				response += $"\t\t\tviewRegion.CssClass = \"{region.CssClass}\";\n";
			}


			response +=
			$"\t\t\tviewRegion.Sections = new List<InputRecordViewSection>();\n\n";
			foreach (var section in region.Sections)
			{
				response += CreateViewSectionCode(section, entityId, entityName);
			}

			response +=
			$"\t\t\t//Save region\n" +
			$"\t\t\tcreateViewInput.Regions.Add(viewRegion);\n" +
			"\t\t}\n" +
			$"\t\t#endregion\n\n";

			return response;
		}

		//ViewSection
		private string CreateViewSectionCode(DbRecordViewSection section, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"\t\t\t#region << Section: {section.Name} >>\n" +
			"\t\t\t{\n" +
			$"\t\t\tvar viewSection = new InputRecordViewSection();\n" +
			$"\t\t\tviewSection.Id = new Guid(\"{section.Id}\");\n" +
			$"\t\t\tviewSection.Name = \"{section.Name}\";\n" +
			$"\t\t\tviewSection.Label = \"{section.Label}\";\n" +
			$"\t\t\tviewSection.ShowLabel = {(section.ShowLabel).ToString().ToLowerInvariant()};\n";
			if (section.CssClass == null)
			{
				response += $"\t\t\tviewSection.CssClass = null;\n";
			}
			else
			{
				response += $"\t\t\tviewSection.CssClass = \"{section.CssClass}\";\n";
			}

			response +=
			$"\t\t\tviewSection.Collapsed = {(section.Collapsed).ToString().ToLowerInvariant()};\n";

			if (section.TabOrder == null)
			{
				response += $"\t\t\tviewSection.TabOrder = null;\n";
			}
			else
			{
				response += $"\t\t\tviewSection.TabOrder = \"{section.TabOrder}\";\n";
			}
			if (section.Weight == null)
			{
				response += $"\t\t\tviewSection.Weight = null;\n";
			}
			else
			{
				response += $"\t\t\tviewSection.Weight = Decimal.Parse(\"{section.Weight}\");\n";
			}


			response +=
			$"\t\t\tviewSection.Rows = new List<InputRecordViewRow>();\n\n";
			var rowIndex = 1;
			foreach (var row in section.Rows)
			{
				response += CreateViewRowCode(row, entityId, entityName, rowIndex);
				rowIndex++;
			}
			response +=
			$"\t\t\t//Save section\n" +
			$"\t\t\tviewRegion.Sections.Add(viewSection);\n" +
			"\t\t\t}\n" +
			$"\t\t\t#endregion\n";

			return response;
		}

		//ViewRow
		private string CreateViewRowCode(DbRecordViewRow row, Guid entityId, string entityName, int rowIndex)
		{
			var response = string.Empty;
			response +=
			$"\t\t\t\t#region << Row {rowIndex}>>\n" +
			"\t\t\t\t{\n" +
			"\t\t\t\t\tvar viewRow = new InputRecordViewRow();\n" +
			$"\t\t\t\t\tviewRow.Id = new Guid(\"{row.Id}\");\n";

			if (row.Weight == null)
			{
				response += $"\t\t\t\t\tviewRow.Weight = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\tviewRow.Weight = Decimal.Parse(\"{row.Weight}\");\n";
			}

			response +=
			$"\t\t\t\t\tviewRow.Columns = new List<InputRecordViewColumn>();\n\n";
			var colIndex = 1;
			foreach (var column in row.Columns)
			{
				response += CreateViewColumnCode(column, entityId, entityName, colIndex);
				colIndex++;
			}
			response +=
			"\t\t\t\t//Save row\n" +
			"\t\t\t\tviewSection.Rows.Add(viewRow);\n" +
			"\t\t\t\t}\n" +
			"\t\t\t\t#endregion\n";
			return response;
		}

		//ViewColumn
		private string CreateViewColumnCode(DbRecordViewColumn column, Guid entityId, string entityName, int colIndex)
		{
			var response = string.Empty;
			response +=
			$"\t\t\t\t\t#region << Column {colIndex} >>\n" +
			"\t\t\t\t\t{\n" +
			$"\t\t\t\t\tvar viewColumn = new InputRecordViewColumn();\n";
			response += $"\t\t\t\t\tviewColumn.GridColCount = Int32.Parse(\"{column.GridColCount}\");\n";
			response +=
			$"\t\t\t\t\tviewColumn.Items = new List<InputRecordViewItemBase>();\n\n";
			foreach (var item in column.Items)
			{
				response += CreateViewItemCode(item, entityId, entityName);
			}
			response +=
			"\t\t\t\t\t//Save column\n" +
			"\t\t\t\t\tviewRow.Columns.Add(viewColumn);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";

			return response;
		}

		//field
		private string CreateViewItemCode(DbRecordViewItemBase item, Guid entityId, string entityName)
		{
			var response = string.Empty;
			if (item is DbRecordViewFieldItem)
			{
				response += CreateRecordViewFieldItemCode(item as DbRecordViewFieldItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationFieldItem)
			{
				response += CreateRecordViewRelationFieldItemCode(item as DbRecordViewRelationFieldItem, entityId, entityName);
			}
			else if (item is DbRecordViewViewItem)
			{
				response += CreateRecordViewViewItemCode(item as DbRecordViewViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationViewItem)
			{
				response += CreateRecordViewRelationViewItemCode(item as DbRecordViewRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewListItem)
			{
				response += CreateRecordViewListItemCode(item as DbRecordViewListItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationListItem)
			{
				response += CreateRecordViewRelationListItemCode(item as DbRecordViewRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationTreeItem)
			{
				response += CreateRecordViewRelationTreeItemCode(item as DbRecordViewRelationTreeItem, entityId, entityName);
			}
			return response;
		}

		//field
		private string CreateRecordViewFieldItemCode(DbRecordViewFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			//var currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			Field currentField = null;
			try
			{
				currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			}
			catch (Exception ex)
			{
				var boz = ex;
			}
			response +=
			$"\t\t\t\t\t#region << {currentField.Name} >>\n" +
			"\t\t\t\t\t{\n" +
				"\t\t\t\t\t\tvar viewItem = new InputRecordViewFieldItem();\n" +
				$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
				$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
				$"\t\t\t\t\t\tviewItem.FieldId = new Guid(\"{fieldItem.FieldId}\");\n" +
				$"\t\t\t\t\t\tviewItem.FieldName = \"{currentField.Name}\";\n" +
				$"\t\t\t\t\t\tviewItem.Type = \"field\";\n" +
				$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//field from relation
		private string CreateRecordViewRelationFieldItemCode(DbRecordViewRelationFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(fieldItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(fieldItem.EntityId).Object;
			var relatedField = relatedEntity.Fields.SingleOrDefault(x => x.Id == fieldItem.FieldId);

			if (relatedField == null)
			{
				response += "\t\t\t\t\t/////////////////////////////////////////////////////////////////////////////\n";
				response += $"\t\t\t\t\t//WARNING: Field not found - fieldId {fieldItem.FieldId} and entity {relatedEntity.Name}\n";
				response += "\t\t\t\t\t/////////////////////////////////////////////////////////////////////////////\n";
				return response;
			}

			response +=
			$"\t\t\t\t\t#region << field from Relation: {relatedField.Name} >>\n" +
			"\t\t\t\t\t{\n" +
				"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationFieldItem();\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{fieldItem.EntityId}\");\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.Type = \"fieldFromRelation\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldId = new Guid(\"{relatedField.Id}\");\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldName = \"{relatedField.Name}\";\n";
			if (fieldItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{fieldItem.FieldLabel}\";\n";
			}
			if (fieldItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{fieldItem.FieldPlaceholder}\";\n";
			}
			if (fieldItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{fieldItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(fieldItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{fieldItem.FieldLookupList}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{fieldItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
		"\t\t\t\t\t}\n" +
		"\t\t\t\t\t#endregion\n";
			return response;
		}
		//view
		private string CreateRecordViewViewItemCode(DbRecordViewViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t\t\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItem = new InputRecordViewViewItem();\n" +
			$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\t\t\t\tviewItem.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\t\t\t\tviewItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\t\t\t\tviewItem.Type = \"view\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//view from relation
		private string CreateRecordViewRelationViewItemCode(DbRecordViewRelationViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordViewItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordViewItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t\t\t\t#region << View from relation: {relatedView.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationViewItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{recordViewItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ViewName = \"{relatedView.Name}\";\n";
			if (recordViewItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{recordViewItem.FieldLabel}\";\n";
			}
			if (recordViewItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{recordViewItem.FieldPlaceholder}\";\n";
			}
			if (recordViewItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{recordViewItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(recordViewItem.FieldRequired).ToString().ToLowerInvariant()};\n";

			if(recordViewItem.FieldLookupList == null) {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = null;\n";
			}
			else {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{recordViewItem.FieldLookupList}\";\n";
			}

			if(recordViewItem.FieldManageView == null) {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldManageView = null;\n";
			}
			else {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldManageView = \"{recordViewItem.FieldManageView}\";\n";
			}

			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{recordViewItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//list
		private string CreateRecordViewListItemCode(DbRecordViewListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t\t\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItem = new InputRecordViewListItem();\n" +
			$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\t\t\t\tviewItem.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\t\t\t\tviewItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\t\t\t\tviewItem.Type = \"list\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";

			return response;
		}
		//list from relation
		private string CreateRecordViewRelationListItemCode(DbRecordViewRelationListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(listItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(listItem.EntityId).Object;
			var relatedList = relatedEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t\t\t\t#region << List from relation: {relatedList.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationListItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{listItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ListName = \"{relatedList.Name}\";\n";
			if (listItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{listItem.FieldLabel}\";\n";
			}
			if (listItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{listItem.FieldPlaceholder}\";\n";
			}
			if (listItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{listItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(listItem.FieldRequired).ToString().ToLowerInvariant()};\n";

			if(listItem.FieldLookupList == null) {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = null;\n";
			}
			else {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{listItem.FieldLookupList}\";\n";
			}

			
			if(listItem.FieldManageView == null) {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldManageView = null;\n";
			}
			else {
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldManageView = \"{listItem.FieldManageView}\";\n";
			}
			
			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{listItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//tree from relation
		private string CreateRecordViewRelationTreeItemCode(DbRecordViewRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordTrees.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t\t\t\t#region << Tree from relation: {relatedTree.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationTreeItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{treeItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.TreeName = \"{relatedTree.Name}\";\n";
			if (treeItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n";
			}
			if (treeItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n";
			}
			if (treeItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}

		//Sidebar
		private string CreateViewSidebarCode(DbRecordViewSidebar sidebar, Guid entityId, string entityName)
		{
			var response = string.Empty;

			if (sidebar == null)
			{
				response +=
				"\t#region << Sidebar >>\n" +
				$"\tcreateViewInput.Sidebar.CssClass = \"\";\n" +
				$"\tcreateViewInput.Sidebar.Render = true;\n" +
				$"\tcreateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();\n\n" +
				"\t#endregion\n";
				return response;
			}

			response +=
			"\t#region << Sidebar >>\n" +
			"\tcreateViewInput.Sidebar = new InputRecordViewSidebar();\n";
			if (sidebar.CssClass == null)
			{
				response += $"\tcreateViewInput.Sidebar.CssClass = null;\n";
			}
			else
			{
				response += $"\tcreateViewInput.Sidebar.CssClass = \"{sidebar.CssClass}\";\n";
			}
			response +=
			$"\tcreateViewInput.Sidebar.Render = {(sidebar.Render).ToString().ToLowerInvariant()};\n" +
			$"\tcreateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();\n\n";
			foreach (var item in sidebar.Items)
			{
				response += CreateSidebarItemCode(item, entityId, entityName);
			}
			response +=
			"\t#endregion\n";
			return response;
		}

		private string CreateSidebarItemCode(DbRecordViewSidebarItemBase item, Guid entityId, string entityName)
		{
			var response = string.Empty;
			if (item is DbRecordViewSidebarViewItem)
			{
				response += CreateSidebarViewItemCode(item as DbRecordViewSidebarViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationViewItem)
			{
				response += CreateSidebarRelationViewItemCode(item as DbRecordViewSidebarRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarListItem)
			{
				response += CreateSidebarListItemCode(item as DbRecordViewSidebarListItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationListItem)
			{
				response += CreateSidebarRelationListItemCode(item as DbRecordViewSidebarRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationTreeItem)
			{
				response += CreateSidebarRelationTreeItemCode(item as DbRecordViewSidebarRelationTreeItem, entityId, entityName);
			}
			return response;
		}

		//sidebar - view
		private string CreateSidebarViewItemCode(DbRecordViewSidebarViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItem = new InputRecordViewSidebarViewItem();\n" +
			$"\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItem.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\tviewItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\tviewItem.Type = \"view\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";

			return response;
		}

		//sidebar - view from relation
		private string CreateSidebarRelationViewItemCode(DbRecordViewSidebarRelationViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordViewItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordViewItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t#region << view from relation: {relatedView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationViewItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{relatedEntity.Id}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\tviewItemFromRelation.ViewName =\"{relatedView.Name}\";\n";
			if (recordViewItem.FieldLabel == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldLabel = \"{recordViewItem.FieldLabel}\";\n";
			}
			if (recordViewItem.FieldPlaceholder == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldPlaceholder = \"{recordViewItem.FieldPlaceholder}\";\n";
			}
			if (recordViewItem.FieldHelpText == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldHelpText = \"{recordViewItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\tviewItemFromRelation.FieldRequired = {(recordViewItem.FieldRequired).ToString().ToLowerInvariant()};\n";

			if (recordViewItem.FieldManageView == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldManageView = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldManageView = \"{recordViewItem.FieldManageView}\";\n";
			}

			if (recordViewItem.FieldLookupList == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldLookupList = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldLookupList = \"{recordViewItem.FieldLookupList}\";\n";
			}
			response +=
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{recordViewItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";

			return response;
		}

		//sidebar - list
		private string CreateSidebarListItemCode(DbRecordViewSidebarListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItem = new InputRecordViewSidebarListItem();\n" +
			$"\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItem.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\tviewItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\tviewItem.Type = \"list\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//sidebar - list from relation
		private string CreateSidebarRelationListItemCode(DbRecordViewSidebarRelationListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(listItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(listItem.EntityId).Object;
			var relatedList = relatedEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t#region << list from relation: {relatedList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationListItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{relatedEntity.Id}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\tviewItemFromRelation.ListName =\"{relatedList.Name}\";\n";
			if (listItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{listItem.FieldLabel}\";\n";
			}
			if (listItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{listItem.FieldPlaceholder}\";\n";
			}
			if (listItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{listItem.FieldHelpText}\";\n";
			}

			response +=
			$"\t\t\tviewItemFromRelation.FieldRequired = {(listItem.FieldRequired).ToString().ToLowerInvariant()};\n";

			if (listItem.FieldManageView == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldManageView = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldManageView = \"{listItem.FieldManageView}\";\n";
			}

			if (listItem.FieldLookupList == null)
			{
				response += $"\t\t\tviewItemFromRelation.FieldLookupList = null;\n";
			}
			else
			{
				response += $"\t\t\tviewItemFromRelation.FieldLookupList = \"{listItem.FieldLookupList}\";\n";
			}

			response +=
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{listItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//sidebar - tree relation item
		private string CreateSidebarRelationTreeItemCode(DbRecordViewSidebarRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordLists.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t#region << list from relation: {relatedTree.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationTreeItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{relatedEntity.Id}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\tviewItemFromRelation.TreeName =\"{relatedTree.Name}\";\n";
			if (treeItem.FieldLabel == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n";
			}
			if (treeItem.FieldPlaceholder == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n";
			}
			if (treeItem.FieldHelpText == null)
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n";
			}
			response +=
			$"\t\t\tviewItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Action Item
		private string CreateViewActionItemCode(ActionItem actionItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			string lineSeparator = ((char)0x2028).ToString();
			string paragraphSeparator = ((char)0x2029).ToString();
			response +=
			$"\t\t#region << action item: {actionItem.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar actionItem = new ActionItem();\n" +
			$"\t\t\tactionItem.Name = \"{actionItem.Name}\";\n" +
			$"\t\t\tactionItem.Menu = \"{actionItem.Menu}\";\n";
			if (actionItem.Weight == null)
			{
				response += $"\t\t\tactionItem.Weight = null;\n";
			}
			else
			{
				response += $"\t\t\tactionItem.Weight = Decimal.Parse(\"{actionItem.Weight}\");\n";
			}

			response +=
			$"\t\t\tactionItem.Template = @\"{actionItem.Template.Replace("\"", "\"\"").Replace(System.Environment.NewLine, "\n")}\";\n" +
			$"\t\t\tcreateViewInput.ActionItems.Add(actionItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";

			return response;
		}

		//Relation option
		private string CreateRelationOptionCode(DbEntityRelationOptions relationOption, Guid entityId, string entityName, string type = "view")
		{
			var response = string.Empty;
			response +=
			$"#region << relation option name: {relationOption.RelationName} >>\n" +
			"{\n" +
			"\tvar relationOption = new EntityRelationOptionsItem();\n" +
			$"\trelationOption.RelationName = \"{relationOption.RelationName}\";\n" +
			$"\trelationOption.RelationId = new Guid(\"{relationOption.RelationId}\");\n" +
			$"\trelationOption.Direction = \"{relationOption.Direction}\";\n";
			switch(type) {
				case "list":
					response += $"\tcreateListInput.RelationOptions.Add(relationOption);\n";
					break;
				default:
					response += $"\tcreateViewInput.RelationOptions.Add(relationOption);\n";
					break;
			}
			
			response += "}\n" +
			"#endregion\n";
			return response;
		}

		//Delete
		private string DeleteViewCode(DbRecordView view, Guid entityId, string entityName)
		{
			var response =
		$"#region << ***Delete view***  Entity: {entityName} View Name: {view.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordView(new Guid(\"{entityId}\"),new Guid(\"{view.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. View: {view.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateViewCode(DbRecordView currentView, DbRecordView oldView, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;

			//escape possible double quotes
			if (currentView.Label != null)
				currentView.Label = currentView.Label.Replace("\"", "\\\"");
			if (currentView.Title != null)
				currentView.Title = currentView.Title.Replace("\"", "\\\"");
			//escape the old to so the update check is correct
			if (oldView.Label != null)
				oldView.Label = oldView.Label.Replace("\"", "\\\"");
			if (oldView.Title != null)
				oldView.Title = oldView.Title.Replace("\"", "\\\"");


			#region << Code >>
			code +=
		   $"#region << ***Update view***  Entity: {currentEntity.Name} View Name: {currentView.Name} >>\n" +
		   "{\n" +
			   $"\tvar updateViewEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			   $"\tvar createViewInput = new InputRecordView();\n\n" +
			   $"\t#region << details >>\n" +
			   $"\tcreateViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == \"{currentView.Name}\").Id;\n" +
			   $"\tcreateViewInput.Type = \"{currentView.Type}\";\n" +
			   $"\tcreateViewInput.Name = \"{currentView.Name}\";\n" +
			   $"\tcreateViewInput.Label = \"{currentView.Label}\";\n";

			if (currentView.Title == null)
			{
				code += $"\tcreateViewInput.Title = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.Title = \"{currentView.Title}\";\n";
			}
			code +=
			$"\tcreateViewInput.Default = {(currentView.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateViewInput.System = {(currentView.System).ToString().ToLowerInvariant()};\n";
			if (currentView.Weight == null)
			{
				code += $"\tcreateViewInput.Weight = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.Weight = Decimal.Parse(\"{currentView.Weight}\");\n";
			}
			if (currentView.CssClass == null)
			{
				code += $"\tcreateViewInput.CssClass = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.CssClass = \"{currentView.CssClass}\";\n";
			}
			if (currentView.IconName == null)
			{
				code += $"\tcreateViewInput.IconName = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.IconName = \"{currentView.IconName}\";\n";
			}
			if (currentView.DynamicHtmlTemplate == null)
			{
				code += $"\tcreateViewInput.DynamicHtmlTemplate = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.DynamicHtmlTemplate = \"{currentView.DynamicHtmlTemplate}\";\n";
			}
			if (currentView.DataSourceUrl == null)
			{
				code += $"\tcreateViewInput.DataSourceUrl = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.DataSourceUrl = \"{currentView.DataSourceUrl}\";\n";
			}
			if (currentView.ServiceCode == null)
			{
				code += $"\tcreateViewInput.ServiceCode = null;\n";
			}
			else
			{
				code += $"\tcreateViewInput.ServiceCode = @\"{currentView.ServiceCode.Replace("\"","\"\"")}\";\n";
			}

			code +=
			$"\t#endregion\n\n" +
			//Region
			$"\t#region << regions >>\n" +
			$"\tcreateViewInput.Regions = new List<InputRecordViewRegion>();\n\n";
			foreach (var region in currentView.Regions)
			{
				code += CreateViewRegionCode(region, currentEntity.Id, currentEntity.Name);
			}
			code += $"\t#endregion\n\n";

			//Relation options
			code +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in currentView.RelationOptions)
			{
				code += CreateRelationOptionCode(relationOption, currentEntity.Id, currentEntity.Name,"view");
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			code +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in currentView.ActionItems)
			{
				code += CreateViewActionItemCode(actionItem, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";
			//Sidebar
			code += CreateViewSidebarCode(currentView.Sidebar, currentEntity.Id, currentEntity.Name);
			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordView(new Guid(\"{currentEntity.Id}\"), createViewInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated view: {oldView.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Check change >>
			if (currentView.Type != oldView.Type)
			{
				hasUpdate = true;
			}
			if (currentView.Name != oldView.Name)
			{
				hasUpdate = true;
			}
			if (currentView.Label != oldView.Label)
			{
				hasUpdate = true;
			}
			if (currentView.Title != oldView.Title &&
				!(currentView.Title == null && oldView.Title == "") &&
				!(currentView.Title == "" && oldView.Title == null))
			{
				hasUpdate = true;
			}
			if (currentView.Default != oldView.Default)
			{
				hasUpdate = true;
			}
			if (currentView.System != oldView.System)
			{
				hasUpdate = true;
			}
			if (currentView.Weight != oldView.Weight)
			{
				hasUpdate = true;
			}
			if (currentView.CssClass != oldView.CssClass)
			{
				hasUpdate = true;
			}
			if (currentView.IconName != oldView.IconName)
			{
				hasUpdate = true;
			}
			if (currentView.DynamicHtmlTemplate != oldView.DynamicHtmlTemplate)
			{
				hasUpdate = true;
			}
			if (currentView.DataSourceUrl != oldView.DataSourceUrl)
			{
				hasUpdate = true;
			}

			//Because of the \r\n <> \n problem we will need to artificially add \r to the new view templates and test again
			if(!String.IsNullOrWhiteSpace(oldView.ServiceCode)) {
				var fixedServiceCode = oldView.ServiceCode.Replace("\n", "").Replace("\r", "");
				var fixedCurrrentServiceCode = currentView.ServiceCode.Replace("\n", "").Replace("\r", "");
				if (fixedCurrrentServiceCode != fixedServiceCode)
				{
					hasUpdate = true;
				}
			}else {
				if (currentView.ServiceCode != oldView.ServiceCode)
				{
					hasUpdate = true;
				}			
			}

			if (JsonConvert.SerializeObject(currentView.Regions) != JsonConvert.SerializeObject(oldView.Regions))
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.RelationOptions) != JsonConvert.SerializeObject(oldView.RelationOptions))
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.ActionItems) != JsonConvert.SerializeObject(oldView.ActionItems))
			{
				//Because of the \r\n <> \n problem we will need to artificially add \r to the new view templates and test again
				var differenceInTemplateFound = false;
				//create an action item dictionary for the oldView
				var oldActionsDictionary = new Dictionary<string, ActionItem>();
				foreach (var item in oldView.ActionItems)
				{
					var dictionaryKey = item.Name + "-" + item.Menu;
					oldActionsDictionary[dictionaryKey] = item;
				}

				foreach (var item in currentView.ActionItems)
				{
					//check if item with the same name-menu exists in the old menu
					var dictionaryKey = item.Name + "-" + item.Menu;
					if (!oldActionsDictionary.ContainsKey(dictionaryKey))
					{
						differenceInTemplateFound = true;
						break;
					}
					else
					{
						var correspondingActionItem = oldActionsDictionary[dictionaryKey];
						var oldTemplateFixed = correspondingActionItem.Template.Replace("\n", "").Replace("\r", "");
						var newTemplateFixed = item.Template.Replace("\n", "").Replace("\r", "");
						if (oldTemplateFixed != newTemplateFixed)
						{
							differenceInTemplateFound = true;
						}
					}
				}
				if (differenceInTemplateFound)
				{
					hasUpdate = true;
				}
			}

			if (JsonConvert.SerializeObject(currentView.Sidebar) != JsonConvert.SerializeObject(oldView.Sidebar))
			{
				hasUpdate = true;
			}

			#endregion


			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>view</span>  with name <span class='go-red'>{oldView.Name}</span> was updated");
			return response;
		}

		#endregion

		#region << List >>
		private string CreateListCode(DbRecordList list, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (list.Label != null)
				list.Label = list.Label.Replace("\"", "\\\"");

			response +=
			$"#region << List  Entity: {entityName} name: {list.Name} >>\n" +
			"{\n" +
			$"\tvar createListEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			$"\tvar createListInput = new InputRecordList();\n\n" +
			$"\t#region << details >>\n" +
			$"\tcreateListInput.Id = new Guid(\"{list.Id}\");\n" +
			$"\tcreateListInput.Type =  \"{list.Type}\";\n" +
			$"\tcreateListInput.Name = \"{list.Name}\";\n" +
			$"\tcreateListInput.Label = \"{list.Label}\";\n";
			if (list.Title == null)
			{
				response += $"\tcreateListInput.Title = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.Title = \"{list.Title}\";\n";
			}

			if (list.Weight == null)
			{
				response += $"\tcreateListInput.Weight = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.Weight = Decimal.Parse(\"{list.Weight}\");\n";
			}
			response +=
			$"\tcreateListInput.Default = {(list.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.System = {(list.System).ToString().ToLowerInvariant()};\n";

			if (list.CssClass == null)
			{
				response += $"\tcreateListInput.CssClass = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.CssClass = \"{list.CssClass}\";\n";
			}

			if (list.IconName == null)
			{
				response += $"\tcreateListInput.IconName = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.IconName = string.IsNullOrEmpty(\"{list.IconName}\") ? string.Empty : \"{list.IconName}\";\n";
			}

			response += $"\tcreateListInput.VisibleColumnsCount = Int32.Parse(\"{list.VisibleColumnsCount}\");\n";

			if (list.ColumnWidthsCSV == null)
			{
				response += $"\tcreateListInput.ColumnWidthsCSV = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.ColumnWidthsCSV = \"{list.ColumnWidthsCSV}\";\n";
			}

			response += $"\tcreateListInput.PageSize = Int32.Parse(\"{list.PageSize}\");\n";
			if (list.DynamicHtmlTemplate == null)
			{
				response += $"\tcreateListInput.DynamicHtmlTemplate = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.DynamicHtmlTemplate = \"{list.DynamicHtmlTemplate}\";\n";
			}
			if (list.DataSourceUrl == null)
			{
				response += $"\tcreateListInput.DataSourceUrl = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.DataSourceUrl = \"{list.DataSourceUrl}\";\n";
			}
			if (list.ServiceCode == null)
			{
				response += $"\tcreateListInput.ServiceCode = null;\n";
			}
			else
			{
				response += $"\tcreateListInput.ServiceCode = @\"{list.ServiceCode.Replace("\"","\"\"")}\";\n";
			}

			response +=
			$"\t#endregion\n\n";

			//Relation options
			response +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateListInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in list.RelationOptions)
			{
				response += CreateRelationOptionCode(relationOption, entityId, entityName,"list");
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			response +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateListInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in list.ActionItems)
			{
				response += CreateListActionItemCode(actionItem, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Columns
			response +=
			$"\t#region << Columns >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Columns = new List<InputRecordListItemBase>();\n\n";
			foreach (var column in list.Columns)
			{
				response += CreateListColumnCode(column, entityId, entityName, list.Name);
			}

			response += "\t}\n" +
			$"\t#endregion\n\n";



			//Query
			response +=
			$"\t#region << Query >>\n" +
			"\t{\n";
			if (list.Query == null)
			{
				response += $"\tcreateListInput.Query = null;\n";
			}
			else
			{
				response +=
				$"\tcreateListInput.Query = new InputRecordListQuery();\n" +
				$"\tvar queryDictionary = new Dictionary<Guid,InputRecordListQuery>();\n" +
				$"\tvar subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();\n" +
				$"\t//Main query rule\n";
				if (list.Query.FieldName == null)
				{
					response += $"\tcreateListInput.Query.FieldName = null;\n";
				}
				else
				{
					response += $"\tcreateListInput.Query.FieldName = \"{list.Query.FieldName}\";\n";
				}
				if (list.Query.FieldValue == null)
				{
					response += $"\tcreateListInput.Query.FieldValue =  null;\n";
				}
				else
				{
					response += $"\tcreateListInput.Query.FieldValue =  \"{list.Query.FieldValue}\";\n";
				}
				response += $"\tcreateListInput.Query.QueryType = \"{list.Query.QueryType}\";\n";
				response +=
				$"\tcreateListInput.Query.SubQueries = new List<InputRecordListQuery>();\n";
				var nodeId = Guid.NewGuid();
				foreach (var query in list.Query.SubQueries)
				{
					response += CreateListQueryCode(query, entityId, entityName, nodeId, 1);
				}
				response +=
				$"\tif(subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{createListInput.Query.SubQueries = subQueryDictionary[new Guid(\"{nodeId}\")];}}\n";
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Sort
			response +=
			$"\t#region << Sorts >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Sorts = new List<InputRecordListSort>();\n\n";
			foreach (var sort in list.Sorts)
			{
				response += CreateListSortCode(sort, entityId, entityName);
			}

			response += "\t}\n" +
			$"\t#endregion\n\n";

			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordList(new Guid(\"{entityId}\"), createListInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Created list: {list.Name} Message:\" + response.Message);\n" +
			"\t}\n";

			response +=
			"}\n" +
			"#endregion\n\n";
			return response;
		}

		//Action Item
		private string CreateListActionItemCode(ActionItem actionItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			string lineSeparator = ((char)0x2028).ToString();
			string paragraphSeparator = ((char)0x2029).ToString();
			response +=
			$"\t\t#region << action item: {actionItem.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar actionItem = new ActionItem();\n" +
			$"\t\t\tactionItem.Name = \"{actionItem.Name}\";\n" +
			$"\t\t\tactionItem.Menu = \"{actionItem.Menu}\";\n";
			if (actionItem.Weight == null)
			{
				response += $"\t\t\tactionItem.Weight = null;\n";
			}
			else
			{
				response += $"\t\t\tactionItem.Weight = Decimal.Parse(\"{actionItem.Weight}\");\n";
			}

			response +=
			$"\t\t\tactionItem.Template = @\"{actionItem.Template.Replace("\"", "\"\"").Replace(System.Environment.NewLine, "\n")}\";\n" +
			$"\t\t\tcreateListInput.ActionItems.Add(actionItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Column Item
		private string CreateListColumnCode(DbRecordListItemBase item, Guid entityId, string entityName, string ListName)
		{
			var response = string.Empty;
			if (item is DbRecordListFieldItem)
			{
				response += CreateRecordListFieldItemCode(item as DbRecordListFieldItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationFieldItem)
			{
				response += CreateRecordListRelationFieldItemCode(item as DbRecordListRelationFieldItem, entityId, entityName);
			}
			else if (item is DbRecordListViewItem)
			{
				response += CreateRecordListViewItemCode(item as DbRecordListViewItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationViewItem)
			{
				response += CreateRecordListRelationViewItemCode(item as DbRecordListRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordListListItem)
			{
				response += CreateRecordListListItemCode(item as DbRecordListListItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationListItem)
			{
				response += CreateRecordListRelationListItemCode(item as DbRecordListRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationTreeItem)
			{
				response += CreateRecordListRelationTreeItemCode(item as DbRecordListRelationTreeItem, entityId, entityName);
			}

			return response;
		}

		//field
		private string CreateRecordListFieldItemCode(DbRecordListFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			response +=
			$"\t\t#region << {currentField.Name} >>\n" +
			"\t\t{\n" +
				"\t\t\tvar listField = new InputRecordListFieldItem();\n" +
				$"\t\t\tlistField.EntityId = new Guid(\"{entityId}\");\n" +
				$"\t\t\tlistField.EntityName = \"{entityName}\";\n" +
				$"\t\t\tlistField.FieldId = new Guid(\"{fieldItem.FieldId}\");\n" +
				$"\t\t\tlistField.FieldName = \"{currentField.Name}\";\n" +
				$"\t\t\tlistField.Type = \"field\";\n" +
				$"\t\t\tcreateListInput.Columns.Add(listField);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//field from relation
		private string CreateRecordListRelationFieldItemCode(DbRecordListRelationFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(fieldItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(fieldItem.EntityId).Object;
			var relatedField = relatedEntity.Fields.SingleOrDefault(x => x.Id == fieldItem.FieldId);
			if (relatedField == null)
			{
				response += "/////////////////////////////////////////////////////////////////////////////\n";
				response += "//WARNING: Field from relation was not found: fieldId: " + fieldItem.FieldId + " in entity: " + relatedEntity.Name + "\n";
				response += "/////////////////////////////////////////////////////////////////////////////\n";
				return response;
			}
			else
			{
				response +=
				$"\t\t#region << field from Relation: {relatedField.Name} >>\n" +
				"\t\t{\n" +
					"\t\t\tvar listItemFromRelation = new InputRecordListRelationFieldItem();\n" +
					$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{fieldItem.EntityId}\");\n" +
					$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
					$"\t\t\tlistItemFromRelation.Type = \"fieldFromRelation\";\n" +
					$"\t\t\tlistItemFromRelation.FieldId = new Guid(\"{relatedField.Id}\");\n" +
					$"\t\t\tlistItemFromRelation.FieldName = \"{relatedField.Name}\";\n";

				if (fieldItem.FieldLabel == null)
				{
					response += $"\t\t\tlistItemFromRelation.FieldLabel = null;\n";
				}
				else
				{
					response += $"\t\t\tlistItemFromRelation.FieldLabel = \"{fieldItem.FieldLabel}\";\n";
				}

				if (fieldItem.FieldPlaceholder == null)
				{
					response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = null;\n";
				}
				else
				{
					response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{fieldItem.FieldPlaceholder}\";\n";
				}

				if (fieldItem.FieldHelpText == null)
				{
					response += $"\t\t\tlistItemFromRelation.FieldHelpText = null;\n";
				}
				else
				{
					response += $"\t\t\tlistItemFromRelation.FieldHelpText = \"{fieldItem.FieldHelpText}\";\n";
				}


				response +=
				$"\t\t\tlistItemFromRelation.FieldRequired = {(fieldItem.FieldRequired).ToString().ToLowerInvariant()};\n";
				if (fieldItem.FieldLookupList == null)
				{
					response += $"\t\t\tlistItemFromRelation.FieldLookupList = null;\n";
				}
				else
				{
					response += $"\t\t\tlistItemFromRelation.FieldLookupList = \"{fieldItem.FieldLookupList}\";\n";
				}
				response +=
				$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{fieldItem.RelationId}\");\n" +
				$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
				$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			}
			return response;
		}

		//view
		private string CreateRecordListViewItemCode(DbRecordListViewItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordListItem.ViewId);
			response +=
			$"\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItem = new InputRecordListViewItem();\n" +
			$"\t\t\tlistItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tlistItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tlistItem.ViewId = new Guid(\"{recordListItem.ViewId}\");\n" +
			$"\t\t\tlistItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\tlistItem.Type = \"view\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//view from relation
		private string CreateRecordListRelationViewItemCode(DbRecordListRelationViewItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordListItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordListItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordListItem.ViewId);
			response +=
			$"\t\t#region << View from relation: {relatedView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationViewItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{recordListItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.ViewId = new Guid(\"{recordListItem.ViewId}\");\n" +
			$"\t\t\tlistItemFromRelation.ViewName = \"{relatedView.Name}\";\n";

			if (recordListItem.FieldLabel == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = \"{recordListItem.FieldLabel}\";\n";
			}

			if (recordListItem.FieldPlaceholder == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{recordListItem.FieldPlaceholder}\";\n";
			}

			if (recordListItem.FieldHelpText == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = \"{recordListItem.FieldHelpText}\";\n";
			}

			response +=
			$"\t\t\tlistItemFromRelation.FieldRequired = {(recordListItem.FieldRequired).ToString().ToLowerInvariant()};\n";
			if (recordListItem.FieldLookupList == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldLookupList = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldLookupList = \"{recordListItem.FieldLookupList}\";\n";
			}

			if (recordListItem.FieldManageView == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldManageView = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldManageView = \"{recordListItem.FieldManageView}\";\n";
			}


			response +=
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{recordListItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//list
		private string CreateRecordListListItemCode(DbRecordListListItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == recordListItem.ListId);
			response +=
			$"\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItem = new InputRecordListListItem();\n" +
			$"\t\t\tlistItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tlistItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tlistItem.ListId = new Guid(\"{recordListItem.ListId}\");\n" +
			$"\t\t\tlistItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\tlistItem.Type = \"list\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";

			return response;
		}

		//list from relation
		private string CreateRecordListRelationListItemCode(DbRecordListRelationListItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordListItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordListItem.EntityId).Object;
			var relatedList = relatedEntity.RecordViews.Single(x => x.Id == recordListItem.ListId);
			response +=
			$"\t\t#region << List from relation: {relatedList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationListItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{recordListItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.ListId = new Guid(\"{recordListItem.ListId}\");\n" +
			$"\t\t\tlistItemFromRelation.ListName = \"{relatedList.Name}\";\n";
			if (recordListItem.FieldLabel == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = \"{recordListItem.FieldLabel}\";\n";
			}
			if (recordListItem.FieldPlaceholder == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{recordListItem.FieldPlaceholder}\";\n";
			}

			if (recordListItem.FieldHelpText == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = \"{recordListItem.FieldHelpText}\";\n";
			}

			response +=
			$"\t\t\tlistItemFromRelation.FieldRequired = {(recordListItem.FieldRequired).ToString().ToLowerInvariant()};\n";
			if (recordListItem.FieldLookupList == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldLookupList = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldLookupList = \"{recordListItem.FieldLookupList}\";\n";
			}
			if (recordListItem.FieldManageView == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldManageView = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldManageView = \"{recordListItem.FieldManageView}\";\n";
			}
			response +=
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{recordListItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//tree from relation
		private string CreateRecordListRelationTreeItemCode(DbRecordListRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordTrees.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t#region << Tree from relation: {relatedTree.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationTreeItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{treeItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\tlistItemFromRelation.TreeName = \"{relatedTree.Name}\";\n";

			if (treeItem.FieldLabel == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n";
			}

			if (treeItem.FieldPlaceholder == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n";
			}

			if (treeItem.FieldHelpText == null)
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = null;\n";
			}
			else
			{
				response += $"\t\t\tlistItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n";
			}

			response +=
			$"\t\t\tlistItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		private string CreateListQueryCode(DbRecordListQuery query, Guid entityId, string entityName, Guid nodeId, int subQueryLevel)
		{
			var response = string.Empty;
			var levelTabs = "";
			for (int i = 0; i < subQueryLevel + 1; i++)
			{
				levelTabs += "\t";
			}
			var encodedFieldValue = query.FieldValue;

			if (query.FieldValue != null && query.FieldValue.StartsWith("{"))
			{
				encodedFieldValue = query.FieldValue.Replace("\"", "\\\"");
			}

			var newNodeId = Guid.NewGuid();
			response +=
			levelTabs + "{\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")] = new InputRecordListQuery();\n";

			if (query.FieldName == null)
			{
				response += levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldName = null;\n";
			}
			else
			{
				response += levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldName = \"{query.FieldName}\";\n";
			}
			if (encodedFieldValue == null)
			{
				response += levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldValue =  null;\n";
			}
			else
			{
				response += levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldValue =  \"{encodedFieldValue}\";\n";
			}
			response += levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].QueryType = \"{query.QueryType}\";\n";
			response +=
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].SubQueries = new List<InputRecordListQuery>();\n";
			foreach (var subQuery in query.SubQueries)
			{
				response += CreateListQueryCode(subQuery, entityId, entityName, newNodeId, subQueryLevel + 1);
			}
			response +=
			levelTabs + $"if(subQueryDictionary.ContainsKey(new Guid(\"{newNodeId}\"))) {{queryDictionary[new Guid(\"{newNodeId}\")].SubQueries = subQueryDictionary[new Guid(\"{newNodeId}\")];}}\n" +
			levelTabs + $"if(!subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{subQueryDictionary[new Guid(\"{nodeId}\")] = new List<InputRecordListQuery>();}}\n" +
			levelTabs + $"subQueryDictionary[new Guid(\"{nodeId}\")].Add(queryDictionary[new Guid(\"{newNodeId}\")]);\n" +
			levelTabs + "}\n";
			return response;
		}

		//Sort
		private string CreateListSortCode(DbRecordListSort sort, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var encodedFieldName = sort.FieldName;
			if (sort.FieldName.StartsWith("{"))
			{
				encodedFieldName = sort.FieldName.Replace("\"", "\\\"");
			}

			response +=
			$"\t\t#region << sort >>\n" +
			"\t\t{\n" +
			"\t\t\tvar sort = new InputRecordListSort();\n" +
			$"\t\t\tsort.FieldName = \"{encodedFieldName}\";\n" +
			$"\t\t\tsort.SortType = \"{sort.SortType}\";\n" +
			$"\t\t\tcreateListInput.Sorts.Add(sort);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Delete
		private string DeleteListCode(DbRecordList list, Guid entityId, string entityName)
		{
			var response =
		$"#region << ***Delete list***  Entity: {entityName} List Name: {list.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordList(new Guid(\"{entityId}\"),new Guid(\"{list.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. List: {list.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateListCode(DbRecordList currentList, DbRecordList oldList, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentList.Label != null)
				currentList.Label = currentList.Label.Replace("\"", "\\\"");

			//escape the old too
			if (oldList.Label != null)
				oldList.Label = oldList.Label.Replace("\"", "\\\"");

			#region << Code >>
			code +=
			$"#region << ***Update list***  Entity: {currentEntity.Name} List Name: {currentList.Name} >>\n" +
			"{\n" +
			$"\tvar createListEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			$"\tvar createListInput = new InputRecordList();\n\n" +
			$"\t#region << details >>\n" +
			$"\tcreateListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == \"{currentList.Name}\").Id;\n" +
			$"\tcreateListInput.Type =  \"{currentList.Type}\";\n" +
			$"\tcreateListInput.Name = \"{currentList.Name}\";\n" +
			$"\tcreateListInput.Label = \"{currentList.Label}\";\n";
			if (currentList.Title == null)
			{
				code += $"\tcreateListInput.Title = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.Title = \"{currentList.Title}\";\n";
			}


			if (currentList.Weight == null)
			{
				code += $"\tcreateListInput.Weight = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.Weight = Decimal.Parse(\"{currentList.Weight}\");\n";
			}

			code +=
			$"\tcreateListInput.Default = {(currentList.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.System = {(currentList.System).ToString().ToLowerInvariant()};\n";

			if (currentList.CssClass == null)
			{
				code += $"\tcreateListInput.CssClass = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.CssClass = \"{currentList.CssClass}\";\n";
			}
			if (currentList.IconName == null)
			{
				code += $"\tcreateListInput.IconName = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.IconName = \"{currentList.IconName}\";\n";
			}
			code += $"\tcreateListInput.VisibleColumnsCount = Int32.Parse(\"{currentList.VisibleColumnsCount}\");\n";
			if (currentList.ColumnWidthsCSV == null)
			{
				code += $"\tcreateListInput.ColumnWidthsCSV = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.ColumnWidthsCSV = \"{currentList.ColumnWidthsCSV}\";\n";
			}
			code += $"\tcreateListInput.PageSize = Int32.Parse(\"{currentList.PageSize}\");\n";
			if (currentList.DynamicHtmlTemplate == null)
			{
				code += $"\tcreateListInput.DynamicHtmlTemplate = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.DynamicHtmlTemplate = \"{currentList.DynamicHtmlTemplate}\";\n";
			}
			if (currentList.DataSourceUrl == null)
			{
				code += $"\tcreateListInput.DataSourceUrl = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.DataSourceUrl = \"{currentList.DataSourceUrl}\";\n";
			}

			if (currentList.ServiceCode == null)
			{
				code += $"\tcreateListInput.ServiceCode = null;\n";
			}
			else
			{
				code += $"\tcreateListInput.ServiceCode = @\"{currentList.ServiceCode.Replace("\"","\"\"")}\";\n";
			}

			code +=
			$"\t#endregion\n\n";


			//Relation options
			code +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateListInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in currentList.RelationOptions)
			{
				code += CreateRelationOptionCode(relationOption, currentEntity.Id, currentEntity.Name,"list");
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			code +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateListInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in currentList.ActionItems)
			{
				code += CreateListActionItemCode(actionItem, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Columns
			code +=
			$"\t#region << Columns >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Columns = new List<InputRecordListItemBase>();\n\n";
			foreach (var column in currentList.Columns)
			{
				code += CreateListColumnCode(column, currentEntity.Id, currentEntity.Name, currentList.Name);
			}

			code += "\t}\n" +
			$"\t#endregion\n\n";



			//Query
			code +=
			$"\t#region << Query >>\n" +
			"\t{\n";
			if (currentList.Query == null)
			{
				code += $"\tcreateListInput.Query = null;\n";
			}
			else
			{
				code +=
				$"\tcreateListInput.Query = new InputRecordListQuery();\n" +
				$"\tvar queryDictionary = new Dictionary<Guid,InputRecordListQuery>();\n" +
				$"\tvar subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();\n" +
				$"\t//Main query rule\n";
				if (currentList.Query.FieldName == null)
				{
					code += $"\tcreateListInput.Query.FieldName = null;\n";
				}
				else
				{
					code += $"\tcreateListInput.Query.FieldName = \"{currentList.Query.FieldName}\";\n";
				}
				if (currentList.Query.FieldValue == null)
				{
					code += $"\tcreateListInput.Query.FieldValue =  null;\n";
				}
				else
				{
					code += $"\tcreateListInput.Query.FieldValue =  \"{currentList.Query.FieldValue}\";\n";
				}
				code += $"\tcreateListInput.Query.QueryType = \"{currentList.Query.QueryType}\";\n";
				code +=
				$"\tcreateListInput.Query.SubQueries = new List<InputRecordListQuery>();\n";
				var nodeId = Guid.NewGuid();
				foreach (var query in currentList.Query.SubQueries)
				{
					code += CreateListQueryCode(query, currentEntity.Id, currentEntity.Name, nodeId, 1);
				}
				code +=
				$"\tif(subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{createListInput.Query.SubQueries = subQueryDictionary[new Guid(\"{nodeId}\")];}}\n";
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Sort
			code +=
			$"\t#region << Sorts >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Sorts = new List<InputRecordListSort>();\n\n";
			foreach (var sort in currentList.Sorts)
			{
				code += CreateListSortCode(sort, currentEntity.Id, currentEntity.Name);
			}

			code += "\t}\n" +
			$"\t#endregion\n\n";

			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordList(new Guid(\"{currentEntity.Id}\"), createListInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated list: {currentList.Name} Message:\" + response.Message);\n" +
			"\t}\n";

			code +=
			"}\n" +
			"#endregion\n\n";
			#endregion

			#region << Check change >>
			if (currentList.Type != oldList.Type)
			{
				hasUpdate = true;
			}
			if (currentList.Name != oldList.Name)
			{
				hasUpdate = true;
			}
			if (currentList.Label != oldList.Label)
			{
				hasUpdate = true;
			}
			if (currentList.Title != oldList.Title)
			{
				hasUpdate = true;
			}
			if (currentList.Weight != oldList.Weight)
			{
				hasUpdate = true;
			}
			if (currentList.Default != oldList.Default)
			{
				hasUpdate = true;
			}
			if (currentList.System != oldList.System)
			{
				hasUpdate = true;
			}
			if (currentList.CssClass != oldList.CssClass)
			{
				hasUpdate = true;
			}
			if (currentList.IconName != oldList.IconName)
			{
				hasUpdate = true;
			}
			if (currentList.VisibleColumnsCount != oldList.VisibleColumnsCount)
			{
				hasUpdate = true;
			}
			if (currentList.ColumnWidthsCSV != oldList.ColumnWidthsCSV)
			{
				hasUpdate = true;
			}
			if (currentList.PageSize != oldList.PageSize)
			{
				hasUpdate = true;
			}
			if (currentList.DynamicHtmlTemplate != oldList.DynamicHtmlTemplate)
			{
				hasUpdate = true;
			}
			if (currentList.DataSourceUrl != oldList.DataSourceUrl)
			{
				hasUpdate = true;
			}
			//Because of the \r\n <> \n problem we will need to artificially add \r to the new view templates and test again
			if(!String.IsNullOrWhiteSpace(oldList.ServiceCode)) {
				//Remove all \r and \n and test the lists as there is often a problem with that
				var fixedServiceCode = oldList.ServiceCode.Replace("\n", "").Replace("\r", "");
				var fixedCurrrentServiceCode = currentList.ServiceCode.Replace("\n", "").Replace("\r", "");
				if (fixedCurrrentServiceCode != fixedServiceCode)
				{
					hasUpdate = true;
				}
			}else {
				if (currentList.ServiceCode != oldList.ServiceCode)
				{
					hasUpdate = true;
				}			
			}

			if (JsonConvert.SerializeObject(currentList.RelationOptions) != JsonConvert.SerializeObject(oldList.RelationOptions))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.ActionItems) != JsonConvert.SerializeObject(oldList.ActionItems))
			{
				//Because of the \r\n <> \n problem we will need to artificially add \r to the new view templates and test again
				var differenceInTemplateFound = false;
				//create an action item dictionary for the oldView
				var oldActionsDictionary = new Dictionary<string, ActionItem>();
				foreach (var item in oldList.ActionItems)
				{
					var dictionaryKey = item.Name + "-" + item.Menu;
					oldActionsDictionary[dictionaryKey] = item;
				}

				foreach (var item in currentList.ActionItems)
				{
					//check if item with the same name-menu exists in the old menu
					var dictionaryKey = item.Name + "-" + item.Menu;
					if (!oldActionsDictionary.ContainsKey(dictionaryKey))
					{
						differenceInTemplateFound = true;
						break;
					}
					else
					{
						var correspondingActionItem = oldActionsDictionary[dictionaryKey];
						var oldTemplateFixed = correspondingActionItem.Template.Replace("\n", "").Replace("\r", "");
						var newTemplateFixed = item.Template.Replace("\n", "").Replace("\r", "");
						if (oldTemplateFixed != newTemplateFixed)
						{
							differenceInTemplateFound = true;
						}
					}
				}
				if (differenceInTemplateFound)
				{
					hasUpdate = true;
				}
			}
			if (JsonConvert.SerializeObject(currentList.Columns) != JsonConvert.SerializeObject(oldList.Columns))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.Query) != JsonConvert.SerializeObject(oldList.Query))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.Sorts) != JsonConvert.SerializeObject(oldList.Sorts))
			{
				hasUpdate = true;
			}
			#endregion


			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>list</span>  with name <span class='go-red'>{oldList.Name}</span> was updated");
			return response;
		}

		#endregion

		#region << Tree >>
		private string CreateTreeCode(DbRecordTree tree, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (tree.Label != null)
				tree.Label = tree.Label.Replace("\"", "\\\"");

			response +=
		   $"#region << Tree  Entity: {entityName} name: {tree.Name} >>\n" +
		   "{\n" +
			"\tvar createTreeInput = new InputRecordTree();\n" +
			$"\tcreateTreeInput.Id = new Guid(\"{tree.Id}\");\n" +
			$"\tcreateTreeInput.Name = \"{tree.Name}\";\n" +
			$"\tcreateTreeInput.Label = \"{tree.Label}\";\n" +
			$"\tcreateTreeInput.Default = {(tree.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateTreeInput.System = {(tree.System).ToString().ToLowerInvariant()};\n";
			if (tree.CssClass == null)
			{
				response += $"\tcreateTreeInput.CssClass = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.CssClass = \"{tree.CssClass}\";\n";
			}
			if (tree.IconName == null)
			{
				response += $"\tcreateTreeInput.IconName = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.IconName = \"{tree.IconName}\";\n";
			}

			response +=
			$"\tcreateTreeInput.RelationId = new Guid(\"{tree.RelationId}\");\n";
			response += $"\tcreateTreeInput.DepthLimit = Int32.Parse(\"{tree.DepthLimit}\");\n";
			if (tree.NodeParentIdFieldId == null)
			{
				response += $"\tcreateTreeInput.NodeIdFieldId = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.NodeIdFieldId = Guid.Parse(\"{tree.NodeIdFieldId}\");\n";
			}
			if (tree.NodeIdFieldId == null)
			{
				response += $"\tcreateTreeInput.NodeParentIdFieldId = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.NodeParentIdFieldId = Guid.Parse(\"{tree.NodeParentIdFieldId}\");\n";
			}
			if (tree.NodeNameFieldId == null)
			{
				response += $"\tcreateTreeInput.NodeNameFieldId = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.NodeNameFieldId = Guid.Parse(\"{tree.NodeNameFieldId}\");\n";
			}
			if (tree.NodeLabelFieldId == null)
			{
				response += $"\tcreateTreeInput.NodeLabelFieldId = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.NodeLabelFieldId = Guid.Parse(\"{tree.NodeLabelFieldId}\");\n";
			}
			if (tree.NodeWeightFieldId == null)
			{
				response += $"\tcreateTreeInput.NodeWeightFieldId = null;\n";
			}
			else
			{
				response += $"\tcreateTreeInput.NodeWeightFieldId = Guid.Parse(\"{tree.NodeWeightFieldId}\");\n";
			}

			response +=
			$"\tcreateTreeInput.RootNodes = new List<RecordTreeNode>();\n";
			foreach (var recordId in tree.RootNodes)
			{
				response += CreateTreeRootNodeCode(recordId, tree, entityId, entityName);
			}
			response += $"\tcreateTreeInput.NodeObjectProperties = new List<Guid>();\n";
			foreach (var propertyId in tree.NodeObjectProperties)
			{
				response += $"\tcreateTreeInput.NodeObjectProperties.Add(new Guid(\"{propertyId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordTree(new Guid(\"{entityId}\"), createTreeInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Updated List: list_name Message:\" + response.Message);\n" +
			"\t}\n" +
			"}\n" +
			"#endregion\n\n";
			return response;
		}

		//RootNode
		private string CreateTreeRootNodeCode(Guid recordId, DbRecordTree tree, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			EntityRecord record = null;
			EntityQuery query = new EntityQuery("entityName", "*", EntityQuery.QueryEQ("id", recordId), null, null, null);
			var result = recMan.Find(query);
			if (result.Success && result.Object.Data.Any())
			{
				record = result.Object.Data[0];
			}

			if (record == null)
			{
				return $"\t\t//Cannot find record for root node recordId {recordId} in entity: {entityName}. Skiping the addition of this root node.\n";
			}

			var nodeId = record[currentEntity.Fields.Single(x => x.Id == tree.NodeIdFieldId).Name];
			var nodeName = record[currentEntity.Fields.Single(x => x.Id == tree.NodeNameFieldId).Name];
			var nodeLabel = record[currentEntity.Fields.Single(x => x.Id == tree.NodeLabelFieldId).Name]; ;
			var nodeParentId = record[currentEntity.Fields.Single(x => x.Id == tree.NodeParentIdFieldId).Name];
			var nodeWeight = record[currentEntity.Fields.Single(x => x.Id == tree.NodeWeightFieldId).Name];
			var nodeRecordId = recordId;

			response +=
			$"\t\t#region << root node: {nodeName} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar treeNode = new RecordTreeNode();\n" +
			$"\t\t\ttreeNode.Id = \"{nodeId}\";\n" +
			$"\t\t\ttreeNode.Name = \"{nodeName}\";\n" +
			$"\t\t\ttreeNode.Label = \"{nodeLabel}\";\n" +
			$"\t\t\ttreeNode.Weight = \"{nodeWeight}\";\n" +
			$"\t\t\ttreeNode.ParentId =  new Guid(\"{nodeParentId}\");\n" +
			$"\t\t\ttreeNode.RecordId =  new Guid(\"{nodeRecordId}\");\n" +
			$"\t\t\tcreateTreeInput.RootNodes.Add(treeNode);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Delete
		private string DeleteTreeCode(DbRecordTree tree, Guid entityId, string entityName)
		{
			var response =
		$"#region << ***Delete tree***  Entity: {entityName} Tree name: {tree.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordTree(new Guid(\"{entityId}\"),new Guid(\"{tree.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Tree: {tree.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		//Update
		private UpdateCheckResponse UpdateTreeCode(DbRecordTree currentTree, DbRecordTree oldTree, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentTree.Label != null)
				currentTree.Label = currentTree.Label.Replace("\"", "\\\"");
			//escape the old for update check sake
			if (oldTree.Label != null)
				oldTree.Label = oldTree.Label.Replace("\"", "\\\"");
			#region << Code >>
			code +=
		   $"#region << Tree  Entity: {currentEntity.Name} name: {currentTree.Name} >>\n" +
		   "{\n" +
			$"\tvar createTreeEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			"\tvar updateTreeInput = new InputRecordTree();\n" +
			$"\tupdateTreeInput.Id = createTreeEntity.RecordTrees.SingleOrDefault(x => x.Name == \"{currentTree.Name}\").Id;\n" +
			$"\tupdateTreeInput.Name = \"{currentTree.Name}\";\n" +
			$"\tupdateTreeInput.Label = \"{currentTree.Label}\";\n" +
			$"\tupdateTreeInput.Default = {(currentTree.Default).ToString().ToLowerInvariant()};\n" +
			$"\tupdateTreeInput.System = {(currentTree.System).ToString().ToLowerInvariant()};\n";
			if (currentTree.CssClass == null)
			{
				code += $"\tupdateTreeInput.CssClass = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.CssClass = \"{currentTree.CssClass}\";\n";
			}

			if (currentTree.IconName == null)
			{
				code += $"\tupdateTreeInput.IconName = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.IconName = \"{currentTree.IconName}\";\n";
			}

			code +=
			$"\tupdateTreeInput.RelationId = new Guid(\"{currentTree.RelationId}\");\n";

			code += $"\tupdateTreeInput.DepthLimit = Int32.Parse(\"{currentTree.DepthLimit}\");\n";
			if (currentTree.NodeParentIdFieldId == null)
			{
				code += $"\tupdateTreeInput.NodeIdFieldId = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.NodeIdFieldId = Guid.Parse(\"{currentTree.NodeIdFieldId}\");\n";
			}
			if (currentTree.NodeIdFieldId == null)
			{
				code += $"\tupdateTreeInput.NodeParentIdFieldId = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.NodeParentIdFieldId = Guid.Parse(\"{currentTree.NodeParentIdFieldId}\");\n";
			}
			if (currentTree.NodeNameFieldId == null)
			{
				code += $"\tupdateTreeInput.NodeNameFieldId = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.NodeNameFieldId = Guid.Parse(\"{currentTree.NodeNameFieldId}\");\n";
			}
			if (currentTree.NodeLabelFieldId == null)
			{
				code += $"\tupdateTreeInput.NodeLabelFieldId = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.NodeLabelFieldId = Guid.Parse(\"{currentTree.NodeLabelFieldId}\");\n";
			}
			if (currentTree.NodeWeightFieldId == null)
			{
				code += $"\tupdateTreeInput.NodeWeightFieldId = null;\n";
			}
			else
			{
				code += $"\tupdateTreeInput.NodeWeightFieldId = Guid.Parse(\"{currentTree.NodeWeightFieldId}\");\n";
			}


			code +=
			$"\tupdateTreeInput.RootNodes = new List<RecordTreeNode>();\n";
			foreach (var recordId in currentTree.RootNodes)
			{
				code += CreateTreeRootNodeCode(recordId, currentTree, currentEntity.Id, currentEntity.Name);
			}
			code += $"\tupdateTreeInput.NodeObjectProperties = new List<Guid>();\n";
			foreach (var propertyId in currentTree.NodeObjectProperties)
			{
				code += $"\tupdateTreeInput.NodeObjectProperties.Add(new Guid(\"{propertyId}\"));\n";
			}
			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordTree(new Guid(\"{currentEntity.Id}\"), updateTreeInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated Tree: {oldTree.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
			"}\n" +
			"#endregion\n\n";

			#endregion


			#region << Changes >>
			if (currentTree.Name != oldTree.Name)
			{
				hasUpdate = true;
			}
			if (currentTree.Label != oldTree.Label)
			{
				hasUpdate = true;
			}
			if (currentTree.Default != oldTree.Default)
			{
				hasUpdate = true;
			}
			if (currentTree.System != oldTree.System)
			{
				hasUpdate = true;
			}
			if (currentTree.CssClass != oldTree.CssClass)
			{
				hasUpdate = true;
			}
			if (currentTree.IconName != oldTree.IconName)
			{
				hasUpdate = true;
			}
			if (currentTree.RelationId != oldTree.RelationId)
			{
				hasUpdate = true;
			}
			if (currentTree.DepthLimit != oldTree.DepthLimit)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeIdFieldId != oldTree.NodeIdFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeParentIdFieldId != oldTree.NodeParentIdFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeNameFieldId != oldTree.NodeNameFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeLabelFieldId != oldTree.NodeLabelFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeWeightFieldId != oldTree.NodeWeightFieldId)
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentTree.RootNodes) != JsonConvert.SerializeObject(oldTree.RootNodes))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentTree.NodeObjectProperties) != JsonConvert.SerializeObject(oldTree.NodeObjectProperties))
			{
				hasUpdate = true;
			}
			#endregion

			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  with name <span class='go-red'>{oldTree.Name}</span> was updated");
			return response;
		}
		#endregion

		#region << Relations >>
		private string CreateRelationCode(DbEntityRelation relationRecord)
		{
			var originEntity = entMan.ReadEntity(relationRecord.OriginEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == relationRecord.OriginFieldId);
			var targetEntity = entMan.ReadEntity(relationRecord.TargetEntityId).Object;
			var targetField = targetEntity.Fields.Single(x => x.Id == relationRecord.TargetFieldId);

			//escape possible double quotes
			if (relationRecord.Label != null)
				relationRecord.Label = relationRecord.Label.Replace("\"", "\\\"");
			if (relationRecord.Description != null)
				relationRecord.Description = relationRecord.Description.Replace("\"", "\\\"");

			var response =
			$"#region << ***Create relation*** Relation name: {relationRecord.Name} >>\n" +
			"{\n" +
				"\tvar relation = new EntityRelation();\n" +
				$"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
				$"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"{originField.Name}\");\n" +
				$"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
				$"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"{targetField.Name}\");\n" +
				$"\trelation.Id = new Guid(\"{relationRecord.Id}\");\n" +
				$"\trelation.Name =  \"{relationRecord.Name}\";\n" +
				$"\trelation.Label = \"{relationRecord.Label}\";\n";
				if(relationRecord.Description == null) {
					response += $"\trelation.Description = null;\n";
				}
				else {
					response += $"\trelation.Description = \"{relationRecord.Description}\";\n";
				}

				response +=
				$"\trelation.System =  {(relationRecord.System).ToString().ToLowerInvariant()};\n";
			if (relationRecord.RelationType == EntityRelationType.OneToOne)
			{
				response += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
			}
			else if (relationRecord.RelationType == EntityRelationType.OneToMany)
			{
				response += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
			}
			else if (relationRecord.RelationType == EntityRelationType.ManyToMany)
			{
				response += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
			}
			response +=
			$"\trelation.OriginEntityId = originEntity.Id;\n" +
			$"\trelation.OriginEntityName = originEntity.Name;\n" +
			$"\trelation.OriginFieldId = originField.Id;\n" +
			$"\trelation.OriginFieldName = originField.Name;\n" +
			$"\trelation.TargetEntityId = targetEntity.Id;\n" +
			$"\trelation.TargetEntityName = targetEntity.Name;\n" +
			$"\trelation.TargetFieldId = targetField.Id;\n" +
			$"\trelation.TargetFieldName = targetField.Name;\n" +
			"\t{\n" +
				$"\t\tvar response = relMan.Create(relation);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Create. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string DeleteRelationCode(DbEntityRelation relationRecord)
		{
			var response =
		$"#region << ***Delete relation*** Relation name: {relationRecord.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = relMan.Delete(new Guid(\"{relationRecord.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateRelationCode(DbEntityRelation currentRelation, DbEntityRelation oldRelation)
		{
			var response = new UpdateCheckResponse();
			var originEntity = entMan.ReadEntity(currentRelation.OriginEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == currentRelation.OriginFieldId);
			var targetEntity = entMan.ReadEntity(currentRelation.TargetEntityId).Object;
			var targetField = targetEntity.Fields.Single(x => x.Id == currentRelation.TargetFieldId);
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentRelation.Label != null)
				currentRelation.Label = currentRelation.Label.Replace("\"", "\\\"");
			if (currentRelation.Description != null)
				currentRelation.Description = currentRelation.Description.Replace("\"", "\\\"");
			//escape the old to for update check sake
			if (oldRelation.Label != null)
				oldRelation.Label = oldRelation.Label.Replace("\"", "\\\"");
			if (oldRelation.Description != null)
				oldRelation.Description = oldRelation.Description.Replace("\"", "\\\"");

			code = $"#region << ***Update relation*** Relation name: {currentRelation.Name} >>\n" +
			"{\n" +
				"\tvar relation = new EntityRelation();\n" +
				$"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
				$"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"{originField.Name}\");\n" +
				$"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
				$"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"{targetField.Name}\");\n" +
				$"\trelation.Id = new Guid(\"{currentRelation.Id}\");\n";

			//name
			if (currentRelation.Name != oldRelation.Name)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldRelation.Name}</span> to <span class='go-red'>{currentRelation.Name}</span>");
			}
			code += $"\trelation.Name = \"{currentRelation.Name}\";\n";

			//label
			if (currentRelation.Label != oldRelation.Label)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldRelation.Label}</span> to <span class='go-red'>{currentRelation.Label}</span>");
			}

			code += $"\trelation.Label = \"{currentRelation.Label}\";\n";

			//description
			if (currentRelation.Description != oldRelation.Description && 
				!(currentRelation.Description == null && oldRelation.Description == "") && 
				!(currentRelation.Description == "" && oldRelation.Description == null))
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>description</span>  from <span class='go-red'>{oldRelation.Description}</span> to <span class='go-red'>{currentRelation.Description}</span>");
			}
			if (currentRelation.Description == null)
			{
				code += $"\trelation.Description = null;\n";
			}
			else
			{
				code += $"\trelation.Description = \"{currentRelation.Description}\";\n";
			}

			//system
			if (currentRelation.System != oldRelation.System)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>system</span>  from <span class='go-red'>{(oldRelation.System).ToString().ToLowerInvariant()}</span> to <span class='go-red'>{(currentRelation.System).ToString().ToLowerInvariant()}</span>");
			}
			code += $"\trelation.System = {(currentRelation.System).ToString().ToLowerInvariant()};\n";

			//relation type
			if (currentRelation.RelationType != oldRelation.RelationType)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>relationType</span>  from <span class='go-red'>{oldRelation.RelationType}</span> to <span class='go-red'>{currentRelation.RelationType}</span>");
			}
			if (currentRelation.RelationType == EntityRelationType.OneToOne)
			{
				code += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
			}
			else if (currentRelation.RelationType == EntityRelationType.OneToMany)
			{
				code += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
			}
			else if (currentRelation.RelationType == EntityRelationType.ManyToMany)
			{
				code += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
			}


			//originEntityId
			if (currentRelation.OriginEntityId != oldRelation.OriginEntityId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>OriginEntityId</span>  from <span class='go-red'>{oldRelation.OriginEntityId}</span> to <span class='go-red'>{currentRelation.OriginEntityId}</span>");
			}
			code += $"\trelation.OriginEntityId = originEntity.Id;\n";

			//OriginFieldId
			if (currentRelation.OriginFieldId != oldRelation.OriginFieldId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>OriginFieldId</span>  from <span class='go-red'>{oldRelation.OriginFieldId}</span> to <span class='go-red'>{currentRelation.OriginFieldId}</span>");
			}
			code += $"\trelation.OriginFieldId = originField.Id;\n";

			//TargetEntityId
			if (currentRelation.TargetEntityId != oldRelation.TargetEntityId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>TargetEntityId</span>  from <span class='go-red'>{oldRelation.TargetEntityId}</span> to <span class='go-red'>{currentRelation.TargetEntityId}</span>");
			}
			code += $"\trelation.TargetEntityId = targetEntity.Id;\n";

			//TargetFieldId
			if (currentRelation.TargetFieldId != oldRelation.TargetFieldId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>TargetFieldId</span>  from <span class='go-red'>{oldRelation.TargetFieldId}</span> to <span class='go-red'>{currentRelation.TargetFieldId}</span>");
			}
			code += $"\trelation.TargetFieldId = targetField.Id;\n";
			code +=
			"\t{\n" +
				$"\tvar response = relMan.Update(relation);\n" +
				$"\tif (!response.Success)\n" +
					$"\t\tthrow new Exception(\"System error 10060. Failed update relation: {currentRelation.Name}. Message:\" + response.Message);\n" +
			"\t}\n" +


			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion

		#region << Role >>
		private string CreateRoleCode(EntityRecord role)
		{

			//escape possible double quotes
			if (role["description"] != null)
				role["description"] = ((string)role["description"]).Replace("\"", "\\\"");

			var response = "" +
$"#region << ***Create role*** Role name: {(string)role["name"]} >>\n" +
"{\n" +
	"\tvar role = new EntityRecord();\n" +
	$"\trole[\"id\"] = new Guid(\"{(Guid)role["id"]}\");\n" +
	$"\trole[\"name\"] = \"{(string)role["name"]}\";\n" +
	$"\trole[\"description\"] = \"{(string)role["description"]}\";\n" +
	"\tvar createRoleResult = recMan.CreateRecord(\"role\", role);\n" +
	"\tif (!createRoleResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Role create with name : {(string)role["name"]}. Message:\" + createRoleResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private string DeleteRoleCode(EntityRecord role)
		{
			var response = "" +
$"#region << ***Delete role*** Role name: {(string)role["name"]} >>\n" +
"{\n" +
	$"\tvar deleteRoleResult = recMan.DeleteRecord(\"role\", new Guid(\"{(Guid)role["id"]}\"));\n" +
	"\tif (!deleteRoleResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Role delete with name : {(string)role["name"]}. Message:\" + deleteRoleResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateRoleCode(EntityRecord currentRole, EntityRecord oldRole)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentRole["description"] != null)
				currentRole["description"] = ((string)currentRole["description"]).Replace("\"", "\\\"");
			//escape the for update check sake
			if (oldRole["description"] != null)
				oldRole["description"] = ((string)oldRole["description"]).Replace("\"", "\\\"");

			code =
$"#region << ***Update role*** Role name: {(string)currentRole["name"]} >>\n" +
"{\n" +
	"\tvar patchObject = new EntityRecord();\n" +
	$"\tpatchObject[\"id\"] = new Guid(\"{(Guid)currentRole["id"]}\");\n";

			//name
			if ((string)currentRole["name"] != (string)oldRole["name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"name\"] = \"{(string)currentRole["name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{(string)oldRole["name"]}</span> to <span class='go-red'>{(string)currentRole["name"]}</span>");
			}
			//label	
			if ((string)currentRole["description"] != (string)oldRole["description"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"description\"] = \"{(string)currentRole["description"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>description</span> from <span class='go-red'>{(string)oldRole["description"]}</span> to <span class='go-red'>{(string)currentRole["description"]}</span>");
			}

			code +=
				"\tvar updateRoleResult = recMan.UpdateRecord(\"role\", patchObject);\n" +
				"\tif (!updateRoleResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Role update with name : {(string)currentRole["name"]}. Message:\" + updateRoleResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion

		#region << Helpers >>
		private bool CheckFieldPermissionsHasUpdate(DbFieldPermissions oldFieldPermissions, DbFieldPermissions currentFieldPermissions)
		{
			var response = false;

			#region << Fast check based on count >>
			if(oldFieldPermissions.CanRead.Count != currentFieldPermissions.CanRead.Count) {
				return true;
			}
			if(oldFieldPermissions.CanUpdate.Count != currentFieldPermissions.CanUpdate.Count) {
				return true;
			}
			#endregion

			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			var currentFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var currentFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();

			#region << Fill dictionaries >>
			foreach (var permission in oldFieldPermissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldFieldPermissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}
			foreach (var permission in currentFieldPermissions.CanRead)
			{
				currentFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in currentFieldPermissions.CanUpdate)
			{
				currentFieldCanUpdatePermissionDictionary[permission] = true;
			}
			#endregion

			#region << Check if all current permissions are existing in the old Field >>

			foreach (var permission in currentFieldPermissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					return true;
				}
			}
			foreach (var permission in currentFieldPermissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					return true;
				}
			}
			#endregion

			#region << Check if all old permissions are existing in the current Field >>

			foreach (var permission in oldFieldPermissions.CanRead)
			{
				if (!currentFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					return true;
				}
			}
			foreach (var permission in oldFieldPermissions.CanUpdate)
			{
				if (!currentFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					return true;
				}
			}
			#endregion

			return response;
		}
		#endregion

		#endregion

	}
}

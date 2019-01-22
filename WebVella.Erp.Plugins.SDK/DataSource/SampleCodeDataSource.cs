//using System;
//using System.Collections.Generic;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Web.Models;

//namespace WebVella.Erp.Plugins.SDK.DataSource
//{
//	public class SampleCodeDataSource : CodeDataSource
//	{
//		public SampleCodeDataSource() : base()
//		{
//			Id = new Guid("f5546db0-bffe-46de-b616-54f66c633535");
//			Name = "SampleCodeDS";
//			Description = "This is a sample code data source.";
//			ResultModel = "List<EntityRecord>";

//			//define custom meta
//			DataSourceModelFieldMeta dsMeta = new DataSourceModelFieldMeta();
//			dsMeta.EntityName = string.Empty;
//			dsMeta.Name = "id";
//			dsMeta.Type = FieldType.GuidField;
//			Fields.Add(dsMeta);
//			dsMeta = new DataSourceModelFieldMeta();
//			dsMeta.EntityName = string.Empty;
//			dsMeta.Name = "name";
//			dsMeta.Type = FieldType.TextField;
//			Fields.Add(dsMeta);

//			Parameters.Add(new DataSourceParameter { Name = "testText", Type = "text", Value = "test value" });
//			Parameters.Add(new DataSourceParameter { Name = "testGuid", Type = "guid", Value = Guid.Empty.ToString() });
//			Parameters.Add(new DataSourceParameter { Name = "testInt", Type = "int", Value = 0.ToString() });
//			Parameters.Add(new DataSourceParameter { Name = "testDecimal", Type = "decimal", Value = (0.0f).ToString() });
//			Parameters.Add(new DataSourceParameter { Name = "testDate", Type = "date", Value = "now" });
//		}

//		public override object Execute(Dictionary<string, object> arguments)
//		{
//			if (arguments.ContainsKey("PageModel"))
//			{
//				//just for test if pageModel is working with no problem
//				//try to read from existing database source - if not found generate random one
//				PageDataModel dataModel = arguments["PageModel"] as PageDataModel;
//				if (dataModel != null)
//				{
//					try
//					{
//						//GetProperty method throws PropertyDoesNotExistException exception if property is not found
//						//so use try catch 
//						var listData = dataModel.GetProperty("List");
//						if (listData != null)
//							return listData;
//					}
//					catch(PropertyDoesNotExistException)
//					{
//					}
//				}
//			}

//			List<EntityRecord> result = new List<EntityRecord>();
//			for (int i = 0; i < 10; i++)
//			{
//				EntityRecord rec = new EntityRecord();
//				rec["id"] = Guid.NewGuid();
//				rec["name"] = $"name{i}";
//				result.Add(rec);
//			}
//			return result;
//		}
//	}
//}

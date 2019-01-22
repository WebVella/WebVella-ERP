using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.DataSource
{
	public class CurrentDate : CodeDataSource
	{
		public CurrentDate() : base()
		{
			Id = new Guid("64207638-D75E-4A25-9965-6E35B0AA835A");
			Name = "CurrentDate";
			Description = "Provides current date plus simple calculations";
			ResultModel = "DateTime";

			//define custom meta
			//DataSourceModelFieldMeta dsMeta = new DataSourceModelFieldMeta();
			//dsMeta.EntityName = string.Empty;
			//dsMeta.Name = "CurrentDate";
			//dsMeta.Type = FieldType.DateField;
			//Fields.Add(dsMeta);


			Parameters.Add(new DataSourceParameter { Name = "addYears", Type = "int", Value = "0" });
			Parameters.Add(new DataSourceParameter { Name = "addMonths", Type = "int", Value = "0" });
			Parameters.Add(new DataSourceParameter { Name = "addDays", Type = "int", Value = "0" });
			Parameters.Add(new DataSourceParameter { Name = "addHours", Type = "int", Value = "0" });
			Parameters.Add(new DataSourceParameter { Name = "addMinutes", Type = "int", Value = "0" });
			Parameters.Add(new DataSourceParameter { Name = "addSeconds", Type = "int", Value = "0" });

		}

		public override object Execute(Dictionary<string, object> arguments)
		{
			var nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Local);

			int addYears, addMonths, addDays, addHours, addMinutes, addSeconds;
			addYears = addMonths = addDays = addHours = addMinutes = addSeconds = 0;

			if (arguments.ContainsKey("addYears"))
				addYears = (int)arguments["addYears"];

			if (arguments.ContainsKey("addMonths"))
				addMonths = (int)arguments["addMonths"];

			if (arguments.ContainsKey("addDays"))
				addDays = (int)arguments["addDays"];

			if (arguments.ContainsKey("addHours"))
				addHours = (int)arguments["addHours"];

			if (arguments.ContainsKey("addMinutes"))
				addMinutes = (int)arguments["addMinutes"];

			if (arguments.ContainsKey("addSeconds"))
				addSeconds = (int)arguments["addSeconds"];

			return nowDate.AddYears(addYears).AddMonths(addMonths).AddDays(addDays).AddMinutes(addMinutes).AddSeconds(addSeconds);
		
		}
	}
}

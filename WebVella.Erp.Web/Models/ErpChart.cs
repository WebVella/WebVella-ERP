using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class ErpChart
	{
		[JsonProperty(PropertyName = "type")]
		public ErpChartType Type { get; set; } = ErpChartType.Line;

		[JsonProperty(PropertyName = "data")]
		public ErpChartData Data { get; set; }

		//Hardcoded for now
		//[JsonProperty(PropertyName = "options")]
		//public ErpChartOptions Options { get; set; }

		/// <summary>
		/// A chart options json.
		/// </summary>
		[JsonProperty(PropertyName = "options")]
		public EntityRecord Options { get; set; }

		public ErpChart(ErpChartType type, List<string> labels, List<ErpChartDataset> datasets, bool ShowLegend)
		{
			Type = type;
			Data = new ErpChartData()
			{
				Labels = labels,
				Datasets = datasets
			};

			var datasetsLabelsCount = 0;
			foreach (var dataset in datasets)
			{
				if (!string.IsNullOrWhiteSpace(dataset.Label)) {
					datasetsLabelsCount++;
				}
				if (dataset.Fill == null && Type == ErpChartType.Area)
				{
					dataset.Fill = true;
				}
				else if (dataset.Fill == null) {
					dataset.Fill = false;
				}
				if (dataset.BorderColor == null) {
					switch (Type) {
						case ErpChartType.Line:
						case ErpChartType.Area:
							dataset.BorderColor = "rgba(0, 150, 136,1)";
							break;
						default:
							dataset.BorderColor = new List<string>();
							foreach (var item in dataset.Data)
								((List<string>)dataset.BorderColor).Add("rgba(0, 150, 136,1)");
							break;
					}
				}
				else if (dataset.BorderColor != null && Type != ErpChartType.Line && Type != ErpChartType.Area && 
					((List<string>)dataset.BorderColor).Count == 1 && dataset.Data.Count > 1) {
					var borderColor = ((List<string>)dataset.BorderColor).First();
					dataset.BorderColor = new List<string>();
					foreach (var item in dataset.Data)
						((List<string>)dataset.BorderColor).Add(borderColor);
				}

				if (dataset.BackgroundColor == null)
				{

					switch (Type)
					{
						case ErpChartType.Line:
						case ErpChartType.Area:
							dataset.BackgroundColor = "rgba(0, 150, 136,0.15)";
							break;
						default:
							dataset.BackgroundColor = new List<string>();
							foreach (var item in dataset.Data)
								((List<string>)dataset.BackgroundColor).Add("rgba(0, 150, 136,0.15)");
							break;
					}
				}
				else if (dataset.BackgroundColor != null && Type != ErpChartType.Line && Type != ErpChartType.Area 
					&& ((List<string>)dataset.BackgroundColor).Count == 1 && dataset.Data.Count > 1 )
				{
					var bgColor = ((List<string>)dataset.BackgroundColor).First();
					dataset.BackgroundColor = new List<string>();
					foreach (var item in dataset.Data)
						((List<string>)dataset.BackgroundColor).Add(bgColor);
				}
			}

			var options = new EntityRecord();

			#region << General >>
			{
				options["responsive"] = true;
				options["maintainAspectRatio"] = false;
				if (Type == ErpChartType.Doughnut) {
					options["cutoutPercentage"] = 70;
				}
			}
			#endregion

			#region << Layout >>
			{
				var layout = new EntityRecord();
				layout["padding"] = 0;
				options["layout"] = layout;
			}
			#endregion

			#region << Animation >>
			{
				var animation = new EntityRecord();
				animation["duration"] = 0;
				options["animation"] = animation;
			}
			#endregion

			#region << Tooltips >>
			{
				var tooltips = new EntityRecord();
				tooltips["enabled"] = false;
				//tooltips["displayColors"] = false;
				//tooltips["callbacks"] = "{{tooltipCallback}}";
				options["tooltips"] = tooltips;
			}
			#endregion

			#region << legend >>
			{
				var legend = new EntityRecord();
				if (ShowLegend)
				{
					legend["display"] = true;
					switch (Type) {
						case ErpChartType.Pie:
						case ErpChartType.Doughnut:
							legend["position"] = "bottom";
							break;
						default:
							legend["position"] = "bottom left";
							break;
					}
				}
				else
					legend["display"] = false;

				options["legend"] = legend;
			}
			#endregion

			#region << Scales >>
			{
				var scales = new EntityRecord();

				//xAxes
				var xAxes = new List<EntityRecord>();
				var xAxesFirst = new EntityRecord();
				if (Type == ErpChartType.Pie || Type == ErpChartType.Doughnut)
				{
					xAxesFirst["display"] = false;
				}
				xAxesFirst["beginAtZero"] = false;

				if (Type == ErpChartType.Bar || Type == ErpChartType.HorizontalBar)
					xAxesFirst["offset"] = true;
				else
					xAxesFirst["offset"] = false;

				var xAxesFirstGridLine = new EntityRecord();
				xAxesFirstGridLine["display"] = false;
				xAxesFirst["gridLines"] = xAxesFirstGridLine;

				var xAxesFirstScaleLabel = new EntityRecord();
				xAxesFirstScaleLabel["display"] = false;
				xAxesFirst["scaleLabel"] = xAxesFirstScaleLabel;

				var xAxesFirstTicks = new EntityRecord();
				xAxesFirstTicks["beginAtZero"] = false;
				xAxesFirstTicks["min"] = 0;
				xAxesFirst["ticks"] = xAxesFirstTicks;

				xAxes.Add(xAxesFirst);

				//yAxes
				var yAxes = new List<EntityRecord>();
				var yAxesFirst = new EntityRecord();

				if (Type == ErpChartType.Pie || Type == ErpChartType.Doughnut)
				{
					yAxesFirst["display"] = false;
				}

				yAxesFirst["beginAtZero"] = false;

				if (Type == ErpChartType.Bar || Type == ErpChartType.HorizontalBar)
					yAxesFirst["offset"] = true;
				else
					yAxesFirst["offset"] = false;

				var yAxesFirstGridLine = new EntityRecord();
				yAxesFirstGridLine["display"] = false;
				yAxesFirst["gridLines"] = yAxesFirstGridLine;

				var yAxesFirstScaleLabel = new EntityRecord();
				yAxesFirstScaleLabel["display"] = false;
				yAxesFirst["scaleLabel"] = yAxesFirstScaleLabel;

				var yAxesFirstTicks = new EntityRecord();
				yAxesFirstTicks["padding"] = 0;
				yAxesFirstTicks["display"] = true;
				yAxesFirstTicks["beginAtZero"] = false;
				yAxesFirstTicks["min"] = 0;
				yAxesFirst["ticks"] = yAxesFirstTicks;


				yAxes.Add(yAxesFirst);

				scales["yAxes"] = yAxes;
				scales["xAxes"] = xAxes;
				options["scales"] = scales;
			}
			#endregion

			switch (type) {
				case ErpChartType.Line:
					break;
				case ErpChartType.Bar:
					break;
				case ErpChartType.Doughnut:
				case ErpChartType.Pie:
					break;
				default:
					break;
			}

			Options = options;
		}

	}
}

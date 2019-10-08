using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Chart", Library = "WebVella", Description = "Line,area,pie, doughnut, bar, horizontal bar", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcChart : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcChart([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcChartOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			[JsonProperty(PropertyName = "datasets")]
			public string Datasets { get; set; } = "";

			[JsonProperty(PropertyName = "labels")]
			public string Labels { get; set; } = "";

			[JsonProperty(PropertyName = "show_legend")]
			public bool ShowLegend { get; set; } = false;

			[JsonProperty(PropertyName = "type")]
			public WvChartType Type { get; set; } = WvChartType.Line;

			[JsonProperty(PropertyName = "height")]
			public string Height { get; set; } = null;

			[JsonProperty(PropertyName = "width")]
			public string Width { get; set; } = null;
		}

		public async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
		{
			ErpPage currentPage = null;
			try
			{
				#region << Init >>
				if (context.Node == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query parameter 'nid', when requesting this component"));
				}

				var pageFromModel = context.DataModel.GetProperty("Page");
				if (pageFromModel == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel cannot be null"));
				}
				else if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}

				var options = new PcChartOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcChartOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;

                if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
                {
                    var isVisible = true;
                    var isVisibleDS = context.DataModel.GetPropertyValueByDataSource(options.IsVisible);
                    if (isVisibleDS is string && !String.IsNullOrWhiteSpace(isVisibleDS.ToString()))
                    {
                        if (Boolean.TryParse(isVisibleDS.ToString(), out bool outBool))
                        {
                            isVisible = outBool;
                        }
                    }
                    else if (isVisibleDS is Boolean)
                    {
                        isVisible = (bool)isVisibleDS;
                    }
                    ViewBag.IsVisible = isVisible;

                    var theme = new Theme();
                    var colorOptionsList = new List<string>() {theme.TealColor,theme.PinkColor, theme.GreenColor, theme.OrangeColor,theme.RedColor,theme.PurpleColor,theme.DeepPurpleColor,
                    theme.BlueColor, theme.LightBlueColor,theme.CyanColor,theme.GreenColor,theme.IndigoColor,theme.LightGreenColor,theme.LimeColor,theme.YellowColor,
                    theme.AmberColor,theme.DeepOrangeColor};

                    var bkgColorOptionsList = new List<string>() {theme.TealLightColor,theme.PinkLightColor, theme.GreenLightColor, theme.OrangeLightColor,theme.RedLightColor,theme.PurpleLightColor,theme.DeepPurpleLightColor,
                    theme.BlueLightColor, theme.LightBlueLightColor,theme.CyanLightColor,theme.GreenLightColor,theme.IndigoLightColor,theme.LightGreenLightColor,theme.LimeLightColor,theme.YellowLightColor,
                    theme.AmberLightColor,theme.DeepOrangeLightColor};

                    List<WvChartDataset> dataSets = context.DataModel.GetPropertyValueByDataSource(options.Datasets) as List<WvChartDataset> ?? new List<WvChartDataset>();

                    if (dataSets == null || dataSets.Count == 0)
                    {
                        var decimalList = new List<decimal>();
                        decimalList = context.DataModel.GetPropertyValueByDataSource(options.Datasets) as List<decimal> ?? new List<decimal>();
                        if ((dataSets == null || dataSets.Count == 0) && !String.IsNullOrWhiteSpace(options.Datasets) && options.Datasets.Contains(","))
                        {
                            var optionValueCsv = options.Datasets.Split(",");
                            var csvDecimalList = new List<decimal>();
                            var csvParseHasError = false;
                            foreach (var valueString in optionValueCsv)
                            {
                                if (Decimal.TryParse(valueString.Trim(), out decimal outDecimal))
                                    csvDecimalList.Add(outDecimal);
                                else
                                {
                                    csvParseHasError = true;
                                    break;
                                }
                            }
                            if (!csvParseHasError)
                                decimalList = csvDecimalList;
                        }

                        if (decimalList != null && decimalList.Count > 0)
                        {
                            var dataSet = new WvChartDataset();
                            dataSet.Data = decimalList;
                            if (options.Type == WvChartType.Area || options.Type == WvChartType.Line)
                            {
                                dataSet.BorderColor = colorOptionsList[0];
                                dataSet.BackgroundColor = bkgColorOptionsList[0];
                            }
                            else
                            {
                                dataSet.BorderColor = new List<string>();
                                dataSet.BackgroundColor = new List<string>();
                                var index = 0;
                                foreach (var value in decimalList)
                                {
                                    ((List<string>)dataSet.BorderColor).Add(colorOptionsList[index]);
                                    if (options.Type == WvChartType.Bar || options.Type == WvChartType.HorizontalBar)
                                        ((List<string>)dataSet.BackgroundColor).Add(bkgColorOptionsList[index]);
                                    else
                                        ((List<string>)dataSet.BackgroundColor).Add(colorOptionsList[index]);
                                    index++;
                                }
                            }
                            dataSets.Add(dataSet);
                        }
                    }

                    List<string> labels = context.DataModel.GetPropertyValueByDataSource(options.Labels) as List<string> ?? new List<string>();
                    if ((labels == null || labels.Count == 0) && !String.IsNullOrWhiteSpace(options.Labels) && options.Labels.Contains(","))
                    {
                        labels = options.Labels.Split(",").ToList();
                    }

                    ViewBag.DataSets = dataSets;
                    ViewBag.Labels = labels;
                    ViewBag.ShowLegend = options.ShowLegend;
                    ViewBag.Height = options.Height;
                    ViewBag.Width = options.Width;
                    ViewBag.Type = (WvChartType)options.Type;
                }

                var chartTypeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvChartType>();
                chartTypeOptions.First(x => x.Value == "4").Label = "area";
                ViewBag.ChartTypeOptions = chartTypeOptions;

                switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						return await Task.FromResult<IViewComponentResult>(View("Options"));
					case ComponentMode.Help:
						return await Task.FromResult<IViewComponentResult>(View("Help"));
					default:
						ViewBag.Error = new ValidationException()
						{
							Message = "Unknown component mode"
						};
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}

			}
			catch (ValidationException ex)
			{
				ViewBag.Error = ex;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.Error = new ValidationException()
				{
					Message = ex.Message
				};
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}

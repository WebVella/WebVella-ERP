using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Grid Filter Field", Library = "WebVella", Description = "used with Grids to generate filters", Version = "0.0.1", IconClass = "fas fa-search")]
	public class PcGridFilterField : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcGridFilterField([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcGridFilterFieldOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			[JsonProperty(PropertyName = "name")]
			public string Name { get; set; } = "field";

			[JsonProperty(PropertyName = "label")]
			public string Label { get; set; } = "";

			[JsonProperty(PropertyName = "try_connect_to_entity")]
			public bool TryConnectToEntity { get; set; } = false;

			[JsonProperty(PropertyName = "field_type")]
			public FieldType FieldType { get; set; } = FieldType.TextField;  //will be overrided with Url Query name: q_fieldName_t

			[JsonProperty(PropertyName = "query_type")]
			public FilterType QueryType { get; set; } = FilterType.CONTAINS;  //will be overrided with Url Query name: q_fieldName_t

			[JsonProperty(PropertyName = "query_options")]
			public List<FilterType> QueryOptions { get; set; } = new List<FilterType>(); //if not set will be initialized with default set

			[JsonProperty(PropertyName = "prefix")]
			public string Prefix { get; set; } = "";
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

				var options = new PcGridFilterFieldOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcGridFilterFieldOptions>(context.Options.ToString());
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
                    if (!isVisible && context.Mode == ComponentMode.Display)
                        return await Task.FromResult<IViewComponentResult>(Content(""));


                }

                if (options.QueryOptions == null)
					options.QueryOptions = new List<FilterType>();

				var selectedQueryOptionsConverted = new List<string>();
				foreach (var option in options.QueryOptions)
				{
					selectedQueryOptionsConverted.Add(((int)option).ToString());
				}
				ViewBag.ConvertedSelectedQueryOptions = selectedQueryOptionsConverted;

				if (context.Mode == ComponentMode.Options) {
					ViewBag.FieldTypeOptions = ModelExtensions.GetEnumAsSelectOptions<FieldType>();
					var filterOptions = ModelExtensions.GetEnumAsSelectOptions<FilterType>();
					var idField = filterOptions.Single(x => x.Value == ((int)FilterType.Undefined).ToString());
					filterOptions.Remove(idField);
					ViewBag.FilterTypeOptions = filterOptions;
				}

				ViewBag.ValueOptions = new List<SelectOption>();
				var entity = context.DataModel.GetProperty("Entity");
				if (options.TryConnectToEntity)
				{
					var fieldName = options.Name;
					var entityField = ((Entity)entity).Fields.FirstOrDefault(x => x.Name == fieldName);
					if (entityField != null)
					{
						//Connection success override the local options
						//Init model
						if (String.IsNullOrWhiteSpace(options.Label)) {
							options.Label = entityField.Label;
						}

						//Specific model properties
						var fieldOptions = new List<SelectOption>();
						switch (entityField.GetFieldType())
						{
							case FieldType.AutoNumberField:
								options.FieldType = FieldType.AutoNumberField;
								break;
							case FieldType.CheckboxField:
								options.FieldType = FieldType.CheckboxField;
								break;
							case FieldType.CurrencyField:
								options.FieldType = FieldType.CurrencyField;
								break;
							case FieldType.DateField:
								options.FieldType = FieldType.DateField;
								break;
							case FieldType.DateTimeField:
								options.FieldType = FieldType.DateTimeField;
								break;
							case FieldType.EmailField:
								options.FieldType = FieldType.EmailField;
								break;
							case FieldType.FileField:
								options.FieldType = FieldType.FileField;
								break;
							case FieldType.GuidField:
								options.FieldType = FieldType.GuidField;
								break;
							case FieldType.HtmlField:
								options.FieldType = FieldType.HtmlField;
								break;
							case FieldType.ImageField:
								options.FieldType = FieldType.ImageField;
								break;
							case FieldType.MultiLineTextField:
								options.FieldType = FieldType.MultiLineTextField;
								break;
							case FieldType.NumberField:
								options.FieldType = FieldType.NumberField;
								break;
							case FieldType.PercentField:
								options.FieldType = FieldType.PercentField;
								break;
							case FieldType.PhoneField:
								options.FieldType = FieldType.PhoneField;
								break;
							case FieldType.SelectField:
								options.FieldType = FieldType.SelectField;
								var selectField = ((SelectField)entityField);
								ViewBag.ValueOptions = selectField.Options;
								break;
							case FieldType.MultiSelectField:
								options.FieldType = FieldType.MultiSelectField;
								break;
							case FieldType.TextField:
								options.FieldType = FieldType.TextField;
								break;
							case FieldType.UrlField:
								options.FieldType = FieldType.UrlField;
								break;
							default:
								throw new Exception("No such field Type");
						}
					}
				}

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

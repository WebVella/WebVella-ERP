using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Utils;
using WebVella.Erp.Recurrence;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Task Repeat Recurrence Set", Library = "WebVella", Description = "special component for setting task occurrence", Version = "0.0.1", IconClass = "fas fa-retweet")]
	public class PcTaskRepeatRecurrenceSet : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcTaskRepeatRecurrenceSet([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcTaskRepeatRecurrenceSetOptions
		{
			[JsonProperty(PropertyName = "records")]
			public string Records { get; set; } = "";
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

				var options = new PcTaskRepeatRecurrenceSetOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcTaskRepeatRecurrenceSetOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.CurrentUser = SecurityContext.CurrentUser;
				ViewBag.CurrentUserJson = JsonConvert.SerializeObject(SecurityContext.CurrentUser);
				ViewBag.StartDate = null;
				ViewBag.EndDate = null;

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					var record = (EntityRecord)context.DataModel.GetProperty("Record");
					if (record == null)
						throw new ValidationException() { Message = "Record not found" };

					if (record.Properties.ContainsKey("start_time") && record["start_time"] is DateTime?)
						ViewBag.StartDate = record["start_time"];
					if (record.Properties.ContainsKey("end_time") && record["end_time"] is DateTime?)
						ViewBag.EndDate = record["end_time"];

					ViewBag.RecurrenceTemplateString = "";
					if (record.Properties.ContainsKey("recurrence_template") && record["recurrence_template"] is string)
					{
						ViewBag.RecurrenceTemplateString = (string)record["recurrence_template"];
					}
					ViewBag.TemplateDefault = JsonConvert.SerializeObject(new RecurrenceTemplate());

					ViewBag.RecurrenceTypeOptions = JsonConvert.SerializeObject(ModelExtensions.GetEnumAsSelectOptions<RecurrenceType>());
					ViewBag.RecurrenceEndTypeOptions = JsonConvert.SerializeObject(ModelExtensions.GetEnumAsSelectOptions<RecurrenceEndType>());
					var periodTypes = ModelExtensions.GetEnumAsSelectOptions<RecurrencePeriodType>();
					periodTypes = periodTypes.FindAll(x => x.Value != "0" && x.Value != "1" && x.Value != "2").ToList(); // remove seconds minutes and hour
					ViewBag.PeriodTypeOptions = JsonConvert.SerializeObject(periodTypes);
					ViewBag.RecurrenceChangeTypeOptions = JsonConvert.SerializeObject(ModelExtensions.GetEnumAsSelectOptions<RecurrenceChangeType>());
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

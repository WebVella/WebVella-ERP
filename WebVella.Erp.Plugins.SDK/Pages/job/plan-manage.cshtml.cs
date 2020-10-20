using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Job
{
	public class PlanManageModel : BaseErpPageModel
	{
		public PlanManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		[BindProperty]
		public bool Enabled { get; set; } = false;

		public Guid Id { get; set; } = Guid.Empty;

		[BindProperty]
		public string Name { get; set; } = "";

		[BindProperty]
		public DateTime? StartDate { get; set; } = null;

		[BindProperty]
		public DateTime? EndDate { get; set; } = null;

		[BindProperty]
		public SchedulePlanType Type { get; set; } = SchedulePlanType.Interval;

		[BindProperty]
		public int? IntervalInMinutes { get; set; } = null;

		[BindProperty]
		public List<string> ScheduledDays { get; set; } = new List<string>();

		[BindProperty]
		public DateTime? StartTimespan { get; set; } = null;

		[BindProperty]
		public DateTime? EndTimespan { get; set; } = null;

		public List<SelectOption> SchedulePlanTypeOptions { get; set; } = ModelExtensions.GetEnumAsSelectOptions<SchedulePlanType>();

		public DateTime? NextTriggerTime { get; set; } = null;

		public string JobTypeName { get; set; } = null;

		private SchedulePlan Plan { get; set; } = null;

		public List<SelectOption> WeekOptions { get; set; } = new List<SelectOption>() {
			new SelectOption(DayOfWeek.Monday.ToString(),DayOfWeek.Monday.ToString()),
			new SelectOption(DayOfWeek.Tuesday.ToString(),DayOfWeek.Tuesday.ToString()),
			new SelectOption(DayOfWeek.Wednesday.ToString(),DayOfWeek.Wednesday.ToString()),
			new SelectOption(DayOfWeek.Thursday.ToString(),DayOfWeek.Thursday.ToString()),
			new SelectOption(DayOfWeek.Friday.ToString(),DayOfWeek.Friday.ToString()),
			new SelectOption(DayOfWeek.Saturday.ToString(),DayOfWeek.Saturday.ToString()),
			new SelectOption(DayOfWeek.Sunday.ToString(),DayOfWeek.Sunday.ToString()),
		};

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		private void InitPage()
		{
			HeaderToolbar.AddRange(AdminPageUtils.GetJobAdminSubNav("plan"));

			if (RecordId != null)
			{
				Plan = ScheduleManager.Current.GetSchedulePlan(RecordId ?? Guid.Empty);
			}


			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/server/job/l/plan";
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (Plan == null)
				return NotFound();



			Enabled = Plan.Enabled;
			Id = Plan.Id;
			Name = Plan.Name;

			StartDate = Plan.StartDate;
			if (StartDate.HasValue)
				StartDate = StartDate.Value;

			EndDate = Plan.EndDate;
			if (EndDate.HasValue)
				EndDate = EndDate.Value;

			Type = Plan.Type;
			NextTriggerTime = Plan.NextTriggerTime;
			IntervalInMinutes = Plan.IntervalInMinutes;
			JobTypeName = $"{Plan.JobType.Name} ({Plan.JobType.Id})";

			if (Plan.ScheduledDays.ScheduledOnSunday) ScheduledDays.Add(DayOfWeek.Sunday.ToString());
			if (Plan.ScheduledDays.ScheduledOnMonday) ScheduledDays.Add(DayOfWeek.Monday.ToString());
			if (Plan.ScheduledDays.ScheduledOnTuesday) ScheduledDays.Add(DayOfWeek.Tuesday.ToString());
			if (Plan.ScheduledDays.ScheduledOnWednesday) ScheduledDays.Add(DayOfWeek.Wednesday.ToString());
			if (Plan.ScheduledDays.ScheduledOnThursday) ScheduledDays.Add(DayOfWeek.Thursday.ToString());
			if (Plan.ScheduledDays.ScheduledOnFriday) ScheduledDays.Add(DayOfWeek.Friday.ToString());
			if (Plan.ScheduledDays.ScheduledOnSaturday) ScheduledDays.Add(DayOfWeek.Saturday.ToString());

			if (Plan.StartTimespan != null) StartTimespan = new DateTime(2000, 1, 1, 0,0,0,DateTimeKind.Local).AddMinutes(Plan.StartTimespan.Value);
			if (Plan.EndTimespan != null) EndTimespan = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local).AddMinutes(Plan.EndTimespan.Value);

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");

			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			try
			{
				Id = Plan.Id;
				Plan.Name = Name;
				Plan.StartDate = StartDate;
				Plan.EndDate = EndDate;
				Plan.Type = Type;

				if (StartDate.HasValue)
					Plan.StartDate = StartDate.ConvertAppDateToUtc();
				else
					Plan.StartDate = null;

				if (EndDate.HasValue)
					Plan.EndDate = EndDate.ConvertAppDateToUtc();
				else
					Plan.EndDate = null;

				if (StartTimespan.HasValue)
				{
					StartTimespan = StartTimespan.ConvertToAppDate();
					Plan.StartTimespan = StartTimespan.Value.Hour * 60 + StartTimespan.Value.Minute;
				}
				else
					Plan.StartTimespan = null;

				if (EndTimespan.HasValue)
				{
					EndTimespan = EndTimespan.ConvertToAppDate();

					Plan.EndTimespan = EndTimespan.Value.Hour * 60 + EndTimespan.Value.Minute;
					if (Plan.EndTimespan == 0)
						Plan.EndTimespan = 1440;
				}
				else
					Plan.EndTimespan = null;

				Plan.IntervalInMinutes = IntervalInMinutes;

				Plan.Enabled = Enabled;

				Plan.ScheduledDays.ScheduledOnMonday = false;
				Plan.ScheduledDays.ScheduledOnTuesday = false;
				Plan.ScheduledDays.ScheduledOnWednesday = false;
				Plan.ScheduledDays.ScheduledOnThursday = false;
				Plan.ScheduledDays.ScheduledOnFriday = false;
				Plan.ScheduledDays.ScheduledOnSaturday = false;
				Plan.ScheduledDays.ScheduledOnSunday = false;

				foreach (var schDay in ScheduledDays)
				{
					if (DayOfWeek.Monday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnMonday = true;

					if (DayOfWeek.Tuesday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnTuesday = true;

					if (DayOfWeek.Wednesday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnWednesday = true;

					if (DayOfWeek.Thursday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnThursday = true;

					if (DayOfWeek.Friday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnFriday = true;

					if (DayOfWeek.Saturday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnSaturday = true;

					if (DayOfWeek.Sunday.ToString() == schDay)
						Plan.ScheduledDays.ScheduledOnSunday = true;
				}

				ValidationException valEx = new ValidationException();

				if (string.IsNullOrWhiteSpace(Name))
					valEx.AddError("Name", "Name is required field and cannot be empty.");

				if (Plan.StartDate >= Plan.EndDate)
				{
					valEx.AddError("StartDate", "Start date must be before end date.");
					valEx.AddError("EndDate", "End date must be greater than start date.");
				}

				if ((Plan.Type == SchedulePlanType.Daily || Plan.Type == SchedulePlanType.Interval) && !Plan.ScheduledDays.HasOneSelectedDay())
					valEx.AddError("ScheduledDays", "At least one day have to be selected for schedule days field.");

				if (Plan.Type == SchedulePlanType.Interval && (Plan.IntervalInMinutes <= 0 || Plan.IntervalInMinutes > 1440))
					valEx.AddError("IntervalInMinutes", "The value of Interval in minutes field must be greater than 0 and less or  equal than 1440.");

				valEx.CheckAndThrow();

				if (Plan.Enabled)
					Plan.NextTriggerTime = ScheduleManager.Current.FindSchedulePlanNextTriggerDate(Plan);
				else
					Plan.NextTriggerTime = null;

				ScheduleManager.Current.UpdateSchedulePlan(Plan);

				BeforeRender();
				return Redirect(ReturnUrl);
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;

				BeforeRender();
				return Page();
			}
		}
	}
}
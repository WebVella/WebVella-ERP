using AutoMapper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Plugins.Mail.Jobs;
using WebVella.Erp.Plugins.Mail.Services;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "mail";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
				SetSchedulePlans();
			}
		}

		public override IEnumerable<Type> GetJobTypes()
		{
			var list = new List<Type>();
			list.Add( typeof(ProcessSmtpQueueJob) );
			return list;
		}

		public override void SetAutoMapperConfiguration(MapperConfigurationExpression cfg)
		{
			Api.AutoMapper.MailPluginAutoMapperConfiguration.Configure(cfg);
			base.SetAutoMapperConfiguration(cfg);
		}

		private void SetSchedulePlans()
		{
			DateTime utcNow = DateTime.UtcNow;

			#region << StartTasksOnStartDate >>
			{
				Guid checkBotSchedulePlanId = new Guid("8f410aca-a537-4c3f-b49b-927670534c07");
				string planName = "Start tasks to process SMTP email queue";
				SchedulePlan checkBotSchedulePlan = ScheduleManager.Current.GetSchedulePlan(checkBotSchedulePlanId);

				if (checkBotSchedulePlan == null)
				{
					checkBotSchedulePlan = new SchedulePlan();
					checkBotSchedulePlan.Id = checkBotSchedulePlanId;
					checkBotSchedulePlan.Name = planName;
					checkBotSchedulePlan.Type = SchedulePlanType.Interval;
					checkBotSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 10, 0, DateTimeKind.Utc);
					checkBotSchedulePlan.EndDate = null;
					checkBotSchedulePlan.ScheduledDays = new SchedulePlanDaysOfWeek()
					{
						ScheduledOnMonday = true,
						ScheduledOnTuesday = true,
						ScheduledOnWednesday = true,
						ScheduledOnThursday = true,
						ScheduledOnFriday = true,
						ScheduledOnSaturday = true,
						ScheduledOnSunday = true
					};
					checkBotSchedulePlan.IntervalInMinutes = 10;
					checkBotSchedulePlan.StartTimespan = 0;
					checkBotSchedulePlan.EndTimespan = 1440;
					checkBotSchedulePlan.JobTypeId = new Guid("9b301dca-6c81-40dd-887c-efd31c23bd77");
					checkBotSchedulePlan.JobAttributes = null;
					checkBotSchedulePlan.Enabled = true;
					checkBotSchedulePlan.LastModifiedBy = null;

					ScheduleManager.Current.CreateSchedulePlan(checkBotSchedulePlan);
				}
			}
			#endregion

		}

		class PluginSettings
		{
			[JsonProperty(PropertyName = "version")]
			public int Version { get; set; }
		}
	}
}

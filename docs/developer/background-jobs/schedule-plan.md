<!--{"sort_order":3, "name": "schedule-plan", "label": "Schedule plans"}-->
# Schedule plans

In order for a background job to be scheduled by the platform, there should be a registered schedule plan which manages the process. All schedule plans of a plugin should be check for existence and registered during your plugin `Initialize` method in the `ErpPlugin` inherited method. Once registered the schedule plan will be executed by the system, even if removed from the plugin initialization.

## Properties

The schedule plan is implemented by the `SchedulePlan` object.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Enabled`                     | *object type*: `bool`                         
|                               |         
|                               | Whether this plan is enabled.
+-------------------------------+-----------------------------------+
| `EndDate`                     | *object type*: `DateTime?`                         
|                               |         
|                               | After which datetime the job scheduling must be canceled
+-------------------------------+-----------------------------------+
| `EndTimespan`                 | *object type*: `int?`                         
|                               |         
|                               | Represents the hour minute format as "HHmm" integer. Sets the end hour of the day when job scheduling is allowed
+-------------------------------+-----------------------------------+
| `Id`                          | *object type*: `Guid`                         
|                               |         
|                               | Id of the schedule plan
+-------------------------------+-----------------------------------+
| `IntervalInMinutes`           | *object type*: `int?`                         
|                               |         
|                               | Used in Interval type. Sets the length of the interval period in minutes.
+-------------------------------+-----------------------------------+
| `JobTypeId`                   | *object type*: `Guid`                         
|                               |         
|                               | Unique Id of the job type as decorated in the `Job` attribute
+-------------------------------+-----------------------------------+
| `LastTriggerTime`             | *object type*: `DateTime?`                         
|                               |         
|                               | When this job type was last triggered.
+-------------------------------+-----------------------------------+
| `LastStartedJobId`            | *object type*: `Guid?`                         
|                               |         
|                               | The id of the last scheduled background job
+-------------------------------+-----------------------------------+
| `Name`                        | *object type*: `string`                         
|                               |         
|                               | Human readable name of the schedule plan, presented in the plans' list.
+-------------------------------+-----------------------------------+
| `NextTriggerTime`             | *object type*: `DateTime?`                         
|                               |         
|                               | When this job type will be next triggered. If null - it will no longer be triggered. If you manually trigger a schedule plan, this property will be set at the start of the next minute.
+-------------------------------+-----------------------------------+
| `ScheduledDays`               | *object type*: `SchedulePlanDaysOfWeek`                         
|                               |         
|                               | List of booleans for each week day. Sets in which days of the week jobs should be scheduled
+-------------------------------+-----------------------------------+
| `StartDate`                   | *object type*: `DateTime?`                         
|                               |         
|                               | Start date and time of the plan. When the first job should be scheduled if this datetime matches the rest specific type conditions. If not, the first possible datetime after this will be selected.
+-------------------------------+-----------------------------------+
| `StartTimespan`               | *object type*: `int?`                         
|                               |         
|                               | Represents the hour minute format as "HHmm" integer. Sets the starting hour of the day when job scheduling is allowed
+-------------------------------+-----------------------------------+
| `Type`                        | *object type*: `SchedulePlanType`                         
|                               |         
|                               | Schedule type. Options are: Interval, Daily, Weekly, Monthly
+-------------------------------+-----------------------------------+

## Code example

You can register a schedule plan by executing the following code in your plugin's initialize method:

```csharp
using WebVella.Erp.Jobs


DateTime utcNow = DateTime.UtcNow;
Guid checkBotSchedulePlanId = new Guid("AC3D460F-77BD-44B6-A7C5-B52A37A0C846");
string planName = "Sample Job Plan";
SchedulePlan checkBotSchedulePlan = ScheduleManager.Current.GetSchedulePlan(checkBotSchedulePlanId);

if (checkBotSchedulePlan == null)
{
	checkBotSchedulePlan = new SchedulePlan();
	checkBotSchedulePlan.Id = checkBotSchedulePlanId;
	checkBotSchedulePlan.Name = planName;
	checkBotSchedulePlan.Type = SchedulePlanType.Daily;
	checkBotSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 2, DateTimeKind.Utc);
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
	checkBotSchedulePlan.IntervalInMinutes = 1440;
	checkBotSchedulePlan.StartTimespan = 0;
	checkBotSchedulePlan.EndTimespan = 1440;
	checkBotSchedulePlan.JobTypeId = new Guid("559c557a-0fd3-4235-b061-117197154ca5");
	checkBotSchedulePlan.JobAttributes = null;
	checkBotSchedulePlan.Enabled = true;
	checkBotSchedulePlan.LastModifiedBy = null;

	ScheduleManager.Current.CreateSchedulePlan(checkBotSchedulePlan);
}

```
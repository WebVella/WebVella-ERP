<!--{"sort_order":2, "name": "background-job", "label": "Background job"}-->
# Background job

The background job type is defined by the `ErpJob` object. This object works with close relation with the `SchedulePlan` object and when scheduled it creates a number of background jobs which execute the same method, as assigned in the type.

## Properties

The background job is implemented by the `ErpJob` object. It has only one method `Execute`.

## Create a new background job

To create your own background job you need to apply the following requirements:

##### Requirement 1: Create a class that inherits the `ErpJob` class

```csharp
public class SampleJob : ErpJob
```

##### Requirement 2: Decorate your class with the `Job` attribute

```csharp
[Job("559c557a-0fd3-4235-b061-117197154ca5", "Sample job", true, JobPriority.Medium)]
```

This attribute has the following properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `allowSingleInstance`	        | *object type*: `bool`                         
|                               |         
|                               | If set to TRUE, before scheduling a job, the platform will check if there is another job from the same type running. If there is, it will not schedule a the new job.
+-------------------------------+-----------------------------------+
| `defaultPriority`             | *object type*: `JobPriority`                         
|                               |         
|                               | Manages the order in which scheduled jobs will be executed. Needed when there are more scheduled jobs then the preset 2 threads. Options are: Low, Medium, High, Higher, Highest
+-------------------------------+-----------------------------------+
| `Id`                          | *object type*: `Guid`                         
|                               |         
|                               | Unique identification of the job
+-------------------------------+-----------------------------------+
| `Name`                        | *object type*: `string`                         
|                               |         
|                               | Human readable name of the job, presented in the jobs' list.
+-------------------------------+-----------------------------------+

##### Requirement 3: Implement the `Execute` method of the inherited class "ErpJob"

``` csharp
public override void Execute(JobContext context)
```

## Full Example

``` csharp
using System.Threading;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Jobs;

namespace WebVella.Erp.Plugins.SDK.Jobs
{
	[Job("559c557a-0fd3-4235-b061-117197154ca5", "Sample job", true, JobPriority.Medium)]
	public class SampleJob : ErpJob
	{
		public override void Execute(JobContext context)
		{
			var log = new Log();
			log.Create(LogType.Info, "Sample job","Execute Sample Job started", "");
			Thread.Sleep(5000);
			log.Create(LogType.Info, "Sample job", "Execute Sample Job completed.", "");
		}
	}
}
```


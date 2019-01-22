<!--{"sort_order":1, "name": "overview", "label": "Overview"}-->
# Overview

If it is needed to regularly execute a method in order to implement a functionality, the background jobs is the way to go. To do that you need to create a schedule plan and subscribe a method from you code, that needs to be executed. The platform has a manager, that checks each minute for jobs that needs to be started. All such jobs are logged and you are able to review when they were scheduled and what was the result of the execution.

All such jobs are executed in the background in a separate threads, which limits the impact on the system's performance.

The functionality is provided by methods decorated with special attributes and the `ScheduleManager` service.

Definitions of the used terms are:

* background job type - the background job definition method that should be executed 
* background job - the instance of the background job type that was or will be run in a specific time.
* schedule plan - the scheduling plan of the background job type, upon which will be decided when to schedule a background job type to be executed.
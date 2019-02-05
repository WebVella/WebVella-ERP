$(function(){

	function RunTimer(wvTimerEl) {
		var recordRow = $(wvTimerEl).closest("tr");
		recordRow.addClass("go-bkg-orange-light");
		var timerTd = $(wvTimerEl).closest('.timer-td');
		var alreadyLoggedSecondsEl = timerTd.find("input[name='timelog_total_seconds']");
		var alreadyLoggedSeconds = 0;
		if (alreadyLoggedSecondsEl && alreadyLoggedSecondsEl.val()) {
			alreadyLoggedSeconds = alreadyLoggedSecondsEl.val();
		}
		var logStartFormInputEl = timerTd.find("input[name='timelog_started_on']");
		var logstartDate = $(logStartFormInputEl).val();
		var totalLoggedSeconds = moment().utc().diff(moment(logstartDate), 'seconds');
		var totalLoggedSecondsDec = new Decimal(totalLoggedSeconds).add(new Decimal(alreadyLoggedSeconds));		
		var loggedHours = totalLoggedSecondsDec.div(3600).toDecimalPlaces(0,Decimal.ROUND_DOWN);
		var totalLeft = totalLoggedSecondsDec.minus(loggedHours.times(3600));
		var loggedMinutes = totalLeft.div(60).toDecimalPlaces(0,Decimal.ROUND_DOWN);
		totalLeft = totalLeft.minus(loggedMinutes.times(60));
		var loggedSeconds = totalLeft;
		var loggedHoursString = loggedHours.toString();
		if (loggedHours.lessThan(10)) {
			loggedHoursString = "0"+loggedHoursString;
		}
		var loggedMinutesString = loggedMinutes.toString();
		if (loggedMinutes.lessThan(10)) {
			loggedMinutesString = "0"+loggedMinutesString;
		}
		var loggedSecondsString = loggedSeconds.toString();
		if (loggedSeconds.lessThan(10)) {
			loggedSecondsString = "0"+loggedSecondsString;
		}
		recordRow.find(".timer-td span").html(loggedHoursString + ' : ' + loggedMinutesString + ' : ' + loggedSecondsString);
		recordRow.find(".timer-td span").addClass("go-orange").removeClass("go-gray");
	}

	function EvaluateTimer(wvTimerEl) {
		var recordRow = $(wvTimerEl).closest("tr");
		var inputLogStartedOn = recordRow.find("input[name='timelog_started_on']");
		if (inputLogStartedOn.val()) {
			recordRow.find(".start-log-group").addClass("d-none");
			recordRow.find(".stop-log-group").removeClass("d-none");
			RunTimer(wvTimerEl);
			setInterval(function () {
				RunTimer(wvTimerEl);
			}, 1000);
		}
		else {
			recordRow.find(".start-log-group").removeClass("d-none");
			recordRow.find(".stop-log-group").addClass("d-none");
		}

	}


    $(".wv-timer").each(function(){
		EvaluateTimer(this);
    });
    
    $(".start-log").click(function(){
        var clickedBtn = $(this);
		var recordRow = clickedBtn.closest("tr");
		var recordTimer = recordRow.find(".wv-timer");

        var clickedBtnIcon = clickedBtn.find(".fa");
        var clickedBtnTd = clickedBtn.closest("td");
        var hiddenTaskInput = clickedBtnTd.find("input[name='task_id']");
        var startLogGroup = clickedBtnTd.find(".start-log-group");
        var stopLogGroup = clickedBtnTd.find(".stop-log-group");
        var taskId = hiddenTaskInput.val();
        
        clickedBtn.prop('disabled', true);
        clickedBtnIcon.removeClass("fa-play").addClass("fa-spin fa-spinner");
        
		$.ajax({
		type: "POST",
		url: "/api/v3.0/p/project/timelog/start?taskId="+taskId,
		dataType:"json",
		success: function(response){
			if(!response.success){
				toastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });
				clickedBtn.prop('disabled', false);
				clickedBtnIcon.addClass("fa-play").removeClass("fa-spin fa-spinner");
			}
			else{
				startLogGroup.addClass("d-none");
				stopLogGroup.removeClass("d-none");
				clickedBtn.prop('disabled', false);
				clickedBtnIcon.addClass("fa-play").removeClass("fa-spin fa-spinner");
				recordRow.find("input[name='timelog_started_on']").val(moment().toISOString());
				EvaluateTimer(recordTimer);
			}
        
		},
		error:function(XMLHttpRequest, textStatus, errorThrown) {
			toastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });
		}
		});        

    });

	$(".stop-log").click(function(){
        var clickedBtn = $(this);
		var recordRow = clickedBtn.closest("tr");		
		var inputTimelogStartEl = recordRow.find("input[name='timelog_started_on']");
		var inputTaskId =  recordRow.find("input[name='task_id']");
		var inputBillableStatus =  recordRow.find("input[name='is_billable']");
		var billableStatusVal = inputBillableStatus.val();
		var defaultBillableStatus = false;
		if (billableStatusVal && billableStatusVal.toLowerCase() === "true") {
			defaultBillableStatus = true;
		}
		var modalId = "wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4";
		var formEl = $("#"+modalId);
		var minutesFormInputEl = formEl.find("input[name='minutes']");
		var taskIdFormInputEl = formEl.find("input[name='task_id']");
		var billableStatusFakeCheckboxEl = formEl.find("input[data-field-name='is_billable']");
		billableStatusFakeCheckboxEl.prop('checked', defaultBillableStatus).trigger("change");

		var logStartFormInputEl = formEl.find("input[name='timelog_started_on']");
		//set minutes
		var logstartDate = $(inputTimelogStartEl).val();
		var totalLoggedSeconds = moment().utc().diff(logstartDate, 'seconds');
		var totalLoggedSecondsDec = new Decimal(totalLoggedSeconds);	
		var totalMinutes = totalLoggedSecondsDec.div(60).toDecimalPlaces(0,Decimal.ROUND_UP);
		minutesFormInputEl.val(totalMinutes.toNumber());
		//set taskId
		taskIdFormInputEl.val(inputTaskId.val());
		logStartFormInputEl.val(moment(logstartDate).format("DD MMM YYYY HH:mm"));
		logStartFormInputEl.prop('disabled', true);
		//set logstart date
		ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:modalId,action:'open',payload:null});
	});
	$(".manual-log").click(function(){
        var clickedBtn = $(this);
		var recordRow = clickedBtn.closest("tr");		
		var inputTaskId =  recordRow.find("input[name='task_id']");
		var inputBillableStatus =  recordRow.find("input[name='is_billable']");
		var billableStatusVal = inputBillableStatus.val();
		var defaultBillableStatus = false;
		if (billableStatusVal && billableStatusVal.toLowerCase() === "true") {
			defaultBillableStatus = true;
		}
		var formId = "wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4";
		var formEl = $("#"+formId);
		var taskIdFormInputEl = formEl.find("input[name='task_id']");
		var billableStatusFakeCheckboxEl = formEl.find("input[data-field-name='is_billable']");
		billableStatusFakeCheckboxEl.prop('checked', defaultBillableStatus).trigger("change");
		//set taskId
		taskIdFormInputEl.val(inputTaskId.val());
		ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:formId,action:'open',payload:null});		
	});

	$(".set-completed").click(function(){
		if (window.confirm("Confirm task change to completed status")) {
			var clickedBtn = $(this);
			var clickedBtnIcon = clickedBtn.find(".fa");
			clickedBtn.prop('disabled', false);
			clickedBtnIcon.removeClass("fa-check").addClass("fa-spin fa-spinner");
			var recordRow = clickedBtn.closest("tr");		
			var inputTaskId =  recordRow.find("input[name='task_id']");
			var taskId = inputTaskId.val();

			$.ajax({
			type: "POST",
			url: "/api/v3.0/p/project/task/status?statusId=b1cc69e5-ce09-40e0-8785-b6452b257bdf&taskId="+taskId,
			dataType:"json",
			success: function(response){
				if(!response.success){
					toastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });
					clickedBtn.prop('disabled', false);
					clickedBtnIcon.addClass("fa-check").removeClass("fa-spin fa-spinner");
				}
				else{
					clickedBtn.removeClass("btn-white").addClass("btn-success").html("<i class='fa fa-check'></i> completed");
				}
        
			},
			error:function(XMLHttpRequest, textStatus, errorThrown) {
				toastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });
			}
			});   
		}
	});
});
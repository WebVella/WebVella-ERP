function StartTaskWatch(taskId) {
	$.ajax({
		type: "POST",
		url: "/api/v3.0/p/project/task/watch?taskId=" + taskId + "&startWatch=true",
		dataType: "json",
		success: function (response) {
			if (!response.success) {
				toastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });
			}
			else {
				location.reload();
			}

		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			toastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });
		}
	});
}

function StopTaskWatch(taskId) {
	$.ajax({
		type: "POST",
		url: "/api/v3.0/p/project/task/watch?taskId=" + taskId + "&startWatch=false",
		dataType: "json",
		success: function (response) {
			if (!response.success) {
				toastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });
			}
			else {
				location.reload();
			}

		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			toastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });
		}
	});
}
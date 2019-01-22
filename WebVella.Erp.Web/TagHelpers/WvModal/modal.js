//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:null,action:'open',payload:null})
//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:null,action:'close',payload:null})

function OpenModal(modalId) {
	if (modalId) {
		$("#" + modalId).modal("show");
	}
	else {
		$(".modal").modal("show");
	}
}

function CloseModal(modalId) {
	if (modalId) {
		$("#" + modalId).modal("hide");
	}
	else {
		$(".modal").modal("hide");
	}
}
function WebVellaErpWebComponentsPcModal_Init(elementId) {

	ErpEvent.ON('WebVella.Erp.Web.Components.PcModal', function (event) {
		var eventData = event.detail;
		var action = null;
		var htmlId = null;
		var payload = null;
		if (eventData && typeof eventData === 'string') {
			action = eventData;
		}
		else if (eventData && typeof eventData === 'object') {
			action = eventData.action;
			htmlId = eventData.htmlId;
			payload = eventData.payload;
		}
		switch (action) {
			case 'open':
			case 'show':
				if (htmlId === elementId) {
					OpenModal(htmlId);
				}
				else if (!htmlId) {
					OpenModal(null);
				}
				break;
			case 'close':
			case 'hide':
				if (htmlId === elementId) {
					CloseModal(htmlId);
				}
				else if (!htmlId) {
					CloseModal(null);
				}
				break;
			default:
				break;
		}
	});
}


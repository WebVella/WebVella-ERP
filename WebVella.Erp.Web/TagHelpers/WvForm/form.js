//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcForm',{htmlId:null,action:'submit',payload:null})

function WebVellaErpWebComponentsPcForm_Init(elementId, formName) {

	ErpEvent.ON('WebVella.Erp.Web.Components.PcForm',function(event){
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
			case 'submit':
				if (htmlId === elementId) {
					$("#"+ htmlId + "[name='" + elementId + "']").submit();
				}
				else if (!htmlId) {
					$("form[name='" + formName + "']").submit();
				}
				break;
			default:
				break;
		}
	});
}


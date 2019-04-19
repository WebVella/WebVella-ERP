//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer',{htmlId:null,action:'open',payload:null})
//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer',{htmlId:null,action:'close',payload:null})

function OpenDrawer(drawerId) {

	$("#body-inner-wrapper-2 > .container-fluid").css("position", "relative");
	$("#body-inner-wrapper-2").css("position", "relative");
	if (drawerId) {
		$("body").append("<div class='drawer-backdrop' data-drawer-id='" + drawerId + "'></div>");
	}
	else {
		$("body").append("<div class='drawer-backdrop' data-drawer-id='none'></div>");
	}
	if (drawerId) {
		$("#" + drawerId).addClass("d-block");
	}
	else {
		$(".drawer").addClass("d-block");
	}
}

function CloseDrawer(drawerId) {
	$("#body-inner-wrapper-2 > .container-fluid").css("position", "unset");
	$("#body-inner-wrapper-2").css("position", "unset");
	$("body .drawer-backdrop").remove();
	if (drawerId && drawerId !== "none") {
		$("#" + drawerId).removeClass("d-block");
	}
	else {
		$(".drawer").removeClass("d-block");
	}
}

function WebVellaErpWebComponentsPcDrawer_Init(elementId){

	ErpEvent.ON('WebVella.Erp.Web.Components.PcDrawer',function(event){
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
					OpenDrawer(htmlId);
				}
				else if (!htmlId) {
					OpenDrawer(null);
				}
				break;
			case 'close':
			case 'hide':
				if (htmlId === elementId) {
					CloseDrawer(htmlId);
				}
				else if (!htmlId) {
					CloseDrawer(null);
				}
				break;
			default:
				break;
		}
	});
}

$(function () {

	$("#body-inner-wrapper-2").on("click", ".drawer-backdrop", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var drawerId = $(this).attr("data-drawer-id");
		CloseDrawer(drawerId);
	});

	$(".drawer-close").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var drawerId = $(this).attr("data-drawer-id");
		CloseDrawer(drawerId);
	});
})
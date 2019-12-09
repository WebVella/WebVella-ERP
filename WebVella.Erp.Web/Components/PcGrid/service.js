"use strict";

	/// Your code goes below
	///////////////////////////////////////////////////////////////////////////////////

	function ColumnCountChange(e) {
		var oldValue = e.target.getAttribute("data-old-value");
		var value = e.target.value;
		if (value.toString() !== oldValue.toString()) {
			e.target.setAttribute("data-old-value",value);
			$('#modal-component-options div[id^="section-column"]').addClass("d-none");
			for (var i = 1; i <= value; i++) {
				$('#modal-component-options #section-column'+ i).removeClass("d-none");
			}	
		}
	}


	//	document.addEventListener("WvPbManager_Design_Loaded", function (event) {
	//		if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcGrid"){
	//			console.log("WebVella.Erp.Web.Components.PcRecordList Design loaded");
	//		}
	//	});

	//	document.addEventListener("WvPbManager_Design_Unloaded", function (event) {
	//		if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcGrid"){
	//			console.log("WebVella.Erp.Web.Components.PcRecordList Design unloaded");
	//		}
	//	});



		document.addEventListener("WvPbManager_Options_Loaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcGrid"){
				window.setTimeout(function () {
					var visibleColumnsCount = document.querySelector('#modal-component-options .modal-body input[name="visible_columns"]');
					visibleColumnsCount.setAttribute("data-old-value",visibleColumnsCount.value);
					visibleColumnsCount.addEventListener("blur", ColumnCountChange);
				},500);
			}
		});

		document.addEventListener("WvPbManager_Options_Unloaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcGrid"){
			console.log("WebVella.Erp.Web.Components.PcGrid UnLoad");
				var visibleColumnsCount = document.querySelector('#modal-component-options .modal-body input[name="visible_columns"]');
				visibleColumnsCount.removeEventListener("blur", ColumnCountChange);
			}
		});


	//////////////////////////////////////////////////////////////////////////////////
	/// You code is above
	
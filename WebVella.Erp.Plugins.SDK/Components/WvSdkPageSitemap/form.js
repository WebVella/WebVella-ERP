$(function () {

	var WvPageSelectionTree = "{{PageSelectionTreeJson}}";

	function WvAdminSitemap() {
		var WvAdminSitemapSelectors = {};
		WvAdminSitemapSelectors.typeSelect = "#input-{{typeOptionsFieldId}}";
		WvAdminSitemapSelectors.appSelect = "#input-{{appOptionsFieldId}}";
		WvAdminSitemapSelectors.areaSelect = "#input-{{areaOptionsFieldId}}";
		WvAdminSitemapSelectors.nodeSelect = "#input-{{nodeOptionsFieldId}}";
		WvAdminSitemapSelectors.entitySelect = "#input-{{entityOptionsFieldId}}";
		WvAdminSitemapSelectors.detachBtn = "#detach-btn";
		WvAdminSitemapSelectors.detachBtnMessage = "#detach-btn-message";
		return WvAdminSitemapSelectors;
	}

	var WvAdminSitemapSelectors = WvAdminSitemap();

	var selectInitObject = {
		theme: 'bootstrap4',
		language: "en",
		minimumResultsForSearch: 10,
		closeOnSelect: true,
		width: 'element'
	};

	function WvAdminSitemapGetCurrentApp() {
		var selectedAppOptions = $(WvAdminSitemapSelectors["appSelect"]).select2('data');
		if (selectedAppOptions && selectedAppOptions.length > 0) {
			var currentAppIndex = _.findIndex(WvPageSelectionTree["app_selection_tree"], function (record) { return record.app_id === selectedAppOptions[0].id });
			if (currentAppIndex > -1) {
				return WvPageSelectionTree["app_selection_tree"][currentAppIndex];
			}
		}
		return null;
	}

	function WvAdminSitemapGetCurrentArea() {
		var currentApp = WvAdminSitemapGetCurrentApp();
		if (currentApp !== null) {
			var selectedAreaOptions = $(WvAdminSitemapSelectors["areaSelect"]).select2('data');
			if (selectedAreaOptions && selectedAreaOptions.length > 0) {
				var currentAreaIndex = _.findIndex(currentApp["area_selection_tree"], function (record) { return record.area_id === selectedAreaOptions[0].id });
				if (currentAreaIndex > -1) {
					return currentApp["area_selection_tree"][currentAreaIndex];
				}
			}
		}
		return null;
	}

	function WvAdminSitemapHideSelect(propertyName) {
		var WvAdminSitemapSelectors = WvAdminSitemap();
		var selector = WvAdminSitemapSelectors[propertyName];
		var $fieldEl = $(selector).closest(".wv-field");
		var currentlyVisible = !$($fieldEl).hasClass("d-none");
		if (currentlyVisible) {
			$(selector).children('option').remove();
			//$(selector).select2(selectInitObject).val(null).trigger("change");
			$(selector).val(null).trigger("change");
			$($fieldEl).addClass("d-none");
		}
	}

	function WvAdminSitemapShowSelect(propertyName, resetValue, hasEmptyOption) {
		var WvAdminSitemapSelectors = WvAdminSitemap();
		var selector = WvAdminSitemapSelectors[propertyName];
		var $fieldEl = $(selector).closest(".wv-field");
		var currentlyVisible = !$($fieldEl).hasClass("d-none");
		var currentlyHasEmptyOption = $($fieldEl).find("option[value='']").length > 0;

		//Remove empty option
		if (!hasEmptyOption && currentlyHasEmptyOption) {
			$(selector).find("option[value='']").remove().trigger("change");
		}
		//Return if no other changes needed. Adding or removing empty option is a change
		if (currentlyVisible && !resetValue) {
			//Just prepend empty option if needed
			if (hasEmptyOption && !currentlyHasEmptyOption) {
				$(selector).prepend('<option value="">not selected</option>').trigger("change");
			}
			return;
		}
		//Reset
		$(selector).children('option').remove();
		//$(selector).select2(selectInitObject).val(null).trigger("change");
		$(selector).val(null).trigger("change");

		//Add empty option
		if (hasEmptyOption && !currentlyHasEmptyOption) {
			$(selector).append('<option value="">not selected</option>').trigger("change");
		}

		//Preselect first
		var options = [];
		switch (propertyName) {
			case "appSelect":
				{
					_.forEach(WvPageSelectionTree["all_apps"], function (option) {
						$(selector).append('<option value="' + option["value"] + '">' + option["label"] + '</option>');
					});
				}
				break;
			case "areaSelect":
				{
					if(hasEmptyOption || !currentApp) {
						//if there is not already an empty option add it
						var areaHasEmptyOption = $($fieldEl).find("option[value='']").length > 0;
						if (!areaHasEmptyOption) {
							$(selector).append('<option value="">not selected</option>').trigger("change");
						}
					}
					var currentApp = WvAdminSitemapGetCurrentApp();
					if (currentApp) {
						_.forEach(currentApp["all_areas"], function (option) {
							$(selector).append('<option value="' + option["value"] + '">' + option["label"] + '</option>');
						});
					}
				}
				break;
			case "nodeSelect":
				{
					var currentArea = WvAdminSitemapGetCurrentArea();
					if (currentArea) {
						_.forEach(currentArea["all_nodes"], function (option) {
							$(selector).append('<option value="' + option["value"] + '">' + option["label"] + '</option>');
						});
					}
					else {
						//if there is not already an empty option add it
						var nodeHasEmptyOption = $($fieldEl).find("option[value='']").length > 0;
						if (!nodeHasEmptyOption) {
							$(selector).append('<option value="">not selected</option>').trigger("change");
						}
					}
				}
				break;
			case "entitySelect":
				{
					var currentEntityApp = WvAdminSitemapGetCurrentApp();
					var appEntities = [];
					if (currentEntityApp !== null) {
						appEntities = currentEntityApp["entities"];
					}

					if (appEntities.length === 0) {
						appEntities = WvPageSelectionTree["all_entities"];
					}

					_.forEach(appEntities, function (option) {
						$(selector).append('<option value="' + option["value"] + '">' + option["label"] + '</option>');
					});

				}
				break;
		}

		if ($(selector).children('option').length > 0) {
			var firstOptionVal = $(selector).find("option:first").val();
			//$(selector).select2(selectInitObject).val(firstOptionVal).trigger("change");
			$(selector).val(firstOptionVal).trigger("change");
		}
		else {
			//$(selector).select2(selectInitObject).trigger("change");
			$(selector).trigger("change");
		}

		$($fieldEl).removeClass("d-none");

	}


	$(WvAdminSitemapSelectors.typeSelect).on('select2:select', function (e) {
		var data = e.params.data;
		switch (data.id) {
			case "0":
			case "1":
				//App should be hidden
				WvAdminSitemapHideSelect("appSelect");
				//Area should be hidden
				WvAdminSitemapHideSelect("areaSelect");
				//Node should be hidden
				WvAdminSitemapHideSelect("nodeSelect");
				//Entity should be hidden
				WvAdminSitemapHideSelect("entitySelect");
				break;
			case "2":
				//App should be visible and value should not be preset
				WvAdminSitemapShowSelect("appSelect", false, false);
				//Area should be hidden
				WvAdminSitemapShowSelect("areaSelect",true,true);
				//Node should be hidden
				WvAdminSitemapShowSelect("nodeSelect",true,true);
				//Entity should be hidden
				WvAdminSitemapHideSelect("entitySelect");
				break;
			case "3":
			case "4":
			case "5":
			case "6":
				//Entity is required but APP, area and node are not. So we need to add an emtpy options

				//App should be visible and value should not be preset
				WvAdminSitemapShowSelect("appSelect", false, true);
				//App should be visible and value should not be preset
				WvAdminSitemapShowSelect("areaSelect", false, true);
				//App should be visible and value should not be preset
				WvAdminSitemapShowSelect("nodeSelect", false, true);
				//App should be visible and value should not be preset
				WvAdminSitemapShowSelect("entitySelect", false, false);
				break;
			default:
				break;
		}
	});

	$(WvAdminSitemapSelectors.appSelect).on('select2:select', function (e) {
		var data = e.params.data;
		var pageType = "1";
		var selectedTypeOptions = $(WvAdminSitemapSelectors["typeSelect"]).select2('data');
		if (selectedTypeOptions && selectedTypeOptions.length > 0) {
			pageType = selectedTypeOptions[0].id;
		}
		switch (pageType) {
			case "0":
			case "1":
				//Should not be possible
				console.error("cannot change application while page type is site");
				break;
			case "2":
				//Do nothing as the rest should be hidden
				//Show area with value reset
				WvAdminSitemapShowSelect("areaSelect", true,true);
				//Show node with value reset
				WvAdminSitemapShowSelect("nodeSelect", true, true);
				break;
			case "3":
			case "4":
			case "5":
			case "6":
				//Area and App and Node could have no values
				//Show area with value reset
				WvAdminSitemapShowSelect("areaSelect", true,true);
				//Show node with value reset
				WvAdminSitemapShowSelect("nodeSelect", true,true);
				break;
			default:
				break;
		}
	});

	$(WvAdminSitemapSelectors.areaSelect).on('select2:select', function (e) {
		var data = e.params.data;
		switch (data.id) {
			case "0":
			case "1":
				//Should not be possible
				console.error("cannot change area while page type is site");
				break;
			case "2":
				//Should not be possible
				console.error("cannot change area while page type is application");
				break;
			default:
				//Show node with value reset
				WvAdminSitemapShowSelect("nodeSelect", true, true);
				//Show node without value reset
				//WvAdminSitemapShowSelect("entitySelect", false);
				break;
		}
	});

	$(WvAdminSitemapSelectors.nodeSelect).on('select2:select', function (e) {
		//Do nothing
	});

	$(WvAdminSitemapSelectors.entitySelect).on('select2:select', function (e) {
		//Do nothing
	});

	$(WvAdminSitemapSelectors.detachBtn).click(function () {
		$("#area-hidden-input").val(null);
		$("#node-hidden-input").val(null);
		$(WvAdminSitemapSelectors.areaSelect).html("not selected");
		$(WvAdminSitemapSelectors.nodeSelect).html("not selected");
		$(WvAdminSitemapSelectors.detachBtn).addClass("d-none");
		$(WvAdminSitemapSelectors.detachBtnMessage).removeClass("d-none");
		$(WvAdminSitemapSelectors.detachBtn).closest(".form-control-plaintext").css("margin-top","21px");
		
	});
});
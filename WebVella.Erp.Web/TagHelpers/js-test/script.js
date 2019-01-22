$(function () {
	var sitemap = "{{SitemapJson}}";

	function initElements() {
		var initedObj = {};
		initedObj.navWrapper = document.getElementById("nav");
		//Area
		initedObj.areaNavWrapper = document.getElementById("area-nav-wrapper");
		initedObj.sitemapEl = document.getElementById("sitemap");
		initedObj.areaNavIcon = document.getElementById("area-nav-icon");

		//Subarea
		initedObj.nodeNavWrapper = document.getElementById("node-nav-wrapper");
		initedObj.nodeNavIcon = document.getElementById("node-nav-icon");

		//Record
		initedObj.recordNavWrapper = document.getElementById("record-nav-wrapper");
		initedObj.recordNav = document.getElementById("record-nav");
		initedObj.recordNavIcon = document.getElementById("record-nav-icon");

		//Current nav state
		initedObj.currentOpenedMenu = initedObj.navWrapper.getAttribute("data-opened-menu");
		return initedObj;
	}

	function getClickedElementType(targetElement) {
		do {
			var currentElementId = targetElement.getAttribute("id");
			if (currentElementId === "area-nav-link") {
				return "area";
			}
			else if (currentElementId === "node-nav-link") {
				return "node";
			}
			else if (currentElementId === "record-nav-link") {
				return "record";
			}
			// Go up the DOM.
			targetElement = targetElement.parentNode;
		} while (targetElement);
		return null;
	}

	function closeAreaNav() {
		var initedObj = initElements();
		initedObj.areaNavWrapper.removeAttribute("class");
		initedObj.areaNavIcon.setAttribute("class", "ti-angle-right nav-caret");
		initedObj.sitemapEl.hide();
		initedObj.navWrapper.removeAttribute("data-opened-menu");
	};

	function openAreaNav() {
		var initedObj = initElements();
		initedObj.areaNavWrapper.setAttribute("class", "active");
		initedObj.areaNavIcon.setAttribute("class", "ti-close nav-caret");
		initedObj.sitemapEl.show();
		initedObj.navWrapper.setAttribute("data-opened-menu", "area");
	}

	function closeSubareaNav() {
		var initedObj = initElements();
		initedObj.nodeNavWrapper.removeAttribute("class");
		initedObj.nodeNavIcon.setAttribute("class", "ti-angle-right nav-caret");
		initedObj.navWrapper.removeAttribute("data-opened-menu");
	}

	function openSubareaNav() {
		var initedObj = initElements();
		initedObj.nodeNavWrapper.setAttribute("class", "active");
		initedObj.nodeNavIcon.setAttribute("class", "ti-close nav-caret");
		initedObj.navWrapper.setAttribute("data-opened-menu", "node");
	}

	function closeRecordNav() {
		var initedObj = initElements();
		if (initedObj.recordNav) {
			initedObj.recordNavWrapper.removeAttribute("class");
			initedObj.recordNavIcon.setAttribute("class", "d-none");
			initedObj.navWrapper.removeAttribute("data-opened-menu");
		}
	}
	function openRecordNav() {
		var initedObj = initElements();
		if (initedObj.recordNav) {
			initedObj.recordNavWrapper.setAttribute("class", "active");
			initedObj.recordNavIcon.setAttribute("class", "ti-close nav-caret");
			initedObj.navWrapper.setAttribute("data-opened-menu", "record");
		}
	}

	function navClickHandler(event) {
		event.preventDefault();
		event.stopPropagation();
		//Elements
		var initedObj = initElements();

		if (initedObj.currentOpenedMenu) {
			var clickedElementType = getClickedElementType(event.target);

			if (initedObj.currentOpenedMenu === "area" && clickedElementType === "area") {
				closeAreaNav();
			}
			else if (initedObj.currentOpenedMenu === "node" && clickedElementType === "node") {
				closeSubareaNav();
			}
			else if (initedObj.currentOpenedMenu === "record" && clickedElementType === "record") {
				closeRecordNav();
			}
			else if (initedObj.currentOpenedMenu !== "area" && clickedElementType === "area") {
				closeSubareaNav();
				closeRecordNav();
				openAreaNav();
			}
			else if (initedObj.currentOpenedMenu !== "node" && clickedElementType === "node") {
				closeAreaNav();
				closeRecordNav();
				openSubareaNav();
			}
			else if (initedObj.currentOpenedMenu !== "record" && clickedElementType === "record") {
				closeAreaNav();
				closeSubareaNav();
				openRecordNav();
			}
			else {
				closeAreaNav();
				closeSubareaNav();
				closeRecordNav();
			}
		}
		else {
			//Menu should be opened
			var clickedElementType2 = getClickedElementType(event.target);

			if (clickedElementType2 === "area") {
				openAreaNav();
			}
			else if (clickedElementType2 === "node") {
				openSubareaNav();
			}
			else if (clickedElementType2 === "record") {
				openRecordNav();
			}
		}
	}

	document.addEventListener("click", function(event){
		var initedObj = initElements();

		var targetElement = event.target;

		if (initedObj.currentOpenedMenu) {
			var safezoneEl = null;

			if (initedObj.currentOpenedMenu === "area") {
				safezoneEl = document.getElementById("sitemap");
			}
			else if (initedObj.currentOpenedMenu === "node") {
				safezoneEl = document.getElementById("node-nav-dropdown");
			}
			else if (initedObj.currentOpenedMenu === "record") {
				safezoneEl = document.getElementById("record-nav-dropdown");
			}
			do {
				if (targetElement === safezoneEl) {
					// Do nothing, just return.
					return;
				}
				// Go up the DOM.
				targetElement = targetElement.parentNode;
			} while (targetElement);
		}

		if (initedObj.currentOpenedMenu === "area") {
			closeAreaNav();
		}
		else if (initedObj.currentOpenedMenu === "node") {
			closeSubareaNav();
		}
		else if (initedObj.currentOpenedMenu === "record") {
			closeRecordNav();
		}

	});
});
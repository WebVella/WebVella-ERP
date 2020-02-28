$(function () {
	function initElements() {
		var initedObj = {};
		initedObj.navWrapper = document.getElementById("nav");
		initedObj.currentOpenedMenu = initedObj.navWrapper.getAttribute("data-opened-menu");
		return initedObj;
	}

	$("[data-navclick-handler]").on("click", function (event) {
		event.preventDefault();
		event.stopPropagation();
		//Elements
		var initedObj = initElements();
		var clickedLink = event.target;
		var clickedLinkWrapper = $(clickedLink).closest(".menu-nav-wrapper");
		var clickedLinkDropdown = $(clickedLinkWrapper).find(".dropdown-menu.level-0");

		if ($(clickedLinkDropdown).hasClass( "d-block" )) {
			//menu should be closed
			$(".menu-nav-wrapper .dropdown-menu").removeClass("d-block");
			$(".menu-nav-wrapper").removeClass("active");
			initedObj.navWrapper.removeAttribute("data-opened-menu");
		}
		else {
			//Menu should be opened
			$(".menu-nav-wrapper .dropdown-menu").removeClass("d-block");
			$(".menu-nav-wrapper").removeClass("active");

			$(clickedLinkDropdown).addClass("d-block");
			$(clickedLinkWrapper).addClass("active");
			initedObj.navWrapper.setAttribute("data-opened-menu", "true");
		}
	});

	document.addEventListener("click", function (event) {

		var initedObj = initElements();

		var targetElement = event.target;

		if (initedObj.currentOpenedMenu) {
			do {
				if (targetElement && $(targetElement).hasClass("menu-nav-wrapper")) {
					return;
				}
				// Go up the DOM.
				targetElement = targetElement.parentNode;
			} while (targetElement);
		}
		$(".menu-nav-wrapper .dropdown-menu").removeClass("d-block");
		$(".menu-nav-wrapper").removeClass("active");
		initedObj.navWrapper.removeAttribute("data-opened-menu");
	});
});
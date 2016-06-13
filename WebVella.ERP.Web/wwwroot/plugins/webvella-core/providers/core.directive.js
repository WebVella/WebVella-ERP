
//#region << valid-date >>

/**
* @desc A custom validation rule for ngMessages
* @example <input valid-date/> and than in the validation messages you can work with the $validators object
*/

(function () {
	'use strict';

	angular
        .module('webvellaCore')
        .directive('validDate', directive);

	directive.$inject = [];


	function directive() {
		// Usage:
		//
		// Creates:
		//
		var directive = {
			restrict: 'A',
			require: "ngModel",
			link: link
		};
		return directive;

		function link(scope, element, attrs, ngModel) {
			ngModel.$validators.validDate = function (modelValue) {
				//moment().toISOString();
				if (modelValue == "" || modelValue == null) {
					return true;
				} else {
					if (moment(modelValue).isValid()) {
						return true;
					} else {
						return false;
					}
				}
			}
		}

	}

})();

//#endregion

//#region << focus-me >>

(function () {
	'use strict';

	angular
        .module('webvellaCore')
        .directive('focusMe', directive);

	directive.$inject = ['$timeout'];


	function directive($timeout) {
		// Usage:
		//
		// Creates:
		//
		var directive = {
			restrict: 'A',
			link: link
		};
		return directive;

		function link(scope, element, attrs) {
			scope.$watch(attrs.focusMe, function (value) {
				if (value === true) {
					$timeout(function () {
						element[0].focus();
					}, 0);
				}
			});
		}

	}

})();

//#endregion

//#region << confirm-click >>

/**
* @desc Requires confirmation before executing function
* @example <button confirmed-click="sayHi()" ng-confirm-click="Would you like to say hi?">Say hi to {{ name }}</button>
*/

(function () {
	'use strict';

	angular
        .module('webvellaCore')
        .directive('ngConfirmClick', directive);

	directive.$inject = [];


	function directive() {
		return {
			link: function (scope, element, attr) {
				var msg = attr.ngConfirmClick || "Are you sure?";
				var clickAction = attr.confirmedClick;
				element.bind('click', function (event) {
					if (window.confirm(msg)) {
						scope.$eval(clickAction)
					}
				});
			}
		};
	}

})();

//#endregion

//#region << Script lazy load in html dynamic views >>
//To load scripts in the dynamical html. Better use plugin
//To use include in app "ngLoadScript"


(function (ng) {
    'use strict';

    var app = ng.module('ngLoadScript', []);

    app.directive('script', function() {
        return {
            restrict: 'E',
            scope: false,
            link: function(scope, elem, attr) {
                if (attr.type=='text/javascript-lazy') {
                    var code = elem.text();
                    var f = new Function(code);
                    f();
                }
            }
        };
    });
}(angular));

//#endregion
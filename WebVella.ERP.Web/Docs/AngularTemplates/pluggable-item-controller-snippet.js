//@desc This will make an item pluggable and nicely initialized, by fixing the duplicated data problem
// A corresponding Factory should support the init, addElement and etc.

////1. CONSTRUCTOR - initialize the factory
elementFactory.initBrowsenav();
////2. READY hook listener
var readyElementDestructor = $rootScope.$on("webvella-some-even-ready", function (event, data) {
	//All actions you want to be done after the "Ready" hook is cast
})
////3. UPDATED hook listener
var updateElementDestructor = $rootScope.$on("webvella-some-even-updated", function (event, data) {
	//All actions you want to be done after the "Update" hook is cast
});
////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
$scope.$on("$destroy", function () {
	readyElementDestructor();
	updateElementDestructor();
});
////5. Bootstrap the pluggable element by casting the Ready hook
$timeout(function () {
	$rootScope.$emit("webvella-some-even-ready");
}, 0);
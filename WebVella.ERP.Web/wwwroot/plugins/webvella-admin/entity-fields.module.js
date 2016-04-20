/* entity-fields.module.js */

/**
* @desc this module manages the entity record fields in the admin screen
*/

(function () {
	//'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityFieldsController', controller)
        .controller('CreateFieldModalController', CreateFieldModalController)
        .controller('ManageFieldModalController', ManageFieldModalController)
        .controller('DeleteFieldModalController', DeleteFieldModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];
	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-fields', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/fields', //  /desktop/areas after the parent state is prepended
			views: {
				"topnavView": {
					controller: 'WebVellaAdminTopnavController',
					templateUrl: '/plugins/webvella-admin/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAdminSidebarController',
					templateUrl: '/plugins/webvella-admin/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityFieldsController',
					templateUrl: '/plugins/webvella-admin/entity-fields.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedRolesList: resolveRolesList,
				resolvedRelationsList: resolveRelationsList
			},
			data: {

			}
		});
	};


	//#region << Resolve Function /////////////////////////
	checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast'];
	
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
		var accessPermission = false;
		for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
			if (resolvedCurrentUser.roles[i] === "bdc56420-caf0-4030-8a0e-d264938e0cda") {
				accessPermission = true;
			}
		}

		if (accessPermission) {
			defer.resolve();
		}
		else {

			ngToast.create({
				className: 'error',
				content: messageContent,
				timeout: 7000
			});
			defer.reject("No access");
		}

		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		return defer.promise;
	}

	// Resolve Roles list /////////////////////////
	resolveRolesList.$inject = ['$q', '$log', 'webvellaCoreService'];
	
	function resolveRolesList($q, $log, webvellaCoreService) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRecordsByListName("null","role", "null", successCallback, errorCallback);

		return defer.promise;
	}

	resolveRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout'];
	
	function resolveRelationsList($q, $log, webvellaCoreService) {

		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);

		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedRolesList',
						'resolvedRelationsList', '$timeout', '$q', 'webvellaCoreService'];
	
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedRolesList,
						resolvedRelationsList, $timeout, $q, webvellaCoreService) {
		
		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.rolesList = fastCopy(resolvedRolesList);
		ngCtrl.entity.fields = ngCtrl.entity.fields.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		//#region << Update page title>> 
		ngCtrl.pageTitle = "Entity Fields | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		ngCtrl.showSidebar = function(){
		    //Show Sidemenu
			$timeout(function(){
				$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			},0);
		}	
		
		//#endregion


		ngCtrl.fieldTypes = getFieldTypes();

		//Currency meta - used in Create and Manage fields
		ngCtrl.currencyMetas =
		[
			{
				"symbol": "$",
				"name": "US Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "USD",
				"name_plural": "US dollars"
			},
			{
				"symbol": "CA$",
				"name": "Canadian Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "CAD",
				"name_plural": "Canadian dollars"
			},
			{
				"symbol": "€",
				"name": "Euro",
				"symbol_native": "€",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "EUR",
				"name_plural": "euros"
			},
			{
				"symbol": "AED",
				"name": "United Arab Emirates Dirham",
				"symbol_native": "د.إ.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "AED",
				"name_plural": "UAE dirhams"
			},
			{
				"symbol": "Af",
				"name": "Afghan Afghani",
				"symbol_native": "؋",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "AFN",
				"name_plural": "Afghan Afghanis"
			},
			{
				"symbol": "ALL",
				"name": "Albanian Lek",
				"symbol_native": "Lek",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "ALL",
				"name_plural": "Albanian lekë"
			},
			{
				"symbol": "AMD",
				"name": "Armenian Dram",
				"symbol_native": "դր.",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "AMD",
				"name_plural": "Armenian drams"
			},
			{
				"symbol": "AR$",
				"name": "Argentine Peso",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "ARS",
				"name_plural": "Argentine pesos"
			},
			{
				"symbol": "AU$",
				"name": "Australian Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "AUD",
				"name_plural": "Australian dollars"
			},
			{
				"symbol": "man.",
				"name": "Azerbaijani Manat",
				"symbol_native": "ман.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "AZN",
				"name_plural": "Azerbaijani manats"
			},
			{
				"symbol": "KM",
				"name": "Bosnia-Herzegovina Convertible Mark",
				"symbol_native": "KM",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BAM",
				"name_plural": "Bosnia-Herzegovina convertible marks"
			},
			{
				"symbol": "Tk",
				"name": "Bangladeshi Taka",
				"symbol_native": "৳",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BDT",
				"name_plural": "Bangladeshi takas"
			},
			{
				"symbol": "BGN",
				"name": "Bulgarian Lev",
				"symbol_native": "лв.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BGN",
				"name_plural": "Bulgarian leva"
			},
			{
				"symbol": "BD",
				"name": "Bahraini Dinar",
				"symbol_native": "د.ب.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "BHD",
				"name_plural": "Bahraini dinars"
			},
			{
				"symbol": "FBu",
				"name": "Burundian Franc",
				"symbol_native": "FBu",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "BIF",
				"name_plural": "Burundian francs"
			},
			{
				"symbol": "BN$",
				"name": "Brunei Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BND",
				"name_plural": "Brunei dollars"
			},
			{
				"symbol": "Bs",
				"name": "Bolivian Boliviano",
				"symbol_native": "Bs",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BOB",
				"name_plural": "Bolivian bolivianos"
			},
			{
				"symbol": "R$",
				"name": "Brazilian Real",
				"symbol_native": "R$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BRL",
				"name_plural": "Brazilian reals"
			},
			{
				"symbol": "BWP",
				"name": "Botswanan Pula",
				"symbol_native": "P",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BWP",
				"name_plural": "Botswanan pulas"
			},
			{
				"symbol": "BYR",
				"name": "Belarusian Ruble",
				"symbol_native": "BYR",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "BYR",
				"name_plural": "Belarusian rubles"
			},
			{
				"symbol": "BZ$",
				"name": "Belize Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "BZD",
				"name_plural": "Belize dollars"
			},
			{
				"symbol": "CDF",
				"name": "Congolese Franc",
				"symbol_native": "FrCD",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "CDF",
				"name_plural": "Congolese francs"
			},
			{
				"symbol": "CHF",
				"name": "Swiss Franc",
				"symbol_native": "CHF",
				"decimal_digits": 2,
				"rounding": 0.05,
				"code": "CHF",
				"name_plural": "Swiss francs"
			},
			{
				"symbol": "CL$",
				"name": "Chilean Peso",
				"symbol_native": "$",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "CLP",
				"name_plural": "Chilean pesos"
			},
			{
				"symbol": "CN¥",
				"name": "Chinese Yuan",
				"symbol_native": "CN¥",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "CNY",
				"name_plural": "Chinese yuan"
			},
			{
				"symbol": "CO$",
				"name": "Colombian Peso",
				"symbol_native": "$",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "COP",
				"name_plural": "Colombian pesos"
			},
			{
				"symbol": "₡",
				"name": "Costa Rican Colón",
				"symbol_native": "₡",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "CRC",
				"name_plural": "Costa Rican colóns"
			},
			{
				"symbol": "CV$",
				"name": "Cape Verdean Escudo",
				"symbol_native": "CV$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "CVE",
				"name_plural": "Cape Verdean escudos"
			},
			{
				"symbol": "Kč",
				"name": "Czech Republic Koruna",
				"symbol_native": "Kč",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "CZK",
				"name_plural": "Czech Republic korunas"
			},
			{
				"symbol": "Fdj",
				"name": "Djiboutian Franc",
				"symbol_native": "Fdj",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "DJF",
				"name_plural": "Djiboutian francs"
			},
			{
				"symbol": "Dkr",
				"name": "Danish Krone",
				"symbol_native": "kr",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "DKK",
				"name_plural": "Danish kroner"
			},
			{
				"symbol": "RD$",
				"name": "Dominican Peso",
				"symbol_native": "RD$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "DOP",
				"name_plural": "Dominican pesos"
			},
			{
				"symbol": "DA",
				"name": "Algerian Dinar",
				"symbol_native": "د.ج.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "DZD",
				"name_plural": "Algerian dinars"
			},
			{
				"symbol": "Ekr",
				"name": "Estonian Kroon",
				"symbol_native": "kr",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "EEK",
				"name_plural": "Estonian kroons"
			},
			{
				"symbol": "EGP",
				"name": "Egyptian Pound",
				"symbol_native": "ج.م.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "EGP",
				"name_plural": "Egyptian pounds"
			},
			{
				"symbol": "Nfk",
				"name": "Eritrean Nakfa",
				"symbol_native": "Nfk",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "ERN",
				"name_plural": "Eritrean nakfas"
			},
			{
				"symbol": "Br",
				"name": "Ethiopian Birr",
				"symbol_native": "Br",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "ETB",
				"name_plural": "Ethiopian birrs"
			},
			{
				"symbol": "£",
				"name": "British Pound Sterling",
				"symbol_native": "£",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "GBP",
				"name_plural": "British pounds sterling"
			},
			{
				"symbol": "GEL",
				"name": "Georgian Lari",
				"symbol_native": "GEL",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "GEL",
				"name_plural": "Georgian laris"
			},
			{
				"symbol": "GH₵",
				"name": "Ghanaian Cedi",
				"symbol_native": "GH₵",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "GHS",
				"name_plural": "Ghanaian cedis"
			},
			{
				"symbol": "FG",
				"name": "Guinean Franc",
				"symbol_native": "FG",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "GNF",
				"name_plural": "Guinean francs"
			},
			{
				"symbol": "GTQ",
				"name": "Guatemalan Quetzal",
				"symbol_native": "Q",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "GTQ",
				"name_plural": "Guatemalan quetzals"
			},
			{
				"symbol": "HK$",
				"name": "Hong Kong Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "HKD",
				"name_plural": "Hong Kong dollars"
			},
			{
				"symbol": "HNL",
				"name": "Honduran Lempira",
				"symbol_native": "L",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "HNL",
				"name_plural": "Honduran lempiras"
			},
			{
				"symbol": "kn",
				"name": "Croatian Kuna",
				"symbol_native": "kn",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "HRK",
				"name_plural": "Croatian kunas"
			},
			{
				"symbol": "Ft",
				"name": "Hungarian Forint",
				"symbol_native": "Ft",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "HUF",
				"name_plural": "Hungarian forints"
			},
			{
				"symbol": "Rp",
				"name": "Indonesian Rupiah",
				"symbol_native": "Rp",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "IDR",
				"name_plural": "Indonesian rupiahs"
			},
			{
				"symbol": "₪",
				"name": "Israeli New Sheqel",
				"symbol_native": "₪",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "ILS",
				"name_plural": "Israeli new sheqels"
			},
			{
				"symbol": "Rs",
				"name": "Indian Rupee",
				"symbol_native": "টকা",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "INR",
				"name_plural": "Indian rupees"
			},
			{
				"symbol": "IQD",
				"name": "Iraqi Dinar",
				"symbol_native": "د.ع.‏",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "IQD",
				"name_plural": "Iraqi dinars"
			},
			{
				"symbol": "IRR",
				"name": "Iranian Rial",
				"symbol_native": "﷼",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "IRR",
				"name_plural": "Iranian rials"
			},
			{
				"symbol": "Ikr",
				"name": "Icelandic Króna",
				"symbol_native": "kr",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "ISK",
				"name_plural": "Icelandic krónur"
			},
			{
				"symbol": "J$",
				"name": "Jamaican Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "JMD",
				"name_plural": "Jamaican dollars"
			},
			{
				"symbol": "JD",
				"name": "Jordanian Dinar",
				"symbol_native": "د.أ.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "JOD",
				"name_plural": "Jordanian dinars"
			},
			{
				"symbol": "¥",
				"name": "Japanese Yen",
				"symbol_native": "￥",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "JPY",
				"name_plural": "Japanese yen"
			},
			{
				"symbol": "Ksh",
				"name": "Kenyan Shilling",
				"symbol_native": "Ksh",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "KES",
				"name_plural": "Kenyan shillings"
			},
			{
				"symbol": "KHR",
				"name": "Cambodian Riel",
				"symbol_native": "៛",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "KHR",
				"name_plural": "Cambodian riels"
			},
			{
				"symbol": "CF",
				"name": "Comorian Franc",
				"symbol_native": "FC",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "KMF",
				"name_plural": "Comorian francs"
			},
			{
				"symbol": "₩",
				"name": "South Korean Won",
				"symbol_native": "₩",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "KRW",
				"name_plural": "South Korean won"
			},
			{
				"symbol": "KD",
				"name": "Kuwaiti Dinar",
				"symbol_native": "د.ك.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "KWD",
				"name_plural": "Kuwaiti dinars"
			},
			{
				"symbol": "KZT",
				"name": "Kazakhstani Tenge",
				"symbol_native": "тңг.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "KZT",
				"name_plural": "Kazakhstani tenges"
			},
			{
				"symbol": "LB£",
				"name": "Lebanese Pound",
				"symbol_native": "ل.ل.‏",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "LBP",
				"name_plural": "Lebanese pounds"
			},
			{
				"symbol": "SLRs",
				"name": "Sri Lankan Rupee",
				"symbol_native": "SL Re",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "LKR",
				"name_plural": "Sri Lankan rupees"
			},
			{
				"symbol": "Lt",
				"name": "Lithuanian Litas",
				"symbol_native": "Lt",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "LTL",
				"name_plural": "Lithuanian litai"
			},
			{
				"symbol": "Ls",
				"name": "Latvian Lats",
				"symbol_native": "Ls",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "LVL",
				"name_plural": "Latvian lati"
			},
			{
				"symbol": "LD",
				"name": "Libyan Dinar",
				"symbol_native": "د.ل.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "LYD",
				"name_plural": "Libyan dinars"
			},
			{
				"symbol": "MAD",
				"name": "Moroccan Dirham",
				"symbol_native": "د.م.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MAD",
				"name_plural": "Moroccan dirhams"
			},
			{
				"symbol": "MDL",
				"name": "Moldovan Leu",
				"symbol_native": "MDL",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MDL",
				"name_plural": "Moldovan lei"
			},
			{
				"symbol": "MGA",
				"name": "Malagasy Ariary",
				"symbol_native": "MGA",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "MGA",
				"name_plural": "Malagasy Ariaries"
			},
			{
				"symbol": "MKD",
				"name": "Macedonian Denar",
				"symbol_native": "MKD",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MKD",
				"name_plural": "Macedonian denari"
			},
			{
				"symbol": "MMK",
				"name": "Myanma Kyat",
				"symbol_native": "K",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "MMK",
				"name_plural": "Myanma kyats"
			},
			{
				"symbol": "MOP$",
				"name": "Macanese Pataca",
				"symbol_native": "MOP$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MOP",
				"name_plural": "Macanese patacas"
			},
			{
				"symbol": "MURs",
				"name": "Mauritian Rupee",
				"symbol_native": "MURs",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "MUR",
				"name_plural": "Mauritian rupees"
			},
			{
				"symbol": "MX$",
				"name": "Mexican Peso",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MXN",
				"name_plural": "Mexican pesos"
			},
			{
				"symbol": "RM",
				"name": "Malaysian Ringgit",
				"symbol_native": "RM",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MYR",
				"name_plural": "Malaysian ringgits"
			},
			{
				"symbol": "MTn",
				"name": "Mozambican Metical",
				"symbol_native": "MTn",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "MZN",
				"name_plural": "Mozambican meticals"
			},
			{
				"symbol": "N$",
				"name": "Namibian Dollar",
				"symbol_native": "N$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NAD",
				"name_plural": "Namibian dollars"
			},
			{
				"symbol": "₦",
				"name": "Nigerian Naira",
				"symbol_native": "₦",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NGN",
				"name_plural": "Nigerian nairas"
			},
			{
				"symbol": "C$",
				"name": "Nicaraguan Córdoba",
				"symbol_native": "C$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NIO",
				"name_plural": "Nicaraguan córdobas"
			},
			{
				"symbol": "Nkr",
				"name": "Norwegian Krone",
				"symbol_native": "kr",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NOK",
				"name_plural": "Norwegian kroner"
			},
			{
				"symbol": "NPRs",
				"name": "Nepalese Rupee",
				"symbol_native": "नेरू",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NPR",
				"name_plural": "Nepalese rupees"
			},
			{
				"symbol": "NZ$",
				"name": "New Zealand Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "NZD",
				"name_plural": "New Zealand dollars"
			},
			{
				"symbol": "OMR",
				"name": "Omani Rial",
				"symbol_native": "ر.ع.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "OMR",
				"name_plural": "Omani rials"
			},
			{
				"symbol": "B/.",
				"name": "Panamanian Balboa",
				"symbol_native": "B/.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "PAB",
				"name_plural": "Panamanian balboas"
			},
			{
				"symbol": "S/.",
				"name": "Peruvian Nuevo Sol",
				"symbol_native": "S/.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "PEN",
				"name_plural": "Peruvian nuevos soles"
			},
			{
				"symbol": "₱",
				"name": "Philippine Peso",
				"symbol_native": "₱",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "PHP",
				"name_plural": "Philippine pesos"
			},
			{
				"symbol": "PKRs",
				"name": "Pakistani Rupee",
				"symbol_native": "₨",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "PKR",
				"name_plural": "Pakistani rupees"
			},
			{
				"symbol": "zł",
				"name": "Polish Zloty",
				"symbol_native": "zł",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "PLN",
				"name_plural": "Polish zlotys"
			},
			{
				"symbol": "₲",
				"name": "Paraguayan Guarani",
				"symbol_native": "₲",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "PYG",
				"name_plural": "Paraguayan guaranis"
			},
			{
				"symbol": "QR",
				"name": "Qatari Rial",
				"symbol_native": "ر.ق.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "QAR",
				"name_plural": "Qatari rials"
			},
			{
				"symbol": "RON",
				"name": "Romanian Leu",
				"symbol_native": "RON",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "RON",
				"name_plural": "Romanian lei"
			},
			{
				"symbol": "din.",
				"name": "Serbian Dinar",
				"symbol_native": "дин.",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "RSD",
				"name_plural": "Serbian dinars"
			},
			{
				"symbol": "RUB",
				"name": "Russian Ruble",
				"symbol_native": "руб.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "RUB",
				"name_plural": "Russian rubles"
			},
			{
				"symbol": "RWF",
				"name": "Rwandan Franc",
				"symbol_native": "FR",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "RWF",
				"name_plural": "Rwandan francs"
			},
			{
				"symbol": "SR",
				"name": "Saudi Riyal",
				"symbol_native": "ر.س.‏",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "SAR",
				"name_plural": "Saudi riyals"
			},
			{
				"symbol": "SDG",
				"name": "Sudanese Pound",
				"symbol_native": "SDG",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "SDG",
				"name_plural": "Sudanese pounds"
			},
			{
				"symbol": "Skr",
				"name": "Swedish Krona",
				"symbol_native": "kr",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "SEK",
				"name_plural": "Swedish kronor"
			},
			{
				"symbol": "S$",
				"name": "Singapore Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "SGD",
				"name_plural": "Singapore dollars"
			},
			{
				"symbol": "Ssh",
				"name": "Somali Shilling",
				"symbol_native": "Ssh",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "SOS",
				"name_plural": "Somali shillings"
			},
			{
				"symbol": "SY£",
				"name": "Syrian Pound",
				"symbol_native": "ل.س.‏",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "SYP",
				"name_plural": "Syrian pounds"
			},
			{
				"symbol": "฿",
				"name": "Thai Baht",
				"symbol_native": "฿",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "THB",
				"name_plural": "Thai baht"
			},
			{
				"symbol": "DT",
				"name": "Tunisian Dinar",
				"symbol_native": "د.ت.‏",
				"decimal_digits": 3,
				"rounding": 0,
				"code": "TND",
				"name_plural": "Tunisian dinars"
			},
			{
				"symbol": "T$",
				"name": "Tongan Paʻanga",
				"symbol_native": "T$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "TOP",
				"name_plural": "Tongan paʻanga"
			},
			{
				"symbol": "TL",
				"name": "Turkish Lira",
				"symbol_native": "TL",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "TRY",
				"name_plural": "Turkish Lira"
			},
			{
				"symbol": "TT$",
				"name": "Trinidad and Tobago Dollar",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "TTD",
				"name_plural": "Trinidad and Tobago dollars"
			},
			{
				"symbol": "NT$",
				"name": "New Taiwan Dollar",
				"symbol_native": "NT$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "TWD",
				"name_plural": "New Taiwan dollars"
			},
			{
				"symbol": "TSh",
				"name": "Tanzanian Shilling",
				"symbol_native": "TSh",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "TZS",
				"name_plural": "Tanzanian shillings"
			},
			{
				"symbol": "₴",
				"name": "Ukrainian Hryvnia",
				"symbol_native": "₴",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "UAH",
				"name_plural": "Ukrainian hryvnias"
			},
			{
				"symbol": "USh",
				"name": "Ugandan Shilling",
				"symbol_native": "USh",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "UGX",
				"name_plural": "Ugandan shillings"
			},
			{
				"symbol": "$U",
				"name": "Uruguayan Peso",
				"symbol_native": "$",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "UYU",
				"name_plural": "Uruguayan pesos"
			},
			{
				"symbol": "UZS",
				"name": "Uzbekistan Som",
				"symbol_native": "UZS",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "UZS",
				"name_plural": "Uzbekistan som"
			},
			{
				"symbol": "Bs.F.",
				"name": "Venezuelan Bolívar",
				"symbol_native": "Bs.F.",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "VEF",
				"name_plural": "Venezuelan bolívars"
			},
			{
				"symbol": "₫",
				"name": "Vietnamese Dong",
				"symbol_native": "₫",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "VND",
				"name_plural": "Vietnamese dong"
			},
			{
				"symbol": "FCFA",
				"name": "CFA Franc BEAC",
				"symbol_native": "FCFA",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "XAF",
				"name_plural": "CFA francs BEAC"
			},
			{
				"symbol": "CFA",
				"name": "CFA Franc BCEAO",
				"symbol_native": "CFA",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "XOF",
				"name_plural": "CFA francs BCEAO"
			},
			{
				"symbol": "YR",
				"name": "Yemeni Rial",
				"symbol_native": "ر.ي.‏",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "YER",
				"name_plural": "Yemeni rials"
			},
			{
				"symbol": "R",
				"name": "South African Rand",
				"symbol_native": "R",
				"decimal_digits": 2,
				"rounding": 0,
				"code": "ZAR",
				"name_plural": "South African rand"
			},
			{
				"symbol": "ZK",
				"name": "Zambian Kwacha",
				"symbol_native": "ZK",
				"decimal_digits": 0,
				"rounding": 0,
				"code": "ZMK",
				"name_plural": "Zambian kwachas"
			}
		];

		ngCtrl.relationsList = fastCopy(resolvedRelationsList);
		//#endregion

		//Create new field modal
		ngCtrl.createFieldModal = function () {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createFieldModal.html',
				controller: 'CreateFieldModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () { return ngCtrl; }
				}
			});
		}

		ngCtrl.findFieldType = function (fieldTypeId) {
			for (var typeIndex = 0; typeIndex < ngCtrl.fieldTypes.length; typeIndex++) {
				if (ngCtrl.fieldTypes[typeIndex].id === fieldTypeId) {
					return ngCtrl.fieldTypes[typeIndex];
				}
			}
			return null;
		};

		//Manage field modal
		ngCtrl.manageFieldModal = function (fieldId) {
			var managedField = null;
			for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
				if (ngCtrl.entity.fields[i].id === fieldId) {
					managedField = ngCtrl.entity.fields[i];
				}
			}
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageFieldModal.html',
				controller: 'ManageFieldModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () { return ngCtrl; },
					resolvedField: function () {
						return managedField;
					},
					resolvedRelatedEntity: resolveRelatedEntity()
				}
			});

			function resolveRelatedEntity() {
				// Initialize
				var defer = $q.defer();

				// Process
				function successCallback(response) {
					if (response.object === null) {
						$timeout(function () {
							alert("error in response!");
						}, 0);
					}
					else {
						defer.resolve(response.object);
					}
				}

				function errorCallback(response) {
					if (response.object === null) {
						$timeout(function () {
							alert("error in response!");
						}, 0);
					}
					else {
						defer.reject(response.message);
					}
				}

				if (managedField.fieldType === 21) {
					webvellaCoreService.getEntityMetaById(managedField.relatedEntityId, successCallback, errorCallback);
				}
				else {
					defer.resolve(null);
				}
				// Return
				return defer.promise;
			}
		}

	}



	//// Create Field Controllers
	CreateFieldModalController.$inject = ['ngCtrl', '$scope', '$uibModalInstance', '$log', '$sce', 'ngToast', '$timeout', '$state', 'webvellaCoreService', '$location'];
	
	function CreateFieldModalController(ngCtrl, $scope, $uibModalInstance, $log, $sce, ngToast, $timeout, $state, webvellaCoreService, $location) {
		
		var popupCtrl = this;
		popupCtrl.validation = {};
		popupCtrl.ngCtrl = ngCtrl;
		popupCtrl.createFieldStep2Loading = false;
		popupCtrl.field = webvellaCoreService.initField();

		popupCtrl.isEmpty = function(object){
			return isEmpty(object)
		}

		popupCtrl.fieldTypes = ngCtrl.fieldTypes;
		// Inject a searchable field
		for (var i = 0; i < popupCtrl.fieldTypes.length; i++) {
			popupCtrl.fieldTypes[i].searchBox = popupCtrl.fieldTypes[i].label + " " + popupCtrl.fieldTypes[i].description;
		}

		//Generate roles and checkboxes
		popupCtrl.fieldPermissions = [];
		for (var i = 0; i < popupCtrl.ngCtrl.rolesList.data.length; i++) {

			//Now create the new entity.roles array
			var role = {};
			role.id = popupCtrl.ngCtrl.rolesList.data[i].id;
			role.label = popupCtrl.ngCtrl.rolesList.data[i].name;
			role.canRead = false;
			role.canUpdate = false;
			popupCtrl.fieldPermissions.push(role);
		}

		popupCtrl.togglePermissionRole = function (permission, roleId) {
			//Get the current state

			var permissionArrayRoleIndex = -1;
			if (popupCtrl.field.permissions !== null) {
				for (var k = 0; k < popupCtrl.field.permissions[permission].length; k++) {
					if (popupCtrl.field.permissions[permission][k] === roleId) {
						permissionArrayRoleIndex = k;
					}
				}
			}

			if (permissionArrayRoleIndex !== -1) {
				popupCtrl.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
			}
			else {
				if (popupCtrl.field.permissions === null) {
					popupCtrl.field.permissions = {};
					popupCtrl.field.permissions.canRead = [];
					popupCtrl.field.permissions.canUpdate = [];
				}
				popupCtrl.field.permissions[permission].push(roleId);
			}

		}

		//#region << Selection Tree field >>

		popupCtrl.relationsList = fastCopy(popupCtrl.ngCtrl.relationsList);

		//#region << Selection types >>
		popupCtrl.selectionTypes = [];
		var singleSelectType = {
			key: "single-select",
			value: "can select only one"
		};
		var multiSelectType = {
			key: "multi-select",
			value: "can select many nodes"
		};
		var singleBranchSelectType = {
			key: "single-branch-select",
			value: "can select only one node in branch"
		};

		//#endregion

		//#region << Selection targets >>
		popupCtrl.selectionTargets = [];
		var allNodesSelectTarget = {
			key: "all",
			value: "all nodes can be selected"
		};
		var multiSelectTarget = {
			key: "leaves",
			value: "only leaves - nodes with no children"
		};
		popupCtrl.selectionTargets.push(allNodesSelectTarget);
		popupCtrl.selectionTargets.push(multiSelectTarget);
		//#endregion

		//#region << Tree selection >>
		// ReSharper disable once UnusedParameter
		popupCtrl.getRelationHtml = function (treeId) {


			return result;
		}

		$scope.$watch("popupCtrl.field.relatedEntityId", function (newValue) {
			if (newValue) {
				popupCtrl.SelectedInTreeRelationHtml = "unknown";
				var selectedEntity = findInArray(popupCtrl.eligibleEntitiesForTree, "id", newValue);
				var selectedTree = selectedEntity.recordTrees[0];
				var selectedRelation = selectedEntity.relations[0];

				//Reinit dropdowns
				popupCtrl.selectedEntityRelations = selectedEntity.relations;
				popupCtrl.field.relationId = selectedRelation.id;

				popupCtrl.selectedEntityTrees = selectedEntity.recordTrees;
				popupCtrl.field.selectedTreeId = selectedTree.id;
			}
		});

		$scope.$watch("popupCtrl.field.relationId", function (newValue) {
			if (newValue) {
				var selectedEntity = findInArray(popupCtrl.eligibleEntitiesForTree, "id", popupCtrl.field.relatedEntityId);
				var selectedRelation = findInArray(selectedEntity.relations, "id", newValue);

				popupCtrl.selectionTypes = [];
				popupCtrl.selectionTypes.push(singleSelectType);
				if (selectedRelation) {
					if (selectedRelation.relationType === 2) {
						//Fix type selection if it was a multiselect option
						popupCtrl.field.selectionType = "single-select";
						popupCtrl.field.selectionTarget = "all";
					}
					else if (selectedRelation.relationType === 3) {
						popupCtrl.selectionTypes.push(multiSelectType);
						popupCtrl.selectionTypes.push(singleBranchSelectType);
						popupCtrl.field.selectionType = "single-select";
						popupCtrl.field.selectionTarget = "all";
					}
				}
			}
		});

		//#endregion

		//#endregion

		//Wizard
		popupCtrl.wizard = {};
		popupCtrl.wizard.steps = [];
		//Initialize steps
		var step = fastCopy({ "active": false }, { "completed": false });
		popupCtrl.wizard.steps.push(step); // Dummy step
		step = fastCopy({ "active": false }, { "completed": false });
		popupCtrl.wizard.steps.push(step); // Step 1
		step = fastCopy({ "active": false }, { "completed": false });
		popupCtrl.wizard.steps.push(step); // Step 2
		// Set steps
		popupCtrl.wizard.steps[1].active = true;
		popupCtrl.wizard.selectedType = null;

		popupCtrl.selectType = function (typeId) {
			//find selected type
			for (var typeIndex = 0; typeIndex < popupCtrl.fieldTypes.length; typeIndex++) {
				if (popupCtrl.fieldTypes[typeIndex].id === typeId) {
					popupCtrl.wizard.selectedType = popupCtrl.fieldTypes[typeIndex];
					break;
				}
			}

			if (popupCtrl.wizard.selectedType === null) {
				console.log('The selected field type [' + typeId + '] is missing in collection of field types');
			}

			popupCtrl.field = webvellaCoreService.initField(popupCtrl.wizard.selectedType.id);
			popupCtrl.wizard.steps[1].active = false;
			popupCtrl.wizard.steps[1].completed = true;
			popupCtrl.wizard.steps[2].active = true;
			if (typeId === 17 || typeId === 11) //If dropdown || multiselect
			{
				popupCtrl.field.options = [];
			}
			else if (typeId === 4) {// If date or datetime
				popupCtrl.field.format = "yyyy-MMM-dd";
			}
			else if (typeId === 5) {// If date or datetime
				popupCtrl.field.format = "yyyy-MMM-dd HH:mm";
			}
			else if (typeId === 21) { //if tree field
				popupCtrl.createFieldStep2Loading = true;
				//Tree select
				//List of entities eligible to be selected.Conditions:
				//1.Have 1:N or N:N relation with the current entity as origin, and the target and origin entity are not the same(and equal to the current)
				//2.Have existing trees
				popupCtrl.eligibleEntitiesForTreeProcessQueue = null;
				popupCtrl.eligibleEntitiesForTree = [];
				for (var i = 0; i < popupCtrl.relationsList.length; i++) {
					if (popupCtrl.relationsList[i].originEntityId !== popupCtrl.relationsList[i].targetEntityId && popupCtrl.relationsList[i].targetEntityId === popupCtrl.ngCtrl.entity.id) {

						if (popupCtrl.eligibleEntitiesForTreeProcessQueue === null) {
							popupCtrl.eligibleEntitiesForTreeProcessQueue = {};
						}
						if (popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId]) {
							//entity already added we need just to push the new relations
							popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId].relations.push(popupCtrl.relationsList[i]);
						}
						else {
							//entity not yet registered
							var processItem = {
								entityId: popupCtrl.relationsList[i].originEntityId,
								relations: [],
								processed: false
							}
							processItem.relations.push(popupCtrl.relationsList[i]);
							popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId] = processItem;
						}

					}
				}

				popupCtrl.selectedEntityTrees = [];
				popupCtrl.selectedEntityRelations = [];

				function reInitializeTreeAndRelationListBySelectedEntity(entity) {
					popupCtrl.selectedEntityTrees = entity.recordTrees;
					popupCtrl.selectedEntityRelations = entity.relations;
					popupCtrl.field.selectedTreeId = entity.recordTrees[0].id;
					popupCtrl.field.relationId = entity.relations[0].id;
				}


				function relatedEntitiesWithTreeSuccessCallback(response) {
					popupCtrl.eligibleEntitiesForTreeProcessQueue[response.object.id].processed = true;
					var entityTreeMeta = {};
					entityTreeMeta.id = response.object.id;
					entityTreeMeta.name = response.object.name;
					entityTreeMeta.label = response.object.label;
					entityTreeMeta.recordTrees = response.object.recordTrees;
					entityTreeMeta.relations = popupCtrl.eligibleEntitiesForTreeProcessQueue[response.object.id].relations;
					popupCtrl.eligibleEntitiesForTree.push(entityTreeMeta);
					var allProcessed = true;
					var nextForProcess = {};
					for (var entityProperty in popupCtrl.eligibleEntitiesForTreeProcessQueue) {
						if (popupCtrl.eligibleEntitiesForTreeProcessQueue.hasOwnProperty(entityProperty)) {
							var processedItem = popupCtrl.eligibleEntitiesForTreeProcessQueue[entityProperty];
							if (!processedItem.processed) {
								nextForProcess = processedItem;
								allProcessed = false;
							}
						}
					}
					if (!allProcessed) {
						webvellaCoreService.getEntityMetaById(nextForProcess.entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
					}
					else {
						//All entities meta received
						//Add only entities that have trees defined
						var tempDictionaryOfEligibleEntities = [];
						for (var m = 0; m < popupCtrl.eligibleEntitiesForTree.length; m++) {
							if (popupCtrl.eligibleEntitiesForTree[m].recordTrees.length > 0) {
								tempDictionaryOfEligibleEntities.push(popupCtrl.eligibleEntitiesForTree[m]);
							}
						}
						popupCtrl.eligibleEntitiesForTree = tempDictionaryOfEligibleEntities;
						if (popupCtrl.eligibleEntitiesForTree.length > 0) {
							popupCtrl.field.relatedEntityId = popupCtrl.eligibleEntitiesForTree[0].id;
							reInitializeTreeAndRelationListBySelectedEntity(popupCtrl.eligibleEntitiesForTree[0]);

							popupCtrl.createFieldStep2Loading = false;
						}
						else {
							popupCtrl.createFieldStep2Error = true;
							popupCtrl.createFieldStep2ErrorMessage = "There are other entities with proper relations, but they do not have existing trees defined";
							popupCtrl.createFieldStep2Loading = false;
						}

					}
				}
				function relatedEntitiesWithTreeErrorCallback() {

				}

				if (popupCtrl.eligibleEntitiesForTreeProcessQueue !== null) {
					webvellaCoreService.getEntityMetaById(popupCtrl.eligibleEntitiesForTreeProcessQueue[Object.keys(popupCtrl.eligibleEntitiesForTreeProcessQueue)[0]].entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
				}
				else {
					popupCtrl.createFieldStep2Error = true;
					popupCtrl.createFieldStep2ErrorMessage = "There are no other entities that has 1:N or N:N relation with the current entity being a target";
					popupCtrl.createFieldStep2Loading = false;
				}
			}
		}
		popupCtrl.setActiveStep = function (stepIndex) {
			popupCtrl.createFieldStep2Error = false;
			popupCtrl.createFieldStep2ErrorMessage = "";
			if (popupCtrl.wizard.steps[stepIndex].completed) {
				for (var i = 1; i < 3; i++) {
					popupCtrl.wizard.steps[i].active = false;
				}
				popupCtrl.wizard.steps[stepIndex].active = true;
			}
		}

		// Logic
		popupCtrl.completeStep2 = function () {
			popupCtrl.wizard.steps[2].active = false;
			popupCtrl.wizard.steps[2].completed = true;
		}

		popupCtrl.calendars = {};
		popupCtrl.openCalendar = function (event, name) {
			popupCtrl.calendars[name] = true;
		}



		//Currency
		popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[0].code;

		//Identifier GUID field specific functions
		popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
		popupCtrl.defaultValueTextboxEnabled = true;
		popupCtrl.defaultValueTextboxPlaceholder = "fill in a GUID";
		popupCtrl.defaultValueTextboxValue = null;
		popupCtrl.uniqueGuidGenerateToggle = function (newValue) {
			if (popupCtrl.field.fieldTypeId === 16) {
				if (newValue) { // if it is checked
					popupCtrl.defaultValueTextboxEnabled = false;
					popupCtrl.defaultValueTextboxPlaceholder = "will be auto-generated";
					popupCtrl.defaultValueTextboxValue = popupCtrl.field.defaultValue;
					popupCtrl.field.defaultValue = null;
				}
				else {
					popupCtrl.defaultValueTextboxEnabled = true;
					popupCtrl.defaultValueTextboxPlaceholder = "fill in a GUID";
					popupCtrl.field.defaultValue = popupCtrl.defaultValueTextboxValue;
					popupCtrl.defaultValueTextboxValue = null;
				}
			}
		}
		popupCtrl.uniqueGuidPropertyChecked = function (newValue) {
			if (popupCtrl.field.fieldTypeId === 16) {
				if (newValue) {
					popupCtrl.field.generateNewId = true;
					popupCtrl.uniqueGuidGenerateCheckboxEnabled = false;
					popupCtrl.uniqueGuidGenerateToggle(true);
				}
				else {
					popupCtrl.field.generateNewId = false;
					popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
					popupCtrl.uniqueGuidGenerateToggle(false);
				}
			}
		}



		//////
		popupCtrl.ok = function () {
			popupCtrl.validation = {};
			switch (popupCtrl.field.fieldType) {
				case 3:
					for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
						if (ngCtrl.currencyMetas[i].code === popupCtrl.selectedCurrencyMeta) {
							popupCtrl.field.currency.symbol = ngCtrl.currencyMetas[i].symbol;
							popupCtrl.field.currency.symbolNative = ngCtrl.currencyMetas[i].symbol_native;
							popupCtrl.field.currency.name = ngCtrl.currencyMetas[i].name;
							popupCtrl.field.currency.namePlural = ngCtrl.currencyMetas[i].name_plural;
							popupCtrl.field.currency.code = ngCtrl.currencyMetas[i].code;
							popupCtrl.field.currency.decimalDigits = ngCtrl.currencyMetas[i].decimal_digits;
							popupCtrl.field.currency.rounding = ngCtrl.currencyMetas[i].rounding;
							popupCtrl.field.currency.symbolPlacement = 1;
						}
					}
					break;
				case 4: //Date
					if (popupCtrl.field.defaultValue !== null) {
						popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('day').utc().toISOString();
					}
					break;
				case 5: //Date & Time
					if (popupCtrl.field.defaultValue !== null) {
						popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('minute').utc().toISOString();
					}
					break;
				case 11: //Multiselect
					if (popupCtrl.field.required && (!popupCtrl.field.defaultValue || popupCtrl.field.defaultValue.length == 0)) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error:</span> You need to select at least one option as default, when the field is required'
						});
					}
					break;
			}
			webvellaCoreService.createField(popupCtrl.field, popupCtrl.ngCtrl.entity.id, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		//Dropdown or Multiselect
		popupCtrl.addDropdownValue = function () {
			var option = {
				key: null,
				value: null
			};
			popupCtrl.field.options.push(option);
		}

		popupCtrl.deleteDropdownValue = function (index) {
			var defaultKeyIndex = -1;
			if (popupCtrl.isKeyDefault(popupCtrl.field.options[index].key)) {
				for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
					if (popupCtrl.field.defaultValue[j] !== popupCtrl.field.options[index].key) {
						defaultKeyIndex = j;
						break;
					}
				}
			}
			if (popupCtrl.field.defaultValue) {
				popupCtrl.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
			}
			popupCtrl.field.options.splice(index, 1); // remove from options
		}

		popupCtrl.toggleKeyAsDefault = function (key) {
			if (!key || key == "") {
  				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> You need to fill in value or label first'
				});
			}
			else {
				if (!popupCtrl.field.defaultValue) {
					popupCtrl.field.defaultValue = [];
				}
				if (popupCtrl.field.defaultValue.indexOf(key) > -1) {
					//There could be multiple keys with the same key
					var cleanArray = [];
					for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
						if (popupCtrl.field.defaultValue[j] !== key) {
							cleanArray.push(popupCtrl.field.defaultValue[j]);
						}
					}
					popupCtrl.field.defaultValue = fastCopy(cleanArray);
				} else {
					popupCtrl.field.defaultValue.push(key);
				}
			}
		}

		popupCtrl.isKeyDefault = function (key) {

			if (popupCtrl.field.defaultValue && popupCtrl.field.defaultValue.indexOf(key) > -1) {
				return true;
			} else {
				return false;
			}
		}

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.field, location);
		}
	};

	//// Create Field Controllers
	ManageFieldModalController.$inject = ['ngCtrl', 'resolvedField', '$uibModal', '$uibModalInstance', '$log', 'ngToast', '$timeout', '$state',
						'webvellaCoreService', '$location', 'resolvedRelatedEntity', '$sce','$scope'];
	
	function ManageFieldModalController(ngCtrl, resolvedField, $uibModal, $uibModalInstance, $log, ngToast, $timeout, $state,
						webvellaCoreService, $location, resolvedRelatedEntity, $sce, $scope) {
		
		var popupCtrl = this;
		popupCtrl.isEmpty = function(object){
			return isEmpty(object)
		}
		//#region << Init >>
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.ngCtrl = ngCtrl;

		popupCtrl.field = fastCopy(resolvedField);
		if (popupCtrl.field.permissions === null) {
			popupCtrl.field.permissions = {
				canRead: [],
				canUpdate: []
			}
		}


		popupCtrl.fieldTypes = ngCtrl.fieldTypes;
		popupCtrl.fieldType = null;

		for (var i = 0; i < popupCtrl.fieldTypes.length; i++) {
			if (popupCtrl.fieldTypes[i].id === popupCtrl.field.fieldType) {
				popupCtrl.fieldType = popupCtrl.fieldTypes[i];
			}
		}

		//#endregion

		//#region << Logic >>

		//Generate roles and checkboxes
		popupCtrl.fieldPermissions = [];
		for (var i = 0; i < popupCtrl.ngCtrl.rolesList.data.length; i++) {

			//Now create the new entity.roles array
			var role = {};
			role.id = popupCtrl.ngCtrl.rolesList.data[i].id;
			role.label = popupCtrl.ngCtrl.rolesList.data[i].name;
			role.canRead = false;
			if (popupCtrl.field.permissions !== null && popupCtrl.field.permissions.canRead.indexOf(popupCtrl.ngCtrl.rolesList.data[i].id) > -1) {
				role.canRead = true;
			}
			role.canUpdate = false;
			if (popupCtrl.field.permissions !== null && popupCtrl.field.permissions.canUpdate.indexOf(popupCtrl.ngCtrl.rolesList.data[i].id) > -1) {
				role.canUpdate = true;
			}
			popupCtrl.fieldPermissions.push(role);
		}

		popupCtrl.togglePermissionRole = function (permission, roleId) {
			//Get the current state

			var permissionArrayRoleIndex = -1;
			if (popupCtrl.field.permissions !== null) {
				for (var k = 0; k < popupCtrl.field.permissions[permission].length; k++) {
					if (popupCtrl.field.permissions[permission][k] === roleId) {
						permissionArrayRoleIndex = k;
					}
				}
			}

			if (permissionArrayRoleIndex !== -1) {
				popupCtrl.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
			}
			else {
				if (popupCtrl.field.permissions === null) {
					popupCtrl.field.permissions = {};
					popupCtrl.field.permissions.canRead = [];
					popupCtrl.field.permissions.canUpdate = [];
				}
				popupCtrl.field.permissions[permission].push(roleId);
			}

		}

		//Currency
		if (popupCtrl.field.fieldType === 3) {
			popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[0].code;
			if (popupCtrl.field.currency !== null && popupCtrl.field.currency !== {} && popupCtrl.field.currency.code) {
				for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
					if (popupCtrl.field.currency.code === ngCtrl.currencyMetas[i].code) {
						popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[i].code;
					}
				}
			}
		}

		//Identifier GUID field specific functions
		popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
		popupCtrl.defaultValueTextboxEnabled = true;
		popupCtrl.defaultValueTextboxPlaceholder = "fill in a GUID";
		popupCtrl.defaultValueTextboxValue = null;
		popupCtrl.uniqueGuidGenerateToggle = function (newValue) {
			if (newValue) { // if it is checked
				popupCtrl.defaultValueTextboxEnabled = false;
				popupCtrl.defaultValueTextboxPlaceholder = "will be auto-generated";
				popupCtrl.defaultValueTextboxValue = popupCtrl.field.defaultValue;
				popupCtrl.field.defaultValue = null;
			}
			else {
				popupCtrl.defaultValueTextboxEnabled = true;
				popupCtrl.defaultValueTextboxPlaceholder = "fill in a GUID";
				popupCtrl.field.defaultValue = popupCtrl.defaultValueTextboxValue;
				popupCtrl.defaultValueTextboxValue = null;
			}
		}

		popupCtrl.uniqueGuidPropertyChecked = function (newValue) {
			if (popupCtrl.field.fieldType === 16) {
				if (newValue) {
					popupCtrl.field.generateNewId = true;
					popupCtrl.uniqueGuidGenerateCheckboxEnabled = false;
					popupCtrl.uniqueGuidGenerateToggle(true);
				}
				else {
					popupCtrl.field.generateNewId = false;
					popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
					popupCtrl.uniqueGuidGenerateToggle(false);
				}
			}
		}

		// Date and Date time
		popupCtrl.calendars = {};
		popupCtrl.openCalendar = function (event, name) {
			popupCtrl.calendars[name] = true;
		}

		//Tree select
		var relationsList = fastCopy(popupCtrl.ngCtrl.relationsList);
		var selectedRelation = {};
		for (var i = 0; i < relationsList.length; i++) {
			if (relationsList[i].id === popupCtrl.field.relationId) {
				selectedRelation = relationsList[i];
			}
		}
		popupCtrl.getTreeSelectRelatedEntityName = function () {
			var relatedEntity = fastCopy(resolvedRelatedEntity);
			return "<i class='fa fa-fw fa-" + relatedEntity.iconName + "'></i> " + relatedEntity.name;
		}
		popupCtrl.getTreeSelectRecordTreeName = function () {
			var relatedEntity = fastCopy(resolvedRelatedEntity);
			for (var i = 0; i < relatedEntity.recordTrees.length; i++) {
				if (relatedEntity.recordTrees[i].id === popupCtrl.field.selectedTreeId) {
					return "<i class='fa fa-fw fa-sitemap'></i> " + relatedEntity.recordTrees[i].name;
				}
			}
			return "";
		}
		popupCtrl.getTreeSelectRelationHtml = function () {
			var result = "unknown";
			if (selectedRelation) {
				if (selectedRelation.relationType === 2) {
					result = $sce.trustAsHtml(selectedRelation.name + " <span class=\"badge badge-primary badge-inverse\" title=\"One to Many\" style=\"margin-left:5px;\">1 : N</span>");
				}
				else if (selectedRelation.relationType === 3) {
					result = $sce.trustAsHtml(selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="Many to Many" style="margin-left:5px;">N : N</span>');
				}
			}
			return result;
		}

		//#region << Selection types >>
		popupCtrl.selectionTypes = [];
		var singleSelectType = {
			key: "single-select",
			value: "can select only one"
		};
		var multiSelectType = {
			key: "multi-select",
			value: "can select many nodes"
		};
		var singleBranchSelectType = {
			key: "single-branch-select",
			value: "can select only one node in branch"
		};

		//#endregion

		//#region << Selection targets >>
		popupCtrl.selectionTargets = [];
		var allNodesSelectTarget = {
			key: "all",
			value: "all nodes can be selected"
		};
		var multiSelectTarget = {
			key: "leaves",
			value: "only leaves - nodes with no children"
		};
		popupCtrl.selectionTargets.push(allNodesSelectTarget);
		popupCtrl.selectionTargets.push(multiSelectTarget);
		//#endregion

		popupCtrl.selectionTypes = [];
		popupCtrl.selectionTypes.push(singleSelectType);
		if (selectedRelation && selectedRelation.relationType === 3) {
			popupCtrl.selectionTypes.push(multiSelectType);
			popupCtrl.selectionTypes.push(singleBranchSelectType);
		}
		//#endregion

		popupCtrl.ok = function () {
			switch (popupCtrl.field.fieldType) {
				case 3:
					for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
						if (ngCtrl.currencyMetas[i].code === popupCtrl.selectedCurrencyMeta) {
							popupCtrl.field.currency.symbol = ngCtrl.currencyMetas[i].symbol;
							popupCtrl.field.currency.symbolNative = ngCtrl.currencyMetas[i].symbol_native;
							popupCtrl.field.currency.name = ngCtrl.currencyMetas[i].name;
							popupCtrl.field.currency.namePlural = ngCtrl.currencyMetas[i].name_plural;
							popupCtrl.field.currency.code = ngCtrl.currencyMetas[i].code;
							popupCtrl.field.currency.decimalDigits = ngCtrl.currencyMetas[i].decimal_digits;
							popupCtrl.field.currency.rounding = ngCtrl.currencyMetas[i].rounding;
							popupCtrl.field.currency.symbolPlacement = 1;
						}
					}
					break;
				case 4: //Date
					if (popupCtrl.field.defaultValue !== null) {
						popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('day').utc().toISOString();
					}
					break;
				case 5: //Date & Time
					if (popupCtrl.field.defaultValue !== null) {
						popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('minute').utc().toISOString();
					}
					break;
			}
			webvellaCoreService.updateField(popupCtrl.field, popupCtrl.ngCtrl.entity.id, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		//Delete field
		//Create new field modal
		popupCtrl.deleteFieldModal = function () {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteFieldModal.html',
				controller: 'DeleteFieldModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentpopupCtrl: function () { return popupCtrl; }
				}
			});
		}

		//Dropdown or Multiselect
		popupCtrl.addDropdownValue = function () {
			var option = {
				key: null,
				value: null
			};
			popupCtrl.field.options.push(option);
		}

		popupCtrl.deleteDropdownValue = function (index) {
			//if the removed field is with the default value or in the default values list(multiselect)
			if (popupCtrl.field.defaultValue && popupCtrl.field.fieldType == 11) { //Multiselect
				var defaultKeyIndex = -1;
				if (popupCtrl.isKeyDefault(popupCtrl.field.options[index].key)) {
					for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
						if (popupCtrl.field.defaultValue[j] === popupCtrl.field.options[index].key) {
							defaultKeyIndex = j;
							break;
						}
					}
				}
				popupCtrl.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
			}
			else if (popupCtrl.field.defaultValue && popupCtrl.field.fieldType == 17) {
				if (popupCtrl.field.options[index].key === popupCtrl.field.defaultValue) {
					popupCtrl.field.defaultValue = ""; //Remove from default										
				}

			}

			popupCtrl.field.options.splice(index, 1); // remove from options
		}

		popupCtrl.toggleKeyAsDefault = function (key) {
			if (popupCtrl.field.defaultValue.indexOf(key) > -1) {
				//There could be multiple keys with the same key
				var cleanArray = [];
				for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
					if (popupCtrl.field.defaultValue[j] !== key) {
						cleanArray.push(popupCtrl.field.defaultValue[j]);
					}
				}
				popupCtrl.field.defaultValue = fastCopy(cleanArray);
			} else {
				popupCtrl.field.defaultValue.push(key);
			}
		}

		popupCtrl.isKeyDefault = function (key) {
			if (popupCtrl.field.defaultValue && popupCtrl.field.defaultValue.indexOf(key) > -1) {
				return true;
			} else {
				return false;
			}
		}

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.field, location);
		}
	};


	//// Modal Controllers
	DeleteFieldModalController.$inject = ['parentpopupCtrl', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];

	
	function DeleteFieldModalController(parentpopupCtrl, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
		
		var popupCtrl = this;
		popupCtrl.parentData = parentpopupCtrl;

		popupCtrl.ok = function () {
			webvellaCoreService.deleteField(popupCtrl.parentData.field.id, popupCtrl.parentData.ngCtrl.entity.id, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			popupCtrl.parentData.modalInstance.close('success');
			$timeout(function () {
				$state.go("webvella-admin-entity-fields", { name: popupCtrl.parentData.ngCtrl.entity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
	};


})();

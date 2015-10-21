/* entity-fields.module.js */

/**
* @desc this module manages the entity record fields in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityFieldsController', controller)
        .controller('CreateFieldModalController', CreateFieldModalController)
        .controller('ManageFieldModalController', ManageFieldModalController)
        .controller('DeleteFieldModalController', DeleteFieldModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];
	/* @ngInject */
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
					controllerAs: 'contentData'
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
	/* @ngInject */
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
		var accessPermission = false;
		for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
			if (resolvedCurrentUser.roles[i] == "bdc56420-caf0-4030-8a0e-d264938e0cda") {
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

		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	// Resolve Roles list /////////////////////////
	resolveRolesList.$inject = ['$q', '$log', 'webvellaAdminService'];
	/* @ngInject */
	function resolveRolesList($q, $log, webvellaAdminService) {
		$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getRecordsByEntityName("null", "role", "null", "null", successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout'];
	/* @ngInject */
	function resolveRelationsList($q, $log, webvellaAdminService, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedRolesList',
						'resolvedRelationsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedRolesList,
						resolvedRelationsList) {
		$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;

		//#region << Init >>
		contentData.search = {};
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.rolesList = fastCopy(resolvedRolesList);
		contentData.entity.fields = contentData.entity.fields.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		//#region << Update page title>> 
		contentData.pageTitle = "Entity Fields | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		//Hide Sidemenu
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		$scope.$on("$destroy", function () {
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		});
		//#endregion


		contentData.fieldTypes = getFieldTypes();

		//Currency meta - used in Create and Manage fields
		contentData.currencyMetas =
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

		contentData.relationsList = fastCopy(resolvedRelationsList);
		//#endregion

		//Create new field modal
		contentData.createFieldModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createFieldModal.html',
				controller: 'CreateFieldModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () { return contentData; }
				}
			});
		}

		//Manage field modal
		contentData.manageFieldModal = function (fieldId) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageFieldModal.html',
				controller: 'ManageFieldModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () { return contentData; },
					resolvedField: function () {
						var managedField = null;
						for (var i = 0; i < contentData.entity.fields.length; i++) {
							if (contentData.entity.fields[i].id === fieldId) {
								managedField = contentData.entity.fields[i];
							}
						}
						return managedField;
					}
				}
			});
		}

		$log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	//// Create Field Controllers
	CreateFieldModalController.$inject = ['contentData', '$scope','$modalInstance', '$log','$sce', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'webvellaRootService', '$location'];
	/* @ngInject */
	function CreateFieldModalController(contentData,$scope, $modalInstance, $log,$sce, webvellaAdminService, ngToast, $timeout, $state, webvellaRootService, $location) {
		$log.debug('webvellaAdmin>entities>CreateFieldModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;

		popupData.contentData = contentData;
		popupData.createFieldStep2Loading = false;
		popupData.field = webvellaAdminService.initField();

		popupData.fieldTypes = contentData.fieldTypes;
		// Inject a searchable field
		for (var i = 0; i < popupData.fieldTypes.length; i++) {
			popupData.fieldTypes[i].searchBox = popupData.fieldTypes[i].label + " " + popupData.fieldTypes[i].description;
		}

		//Generate roles and checkboxes
		popupData.fieldPermissions = [];
		for (var i = 0; i < popupData.contentData.rolesList.data.length; i++) {

			//Now create the new entity.roles array
			var role = {};
			role.id = popupData.contentData.rolesList.data[i].id;
			role.label = popupData.contentData.rolesList.data[i].name;
			role.canRead = false;
			role.canUpdate = false;
			popupData.fieldPermissions.push(role);
		}

		popupData.togglePermissionRole = function (permission, roleId) {
			//Get the current state

			var permissionArrayRoleIndex = -1;
			if (popupData.field.permissions != null) {
				for (var k = 0; k < popupData.field.permissions[permission].length; k++) {
					if (popupData.field.permissions[permission][k] === roleId) {
						permissionArrayRoleIndex = k;
					}
				}
			}

			if (permissionArrayRoleIndex != -1) {
				popupData.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
			}
			else {
				if (popupData.field.permissions == null) {
					popupData.field.permissions = {};
					popupData.field.permissions.canRead = [];
					popupData.field.permissions.canUpdate = [];
				}
				popupData.field.permissions[permission].push(roleId);
			}

		}

		//#region << Selection Tree field >>

		popupData.relationsList = fastCopy(popupData.contentData.relationsList);

		//#region << Selection types >>
		popupData.selectionTypes = [];
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
		popupData.selectionTargets = [];
		var allNodesSelectTarget = {
			key: "all",
			value: "all nodes can be selected"
		};
		var multiSelectTarget = {
			key: "leaves",
			value: "only leaves - nodes with no children"
		};
		popupData.selectionTargets.push(allNodesSelectTarget);
		popupData.selectionTargets.push(multiSelectTarget);
		//#endregion

		//#region << Tree selection >>
		popupData.getRelationHtml = function (treeId) {


			return result;
		}

		$scope.$watch("popupData.field.selectedTreeId", function (newValue, oldValue) {
			if (newValue) {
				popupData.SelectedInTreeRelationHtml = "unknown";
				var selectedTree = findInArray(popupData.entityTreeList, "id", newValue);
				var selectedRelation = findInArray(popupData.relationsList, "id", selectedTree.relationId);
				popupData.selectionTypes = [];
				popupData.selectionTypes.push(singleSelectType);
				if (selectedRelation) {
					if (selectedRelation.relationType == 2) {
						//Fix type selection if it was a multiselect option
						popupData.field.selectionType = "single-select";
						popupData.SelectedInTreeRelationHtml = $sce.trustAsHtml(selectedRelation.name + " <span class=\"badge badge-primary badge-inverse\" title=\"One to Many\" style=\"margin-left:5px;\">1 : N</span>");
					}
					else if (selectedRelation.relationType == 3) {
						popupData.selectionTypes.push(multiSelectType);
						popupData.selectionTypes.push(singleBranchSelectType);
						popupData.SelectedInTreeRelationHtml = $sce.trustAsHtml(selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="Many to Many" style="margin-left:5px;">N : N</span>');
					}
				}
			}
		});
		//#endregion

		//#endregion

		//Wizard
		popupData.wizard = {};
		popupData.wizard.steps = [];
		//Initialize steps
		var step = fastCopy({ "active": false }, { "completed": false });
		popupData.wizard.steps.push(step); // Dummy step
		step = fastCopy({ "active": false }, { "completed": false });
		popupData.wizard.steps.push(step); // Step 1
		step = fastCopy({ "active": false }, { "completed": false });
		popupData.wizard.steps.push(step); // Step 2
		// Set steps
		popupData.wizard.steps[1].active = true;
		popupData.wizard.selectedType = null;

		popupData.selectType = function (typeId) {
			var typeIndex = typeId - 1;
			popupData.wizard.selectedType = popupData.fieldTypes[typeIndex];
			popupData.field = webvellaAdminService.initField(popupData.wizard.selectedType.id);

			popupData.wizard.steps[1].active = false;
			popupData.wizard.steps[1].completed = true;
			popupData.wizard.steps[2].active = true;
			if (typeId == 17 || typeId == 11) //If dropdown || multiselect
			{
				popupData.field.options = [];
			}
			else if (typeId == 4 ) {// If date or datetime
				popupData.field.format = "yyyy-MMM-dd";
			}
			else if (typeId == 5){// If date or datetime
				popupData.field.format = "yyyy-MMM-dd HH:mm";
			}
			else if (typeId == 20) {
				popupData.createFieldStep2Loading = true;
				//Tree select
				//List of entities eligible to be selected.Conditions:
				//1.Have 1:N or N:N relation with the current entity as origin, and the target and origin entity are not the same(and equal to the current)
				//2.Have existing trees
				popupData.eligibleEntitiesForTreeProcessQueue = null;
				popupData.eligibleEntitiesForTree = [];
				for (var i = 0; i < popupData.relationsList.length; i++) {
					if (popupData.relationsList[i].originEntityId != popupData.relationsList[i].targetEntityId && popupData.relationsList[i].targetEntityId == popupData.contentData.entity.id) {

						if (popupData.eligibleEntitiesForTreeProcessQueue == null) {
							popupData.eligibleEntitiesForTreeProcessQueue = {};
						}
						if (popupData.eligibleEntitiesForTreeProcessQueue[popupData.relationsList[i].originEntityId]) {
							//entity already added we need just to push the new relations
							popupData.eligibleEntitiesForTreeProcessQueue[popupData.relationsList[i].originEntityId].relations.push(popupData.relationsList[i]);
						}
						else {
							//entity not yet registered
							var processItem = {
								entityId: popupData.relationsList[i].originEntityId,
								relations: [],
								processed: false
							}
							processItem.relations.push(popupData.relationsList[i]);
							popupData.eligibleEntitiesForTreeProcessQueue[popupData.relationsList[i].originEntityId] = processItem;
						}
						
					}
				}
				function relatedEntitiesWithTreeSuccessCallback(response) {
					popupData.eligibleEntitiesForTreeProcessQueue[response.object.id].processed = true;
					var entityTreeMeta = {};
					entityTreeMeta.id = response.object.id;
					entityTreeMeta.name = response.object.name;
					entityTreeMeta.label = response.object.label;
					entityTreeMeta.recordTrees = response.object.recordTrees;
					popupData.eligibleEntitiesForTree.push(entityTreeMeta);
					var allProcessed = true;
					var nextForProcess = {};
					for (var entityProperty in popupData.eligibleEntitiesForTreeProcessQueue) {
						var processedItem = popupData.eligibleEntitiesForTreeProcessQueue[entityProperty];
						if (!processedItem.processed) {
							nextForProcess = processedItem;
							allProcessed = false;
						}
					}
					if (!allProcessed) {
						webvellaAdminService.getEntityMetaById(nextForProcess.entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
					}
					else {
						//All entities meta received
						//Add only entities that have trees defined
						var tempDictionaryOfEligibleEntities = [];
						for (var m = 0; m < popupData.eligibleEntitiesForTree.length; m++) {
							if (popupData.eligibleEntitiesForTree[m].recordTrees.length > 0) {
								tempDictionaryOfEligibleEntities.push(popupData.eligibleEntitiesForTree[m]);
							}
						}
						popupData.eligibleEntitiesForTree = tempDictionaryOfEligibleEntities;
						if (popupData.eligibleEntitiesForTree.length > 0) {
							popupData.field.relatedEntityId = popupData.eligibleEntitiesForTree[0].id;
							//BOZTODO - I need to fill the list of probably relations and trees on entity selection or change. Based on the relation type we need to also manipulate the selection type

							popupData.createFieldStep2Loading = false;
						}
						else {
							popupData.createFieldStep2Error = true;
							popupData.createFieldStep2ErrorMessage = "There are other entities with proper relations, but they do not have existing trees defined";
							popupData.createFieldStep2Loading = false;
						}
						
					}
				}
				function relatedEntitiesWithTreeErrorCallback(response) {

				}

				if (popupData.eligibleEntitiesForTreeProcessQueue != null) {
					webvellaAdminService.getEntityMetaById(popupData.eligibleEntitiesForTreeProcessQueue[Object.keys(popupData.eligibleEntitiesForTreeProcessQueue)[0]].entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
				}
				else {
					popupData.createFieldStep2Error = true;
					popupData.createFieldStep2ErrorMessage = "There are no other entities that has 1:N or N:N relation with the current entity";
				}
			}
		}
		popupData.setActiveStep = function (stepIndex) {
			popupData.createFieldStep2Error = false;
			popupData.createFieldStep2ErrorMessage = "";
			if (popupData.wizard.steps[stepIndex].completed) {
				for (var i = 1; i < 3; i++) {
					popupData.wizard.steps[i].active = false;
				}
				popupData.wizard.steps[stepIndex].active = true;
			}
		}

		// Logic
		popupData.completeStep2 = function () {
			popupData.wizard.steps[2].active = false;
			popupData.wizard.steps[2].completed = true;
		}

		popupData.calendars = {};
		popupData.openCalendar = function (event, name) {
			popupData.calendars[name] = true;
		}



		//Currency
		popupData.selectedCurrencyMeta = contentData.currencyMetas[0].code;

		//Identifier GUID field specific functions
		popupData.uniqueGuidGenerateCheckboxEnabled = true;
		popupData.defaultValueTextboxEnabled = true;
		popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
		popupData.defaultValueTextboxValue = null;
		popupData.uniqueGuidGenerateToggle = function (newValue) {
			if (newValue) { // if it is checked
				popupData.defaultValueTextboxEnabled = false;
				popupData.defaultValueTextboxPlaceholder = "will be auto-generated";
				popupData.defaultValueTextboxValue = popupData.field.defaultValue;
				popupData.field.defaultValue = null;
			}
			else {
				popupData.defaultValueTextboxEnabled = true;
				popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
				popupData.field.defaultValue = popupData.defaultValueTextboxValue;
				popupData.defaultValueTextboxValue = null;
			}
		}
		popupData.uniqueGuidPropertyChecked = function (newValue) {
			if (newValue) {
				popupData.field.generateNewId = true;
				popupData.uniqueGuidGenerateCheckboxEnabled = false;
				popupData.uniqueGuidGenerateToggle(true);
			}
			else {
				popupData.field.generateNewId = false;
				popupData.uniqueGuidGenerateCheckboxEnabled = true;
				popupData.uniqueGuidGenerateToggle(false);
			}
		}



		//////
		popupData.ok = function () {
			switch (popupData.field.fieldType) {
				case 3:
					for (var i = 0; i < contentData.currencyMetas.length; i++) {
						if (contentData.currencyMetas[i].code === popupData.selectedCurrencyMeta) {
							popupData.field.currency.symbol = contentData.currencyMetas[i].symbol;
							popupData.field.currency.symbolNative = contentData.currencyMetas[i].symbol_native;
							popupData.field.currency.name = contentData.currencyMetas[i].name;
							popupData.field.currency.namePlural = contentData.currencyMetas[i].name_plural;
							popupData.field.currency.code = contentData.currencyMetas[i].code;
							popupData.field.currency.decimalDigits = contentData.currencyMetas[i].decimal_digits;
							popupData.field.currency.rounding = contentData.currencyMetas[i].rounding;
							popupData.field.currency.symbolPlacement = 1;
						}
					}
					break;
				case 4: //Date
					if(popupData.field.defaultValue!=null) {
						popupData.field.defaultValue = moment(popupData.field.defaultValue).startOf('day').utc().toISOString();
					}
					break;
				case 5: //Date & Time
					if(popupData.field.defaultValue!=null) {
						popupData.field.defaultValue = moment(popupData.field.defaultValue).startOf('minute').utc().toISOString();
					}
					break;
			}
			webvellaAdminService.createField(popupData.field, popupData.contentData.entity.id, successCallback, errorCallback);
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		//Dropdown or Multiselect
		popupData.addDropdownValue = function () {
			var option = {
				key: null,
				value: null
			};
			popupData.field.options.push(option);
		}

		popupData.deleteDropdownValue = function (index) {
			var defaultKeyIndex = -1;
			if (popupData.isKeyDefault(popupData.field.options[index].key)) {
				for (var j = 0; j < popupData.field.defaultValue.length; j++) {
					if (popupData.field.defaultValue[j] !== popupData.field.options[index].key) {
						defaultKeyIndex = j;
						break;
					}
				}
			}
			popupData.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
			popupData.field.options.splice(index, 1); // remove from options
		}

		popupData.toggleKeyAsDefault = function (key) {
			if (popupData.field.defaultValue.indexOf(key) > -1) {
				//There could be multiple keys with the same key
				var cleanArray = [];
				for (var j = 0; j < popupData.field.defaultValue.length; j++) {
					if (popupData.field.defaultValue[j] !== key) {
						cleanArray.push(popupData.field.defaultValue[j]);
					}
				}
				popupData.field.defaultValue = fastCopy(cleanArray);
			} else {
				popupData.field.defaultValue.push(key);
			}
		}

		popupData.isKeyDefault = function (key) {
			if (popupData.field.defaultValue.indexOf(key) > -1) {
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
			$modalInstance.close('success');
			webvellaRootService.GoToState($state, $state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.field, location);
		}
		$log.debug('webvellaAdmin>entities>CreateFieldModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//// Create Field Controllers
	ManageFieldModalController.$inject = ['contentData', 'resolvedField', '$uibModal', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'webvellaRootService', '$location'];
	/* @ngInject */
	function ManageFieldModalController(contentData, resolvedField, $uibModal, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, webvellaRootService, $location) {
		$log.debug('webvellaAdmin>entities>ManageFieldModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $modalInstance;
		popupData.contentData = contentData;

		popupData.field = fastCopy(resolvedField);
		if (popupData.field.permissions == null) {
			popupData.field.permissions = {
				canRead: [],
				canUpdate:[]
			}
		}


		popupData.fieldTypes = contentData.fieldTypes;
		popupData.fieldType = null;

		for (var i = 0; i < popupData.fieldTypes.length; i++) {
			if (popupData.fieldTypes[i].id === popupData.field.fieldType) {
				popupData.fieldType = popupData.fieldTypes[i];
			}
		}

		//Generate roles and checkboxes
		popupData.fieldPermissions = [];
		for (var i = 0; i < popupData.contentData.rolesList.data.length; i++) {

			//Now create the new entity.roles array
			var role = {};
			role.id = popupData.contentData.rolesList.data[i].id;
			role.label = popupData.contentData.rolesList.data[i].name;
			role.canRead = false;
			if (popupData.field.permissions != null && popupData.field.permissions.canRead.indexOf(popupData.contentData.rolesList.data[i].id) > -1) {
				role.canRead = true;
			}
			role.canUpdate = false;
			if (popupData.field.permissions != null && popupData.field.permissions.canUpdate.indexOf(popupData.contentData.rolesList.data[i].id) > -1) {
				role.canUpdate = true;
			}
			popupData.fieldPermissions.push(role);
		}

		popupData.togglePermissionRole = function (permission, roleId) {
			//Get the current state

			var permissionArrayRoleIndex = -1;
			if (popupData.field.permissions != null) {
				for (var k = 0; k < popupData.field.permissions[permission].length; k++) {
					if (popupData.field.permissions[permission][k] === roleId) {
						permissionArrayRoleIndex = k;
					}
				}
			}

			if (permissionArrayRoleIndex != -1) {
				popupData.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
			}
			else {
				if (popupData.field.permissions == null) {
					popupData.field.permissions = {};
					popupData.field.permissions.canRead = [];
					popupData.field.permissions.canUpdate = [];
				}
				popupData.field.permissions[permission].push(roleId);
			}

		}



		//Currency
		if (popupData.field.fieldType == 3) {
			popupData.selectedCurrencyMeta = contentData.currencyMetas[0].code;
			if (popupData.field.currency != null && popupData.field.currency != {} && popupData.field.currency.code) {
				for (var i = 0; i < contentData.currencyMetas.length; i++) {
					if (popupData.field.currency.code == contentData.currencyMetas[i].code) {
						popupData.selectedCurrencyMeta = contentData.currencyMetas[i].code;
					}
				}
			}
		}

		//Identifier GUID field specific functions
		popupData.uniqueGuidGenerateCheckboxEnabled = true;
		popupData.defaultValueTextboxEnabled = true;
		popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
		popupData.defaultValueTextboxValue = null;
		popupData.uniqueGuidGenerateToggle = function (newValue) {
			if (newValue) { // if it is checked
				popupData.defaultValueTextboxEnabled = false;
				popupData.defaultValueTextboxPlaceholder = "will be auto-generated";
				popupData.defaultValueTextboxValue = popupData.field.defaultValue;
				popupData.field.defaultValue = null;
			}
			else {
				popupData.defaultValueTextboxEnabled = true;
				popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
				popupData.field.defaultValue = popupData.defaultValueTextboxValue;
				popupData.defaultValueTextboxValue = null;
			}
		}

		popupData.uniqueGuidPropertyChecked = function (newValue) {
			if (newValue) {
				popupData.field.generateNewId = true;
				popupData.uniqueGuidGenerateCheckboxEnabled = false;
				popupData.uniqueGuidGenerateToggle(true);
			}
			else {
				popupData.field.generateNewId = false;
				popupData.uniqueGuidGenerateCheckboxEnabled = true;
				popupData.uniqueGuidGenerateToggle(false);
			}
		}




		/////
		popupData.calendars = {};
		popupData.openCalendar = function (event, name) {
			popupData.calendars[name] = true;
		}


		popupData.ok = function () {
			switch (popupData.field.fieldType) {
				case 3:
					for (var i = 0; i < contentData.currencyMetas.length; i++) {
						if (contentData.currencyMetas[i].code === popupData.selectedCurrencyMeta) {
							popupData.field.currency.symbol = contentData.currencyMetas[i].symbol;
							popupData.field.currency.symbolNative = contentData.currencyMetas[i].symbol_native;
							popupData.field.currency.name = contentData.currencyMetas[i].name;
							popupData.field.currency.namePlural = contentData.currencyMetas[i].name_plural;
							popupData.field.currency.code = contentData.currencyMetas[i].code;
							popupData.field.currency.decimalDigits = contentData.currencyMetas[i].decimal_digits;
							popupData.field.currency.rounding = contentData.currencyMetas[i].rounding;
							popupData.field.currency.symbolPlacement = 1;
						}
					}
					break;
				case 4: //Date
					if(popupData.field.defaultValue!=null) {
						popupData.field.defaultValue = moment(popupData.field.defaultValue).startOf('day').utc().toISOString();
					}
					break;
				case 5: //Date & Time
					if(popupData.field.defaultValue!=null) {
						popupData.field.defaultValue = moment(popupData.field.defaultValue).startOf('minute').utc().toISOString();
					}
					break;
			}
			webvellaAdminService.updateField(popupData.field, popupData.contentData.entity.id, successCallback, errorCallback);
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		//Delete field
		//Create new field modal
		popupData.deleteFieldModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteFieldModal.html',
				controller: 'DeleteFieldModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					parentPopupData: function () { return popupData; }
				}
			});
		}

		//Dropdown or Multiselect
		popupData.addDropdownValue = function () {
			var option = {
				key: null,
				value: null
			};
			popupData.field.options.push(option);
		}

		popupData.deleteDropdownValue = function (index) {
			var defaultKeyIndex = -1;
			if (popupData.isKeyDefault(popupData.field.options[index].key)) {
				for (var j = 0; j < popupData.field.defaultValue.length; j++) {
					if (popupData.field.defaultValue[j] !== popupData.field.options[index].key) {
						defaultKeyIndex = j;
						break;
					}
				}
			}
			popupData.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
			popupData.field.options.splice(index, 1); // remove from options
		}

		popupData.toggleKeyAsDefault = function (key) {
			if (popupData.field.defaultValue.indexOf(key) > -1) {
				//There could be multiple keys with the same key
				var cleanArray = [];
				for (var j = 0; j < popupData.field.defaultValue.length; j++) {
					if (popupData.field.defaultValue[j] !== key) {
						cleanArray.push(popupData.field.defaultValue[j]);
					}
				}
				popupData.field.defaultValue = fastCopy(cleanArray);
			} else {
				popupData.field.defaultValue.push(key);
			}
		}

		popupData.isKeyDefault = function (key) {
			if (popupData.field.defaultValue.indexOf(key) > -1) {
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
			$modalInstance.close('success');
			webvellaRootService.GoToState($state, $state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.field, location);
		}
		$log.debug('webvellaAdmin>entities>ManageFieldModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	//// Modal Controllers
	DeleteFieldModalController.$inject = ['parentPopupData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
	function DeleteFieldModalController(parentPopupData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.parentData = parentPopupData;

		popupData.ok = function () {
			webvellaAdminService.deleteField(popupData.parentData.field.id, popupData.parentData.contentData.entity.id, successCallback, errorCallback);
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$modalInstance.close('success');
			popupData.parentData.modalInstance.close('success');
			$timeout(function () {
				$state.go("webvella-admin-entity-fields", { name: popupData.parentData.contentData.entity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupData.hasError = true;
			popupData.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


})();

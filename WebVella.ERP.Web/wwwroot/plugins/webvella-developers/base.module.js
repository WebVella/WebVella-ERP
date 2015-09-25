/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
    'use strict';

    angular
        .module('webvellaDevelopers', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDevelopersBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-developers-base', {
            //abstract: true,
            url: '/developers', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaDevelopersBaseController',
                    templateUrl: '/plugins/webvella-developers/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                }
            },
            data: {}
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory', 'Upload'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopBrowsenavFactory, upload) {
    	$log.debug('webvellaDevelopers>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));
        $rootScope.$on('webvellaDesktop-browsenav-ready', function (event) {
            var item = {
                "label": "Developers",
                "stateName": "webvella-developers-base",
                "stateParams": {},
                "parentName": "",
                "nodes": [],
                "weight": 101.0,
                "color": "purple",
                "iconName": "code"
            };
			//If item is admin
            webvellaDesktopBrowsenavFactory.addItem(item);
        });
        $log.debug('webvellaDevelopers>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$scope', '$timeout', 'webvellaDevelopersQueryService', 'Upload'];

    /* @ngInject */
    function controller($log, $scope, $timeout, queryService, Upload) {
    	$log.debug('webvellaDevelopers>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var pluginData = this;
        pluginData.executeSampleQuery = executeSampleQuery;
        pluginData.createSampleQueryDataStructure = createSampleQueryDataStructure;
        pluginData.executeSampleRelationRecordUpdate = executeSampleRelationRecordUpdate;
        pluginData.result = "";

        $log.debug('webvellaDevelopers>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

        $scope.$watch('pluginData.files', function () {
            pluginData.upload(pluginData.files);
        });

        pluginData.upload = function (files) {
            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    $log.info(file);
                    Upload.upload({
                        url: 'http://localhost:2202/fs/upload/',
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                        $log.info(progressPercentage + '%');
                    }).success(function (data, status, headers, config) {
                        $timeout(function () {
                            //$scope.log = 'file: ' + config.file.name + ', Response: ' + angular.toJson(data) + '\n' + $scope.log;
                            //$log.info(data);
                            //$log.info(status);
                            //$log.info(headers);
                            //$log.info(config);
                            queryService.moveFile({ 'source': data.object.url, 'target': "/test/test.pdf", overwrite: true },
                                function (response) {
                                    $log.info('webvellaDevelopers>base> END controller.moveFile> SUCCESS');
                                    pluginData.result = response;

                                    queryService.deleteFile("/fs/test/test.pdf",
                                        function (response) {
                                            $log.info('webvellaDevelopers>base> END controller.deleteFile> SUCCESS');
                                            $log.info(response);
                                            pluginData.result = response;
                                        },
                                        function (response) {
                                            $log.info('webvellaDevelopers>base> END controller.deleteFile> ERROR');
                                            $log.info(response);
                                            pluginData.result = response;
                                        });


                                },
				                function (response) {
				                    $log.info('webvellaDevelopers>base> END controller.moveFile> ERROR');
				                    $log.info(response);
				                    pluginData.result = response;
				                });

                        });
                    });
                }
            }
        };

        function activate() {
        }


        function executeSampleRelationRecordUpdate() {
        	$log.debug('webvellaDevelopers>base> BEGIN controller.executeSampleRelationRecordUpdate ' + moment().format('HH:mm:ss SSSS'));
            queryService.executeSampleRelationRecordUpdate({ relationName: "account_id_to_test_user_id", originFieldRecordId: '8e253615-d91c-4c60-91ca-c13d3c7c6053', targetFieldRecordIds: ['1af61222-986c-403c-a1e6-738c986312f0'], operation: "create" },
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleRelationRecordUpdate> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleRelationRecordUpdate> ERROR ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				}
			);

        }

        function executeSampleQuery() {
        	$log.debug('webvellaDevelopers>base> BEGIN controller.executeSampleQuery ' + moment().format('HH:mm:ss SSSS'));
            queryService.executeSampleQuery({},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> ERROR ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				}
			);

        }

        function createSampleQueryDataStructure() {
        	$log.debug('webvellaDevelopers>base> BEGIN controller.createSampleQueryDataStructure ' + moment().format('HH:mm:ss SSSS'));
            queryService.createSampleQueryDataStructure({},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> ERROR ' + moment().format('HH:mm:ss SSSS'));
				    $log.debug(response);
				    pluginData.result = response;
				}
			);
        }





    }

})();

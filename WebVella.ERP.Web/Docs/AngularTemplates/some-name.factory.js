/* some-name.factory.js */

/**
* @desc just a sample factory code
*/

 (function () {
            'use strict';
            angular
                .module('app')
                .factory('someNameFactory', factory);

            factory.$inject = ['dependencies'];

            /* @ngInject */
            function factory(dependencies){
                var exports = {
                    func: func
                };
                //Some code

                return exports;

                ////////////////

                function func() {
                }
            }
        })();
/* some-name.factory.js */

/**
* @desc just a sample factory code
*/

 (function () {
            'use strict';
            angular
                .module('appCore')
                .factory('someNameFactory', factory);

            factory.$inject = ['dependencies'];

            
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
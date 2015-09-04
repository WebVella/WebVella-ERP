describe('ng-ckeditor', function () {
    CKEDITOR_BASEPATH = '/base/ckeditor/';
    var elm, scope, controller, content, instance, i = 1;

    beforeEach(module('ngCkeditor'));

    beforeEach(inject(function ($rootScope, $compile, $document) {
        elm = angular.element(
            '<form name="testForm">{{test}}<div><textarea id="' + 'editor' + i + '" ckeditor ng-model="test"></textarea></div></form>');

        scope = $rootScope.$new(true);

        scope.test = 'test';

        $document.find('body').append(elm);

        $compile(elm)(scope);
        controller = elm.controller('ngModel');
        instance = CKEDITOR.instances['editor' + i++];
        instance.on('instanceReady', function() {
            content = angular.element(instance.container.$.getElementsByClassName('cke_wysiwyg_div')[0]);
        });

        scope.$digest();
    }));

    afterEach( function() {
        elm.remove();
    } );

    it('should create editor', inject(function ($timeout, $rootScope, $document) {
        var flag;
        runs(function() {
            expect(CKEDITOR.instances['editor' + (i - 1)]).toBeDefined();

            expect(scope.test).toBe('test');
            setTimeout(function() {
                flag = true;
            }, 500)
        });
        waitsFor(function() {
            return flag;
        }, "instance should be ready", 1000);
    }));

    it('should change model value editor', inject(function ($timeout, $rootScope, $document) {
        var flag;
        runs(function() {
            flag = false;
            scope.test = 'new value';
            $rootScope.$apply();

            setTimeout(function() {
                // instance not ready
                expect(instance.getData()).toBe('');
                expect(scope.test).toBe('new value');
                expect(scope.testForm.$dirty).toBe(false);
            }, 10);

            instance.on('instanceReady', function() {
                $timeout.flush();
                expect(content.html()).toBe('<p>new value</p>');
                expect(instance.getData()).toBe('<p>new value</p>\n');
                expect(scope.test).toBe('<p>new value</p>\n');
                expect(scope.testForm.$dirty).toBe(false);
                flag = true;
            });
        });
        waitsFor(function() {
            return flag;
        }, "instance should be ready", 1000);
    }));

    it('should async change model value editor', inject(function ($timeout, $rootScope, $document) {
        var flag;
        runs(function() {
            flag = false;
            scope.test = 'new value';

            setTimeout(function() {
                scope.test = 'test again';
                scope.$apply();
                expect(scope.test).toBe('test again');
            }, 1000);

            setTimeout(function() {
                content[0].focus();

                // emulate key press
                $(content[0]).simulate("click")
                            .simulate("keydown", {keyCode : 13, charCode: 0})
                            .simulate("keypress", {keyCode : 13, charCode: 0})
                            .simulate("keyup", {keyCode : 13, charCode: 0});
                $timeout.flush();
            }, 1100);

            setTimeout(function() {
                scope.$apply();
                flag = true;
                expect(scope.test).toBe('<p>&nbsp;</p>\n\n<p>test again</p>\n');
                expect(scope.testForm.$dirty).toBe(true);
            }, 1200);
        });
        waitsFor(function() {
            return flag;
        }, "value should be changed", 2000);
    }));

    it('should change in source view', inject(function ($timeout, $rootScope, $document) {
        var flag;
        runs(function() {
            flag = false;

            setTimeout(function() {
                $('.cke_button__source').simulate("click")
                // emulate key press
                $('.cke_source').simulate("click")
                            .simulate("keydown", {keyCode : 13, charCode: 0})
                            .simulate("keypress", {keyCode : 13, charCode: 0})
                            .simulate("keyup", {keyCode : 13, charCode: 0});
                $timeout.flush();
            }, 100);

            setTimeout(function() {
                scope.$apply();
                flag = true;
                expect(scope.test).toBe('<p>test</p>\n');
            }, 200);
        });
        waitsFor(function() {
            return flag;
        }, "value should be changed", 1000);
    }));
});

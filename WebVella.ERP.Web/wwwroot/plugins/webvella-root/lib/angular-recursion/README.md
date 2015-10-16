# angular-recursion

A service which makes it easy possible to have recursive Angular directives.

## Why
When an Angular directive calls itself, Angular gets into an endless loop. This service provides the logic needed to work around this.

## Installation
1. `bower install angular-recursion`.
2. Include `bower_components/angular-recursion/angular-recursion.min.js`.
3. Add the `RecursionHelper` module as a dependency.
4. Inject the `RecursionHelper` service and use it.

## Usage
Inject the `RecursionHelper` service into your directive, and use it in the `compile` function, as shown in the example below. The example is also available as [a Plunker](http://plnkr.co/edit/JAIyolmqPqO9KsynSiZp?p=preview), so you can see it running.

``` javascript
angular.module('myModule', ['RecursionHelper']).directive("tree", function(RecursionHelper) {
    return {
        restrict: "E",
        scope: {family: '='},
        template: 
        '<p>{{ family.name }}{{test }}</p>'+
            '<ul>' + 
                '<li ng-repeat="child in family.children">' + 
                    '<tree family="child"></tree>' +
                '</li>' +
            '</ul>',
        compile: function(element) {
            return RecursionHelper.compile(element, function(scope, iElement, iAttrs, controller, transcludeFn){
                // Define your normal link function here.
                // Alternative: instead of passing a function,
                // you can also pass an object with 
                // a 'pre'- and 'post'-link function.
            });
        }
    };
});
```

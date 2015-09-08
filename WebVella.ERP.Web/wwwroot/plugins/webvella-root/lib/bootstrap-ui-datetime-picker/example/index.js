var app = angular.module('app', ['ui.bootstrap', 'ui.bootstrap.datetimepicker']);

app.controller('MyController', ['$scope', function($scope) {

    var that = this;

    var in10Days = new Date();
    in10Days.setDate(in10Days.getDate() + 10);

    this.dates = {
        date1: new Date('01 Mar 2015 00:00:00.000'),
        date2: new Date(),
        date3: new Date(),
        date4: new Date(),
        date5: in10Days,
        date6: new Date(),
        date7: new Date(),
        date8: new Date()
    };

    this.open = {
        date1: false,
        date2: false,
        date3: false,
        date4: false,
        date5: false,
        date6: false,
        date7: false,
        date8: false
    };

    // Disable weekend selection
    this.disabled = function(date, mode) {
        return (mode === 'day' && (new Date().toDateString() == date.toDateString()));
    };

    this.dateOptions = {
        showWeeks: false,
        startingDay: 1
    };

    this.timeOptions = {
        readonlyInput: false,
        showMeridian: false
    };

    this.dateModeOptions = {
        minMode: 'year',
        maxMode: 'year'
    };

    this.openCalendar = function(e, date) {
        e.preventDefault();
        e.stopPropagation();

        that.open[date] = true;
    };

    // watch date4 and date5 to calculate difference
    this.calculateWatch = $scope.$watch(function() {
        return that.dates;
    }, function() {
        if (that.dates.date4 && that.dates.date5) {
            var diff = that.dates.date4.getTime() - that.dates.date5.getTime();
            that.dayRange = Math.round(Math.abs(diff/(1000*60*60*24)))
        } else {
            that.dayRange = 'n/a';
        }
    }, true);

    $scope.$on('$destroy', function() {
        that.calculateWatch();
    });
}]);
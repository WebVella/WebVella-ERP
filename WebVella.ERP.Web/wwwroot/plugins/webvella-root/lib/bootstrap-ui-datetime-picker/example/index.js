var app = angular.module('app', ['ui.bootstrap', 'ui.bootstrap.datetimepicker']);

app.controller('MyController', ['$scope', function($scope) {

    var that = this;

    var in10Days = new Date();
    in10Days.setDate(in10Days.getDate() + 10);

    this.dates = {
        date1: new Date('2015-03-01T00:00:00Z'),
        date2: new Date('2015-03-01T12:30:00Z'),
        date3: new Date(),
        date4: new Date(),
        date5: in10Days,
        date6: new Date(),
        date7: new Date(),
        date8: new Date(),
        date9: null,
        date10: new Date('2015-03-01T09:00:00Z'),
        date11: new Date('2015-03-01T10:00:00Z'),
        date12: new Date('2015-03-01T10:00:00Z'),
        date13: null,
        date14: new Date('2016-02-16T10:00:00Z'),
        date15: new Date('2016-02-16T10:00:00Z'),
        date16: new Date('2016-02-16T10:00:00Z'),
        date17: new Date('2016-02-16T10:00:00Z'),
        date18: new Date('2016-02-16T10:00:00Z'),
        date19: new Date('2016-02-16T10:00:00Z'),
    };

    this.open = {
        date1: false,
        date2: false,
        date3: false,
        date4: false,
        date5: false,
        date6: false,
        date7: false,
        date8: false,
        date9: false,
        date10: false,
        date11: false,
        date12: false,
        date13: false,
        date14: false,
        date15: false,
        date16: false,
        date17: false,
        date18: false,
        date19: false
    };

    // Disable today selection
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

    this.buttonBar = {
        show: true,
        now: {
            show: true,
            text: 'Now!'
        },
        today: {
            show: true,
            text: 'Today!'
        },
        clear: {
            show: false,
            text: 'Wipe'
        },
        date: {
            show: true,
            text: 'Date'
        },
        time: {
            show: true,
            text: 'Time'
        },
        close: {
            show: true,
            text: 'Shut'
        }
    };

    this.onClosed = function(args) {
        that.closedArgs = args;
    };

    this.openCalendar = function(e, date) {
        that.open[date] = true;
    };

    // watch date4 and date5 to calculate difference
    var unwatch = $scope.$watch(function() {
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
        unwatch();
    });
}]);
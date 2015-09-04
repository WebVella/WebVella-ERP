// Karma configuration file
// See http://karma-runner.github.io/0.10/config/configuration-file.html
module.exports = function(config) {
  config.set({
    basePath: '',

    frameworks: ['jasmine'],

    // list of files / patterns to load in the browser
    files: [
      // libraries
      'bower_components/angular/angular.js',
      'bower_components/angular-mocks/angular-mocks.js',
      'bower_components/jquery/jquery.js',
      'bower_components/jquery.simulate/libs/bililiteRange.js',
      'bower_components/jquery.simulate/libs/jquery.simulate.js',

      'libs/ckeditor/*.js',
      'libs/ckeditor/skins/**/*.css',
      'libs/ckeditor/lang/**/*.js',
      'libs/ckeditor/plugins/**/*.js',


      // directive
      'ng-ckeditor.min.js',

      // tests
      'test/*.js'
      //'test/tableParamsSpec.js'
      //'test/tableControllerSpec.js'
    ],

    // generate js files from html templates
    preprocessors: {
      '*.js': 'coverage'
    },

    reporters: ['progress', 'coverage'],

    autoWatch: true,
    browsers: ['Chrome'],
    coverageReporter: {
        type: 'lcov',
        dir : 'out/coverage'
    }
  });
};

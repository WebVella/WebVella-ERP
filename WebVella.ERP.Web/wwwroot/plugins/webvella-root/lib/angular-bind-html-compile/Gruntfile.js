'use strict';

module.exports = function (grunt) {
    require('time-grunt')(grunt);
    require('load-grunt-tasks')(grunt);

    grunt.initConfig({
        // Configurable paths
        config: {
            lintFiles: [
                '**/*.js',
                '!*.min.js'
            ]
        },
        jshint: {
            options: {
                jshintrc: '.jshintrc'
            },
            all: [
                '<%= config.lintFiles %>'
            ]
        },
        jscs: {
            options: {
                config: '.jscsrc'
            },
            src: '<%= config.lintFiles %>'
        },
        uglify: {
            build: {
                files: {
                    'angular-bind-html-compile.min.js': 'angular-bind-html-compile.js'
                }
            }
        }
    });

    grunt.registerTask('lint', [
        'jshint',
        'jscs'
    ]);

    grunt.registerTask('test', [
        'lint'
    ]);

    grunt.registerTask('build', [
        'uglify'
    ]);
};

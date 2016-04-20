
module.exports = function (grunt) {
    grunt.initConfig({
        cmpnt: grunt.file.readJSON('bower.json'),
        banner: '/*! ngCkeditor v<%= cmpnt.version %> by Vitalii Savchuk(esvit666@gmail.com) - ' +
            'https://github.com/esvit/ng-ckeditor - New BSD License */\n',
        clean: {
            working: {
                src: ['ng-ckeditor.*', './.temp/']
            }
        },
        copy: {
            styles: {
                files: [
                    {
                        src: './src/styles/ng-ckeditor.css',
                        dest: './ng-ckeditor.css'
                    }
                ]
            }
        },
        uglify: {
            js: {
                src: ['ng-ckeditor.js'],
                dest: 'ng-ckeditor.min.js',
                options: {
                    banner: '<%= banner %>',
                    sourceMap: function (fileName) {
                        return fileName.replace(/\.js$/, '.map');
                    }
                }
            }
        },
        concat: {
            js: {
                src: ['src/scripts/*.js'],
                dest: 'ng-ckeditor.js'
            }
        },
        less: {
            css: {
                files: {
                    'ng-ckeditor.css': 'src/styles/ng-ckeditor.less'
                }
            }
        },
        cssmin: {
            css: {
                files: {
                    'ng-ckeditor.css': 'ng-ckeditor.css'
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.registerTask('dev', ['clean', 'concat', 'less', 'copy']);

    return grunt.registerTask('default', ['dev', 'uglify', 'cssmin']);
};
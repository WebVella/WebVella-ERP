// conf.js
exports.config = {
    seleniumAddress: 'http://localhost:4444/wd/hub',
    capabilities: {'browserName': 'firefox' },
    // Options to be passed to Jasmine-node.
    jasmineNodeOpts: { showColors: true},
    // A callback function called once protractor is ready and available, and
    // before the specs are executed
    // You can specify a file containing code to run by setting onPrepare to
    // the filename string.
    onPrepare: function () {
        // you can also add properties to globals here
        browser.erpSiteAddress = 'http://localhost:3945';
    },
    specs: ['../app_test.js',
            '../dashboard/dashboard_test.js',
            '../navigation/navigation_test.js'
    ],
}
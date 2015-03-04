'use strict';

/* https://github.com/angular/protractor/blob/master/docs/toc.md */

describe('my app', function () {

    browser.get('http://localhost:3945/');

    it('should automatically redirect to /dashboard when location hash/fragment is empty', function () {
        expect(browser.getLocationAbsUrl()).toMatch("/dashboard");
    });
});

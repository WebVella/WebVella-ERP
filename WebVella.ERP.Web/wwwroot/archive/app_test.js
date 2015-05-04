'use strict';

/* https://github.com/angular/protractor/blob/master/docs/toc.md */

describe('Application General Functions Test', function () {

    browser.get(browser.erpSiteAddress);

    it('should automatically redirect to /dashboard when location hash/fragment is empty', function () {
        expect(browser.getLocationAbsUrl()).toMatch("/dashboard");
    });
});

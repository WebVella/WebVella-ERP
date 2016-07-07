#moment-angular-date-format-parser
[![Build Status](https://travis-ci.org/wmluke/moment-angular-date-format-parser.svg?branch=master)](https://travis-ci.org/wmluke/moment-angular-date-format-parser)

This `moment.js` plugin translates the [Angular JS filter date format](https://docs.angularjs.org/api/ng/filter/date) to the `moment.js` date format.

I took the structure graciously from <https://github.com/wmluke/moment-jdateformatparser> (thanks a lot!).

Usage
=====
* `formatWithADF`: Formats the moment with a angular date format.
  > e.g.: `moment("2013-12-24 14:30").formatWithADF("dd.MM.yyyy")` will return `24.12.2013`

* `toMomentFormatString`: Translates the angular date format to a momentjs format.
  > e.g.: `moment().toMomentFormatString("dd.MM.yyyy")` will return `DD.MM.YYYY`

* `toADFString`: Translates the momentjs format to a angular date format.
  > e.g.: `moment().toADFString("DD.MM.YYYY")` will return `dd.MM.yyyy`

Installation
============

Bower
------
Installation is possible with `bower install moment-angular-date-format-parser`


License
=======
moment-angular-date-format-parser is freely distributable under the terms of the MIT license.

Copyright (c) 2013 Heinz Romirer, Martin Groller

# angular-bind-html-compile
This repo contains a bower package that provides an angular directive which can be passed trusted html with angular template content to evaluate.

The `bind-html-compile` directive allows for HTML containing directives to be compiled.

You should only use this directive where the content is coming from a trusted
source.

## Install
Install via [bower](http://bower.io)

* `bower install angular-bind-html-compile`

Add dependency to your app module

* `'angular-bind-html-compile'`

## Usage 
`ng-bind-html`:
```
<div ng-bind-html="data.content"></div>
```

If the `data.content` contained a directive, it would not be compiled.

`bind-html-compile`:
```
<div bind-html-compile="data.content"></div>
```

## Development
* Contributions welcome - Create an issue to discuss proposed changes and additions
* All contributions should be done in branches and submitted as pull requests.
* Code style follows the [jsHint](http://jshint.com/docs/) and [jscs](http://jscs.info/) rules in `.jshintrc` and `jscsrc`. Automated travis tests will fail if these are not adhered to.

## Releasing a new version

1. Commit your changes.
1. Follow the guidelines at http://semver.org/ to determine your new version number.
1. Update `CHANGELOG.md` with your new version number and a description of changes.
1. Update the `version` property in `bower.json` and `package.json`
1. Commit those changes with the commit message "Bump to [version number]". [version number] should be in the format x.y.z.
1. `git tag [version number]`
1. `git push`
1. `git push --tags` - must be done separately.

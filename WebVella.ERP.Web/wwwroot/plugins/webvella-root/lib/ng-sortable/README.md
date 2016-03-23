
ng-sortable
==============

Angular Library for Drag and Drop, supports Sortable and Draggable. No JQuery UI used. Supports Touch devices.

If you use this module you can give it a thumbs up at [http://ngmodules.org/modules/ng-sortable](http://ngmodules.org/modules/ng-sortable).

#### Release:

Latest release version 1.3.1 
[Module name is modified from 'ui.sortable' to 'as.sortable' from versions 1.3.x,
considering the conflict with the sortable module from bootstrap-ui.]

#### Demo Page:

[Simple] (http://a5hik.github.io/ng-sortable/plunker.html)

[Advanced] (http://a5hik.github.io/ng-sortable/)

Demo Includes:

- Drag between adjacent Lists.
- Control Drag on Specific Destinations.

#### Features:

- Drag both Horizontally and Vertically.
- Drag and Drop items within a column.
- Drag and Drop items across columns.
- Can do Ranking by Sorting and Change Status by Moving.
- Hooks provided to invoke API's after a particular action.
- Preventing/Allowing Drop Zone can be determined at run time.
- Enable/Disable Drag at run time.
- Drag Boundary can be defined.
- Clone an item and drop.

#### Implementation Details:

- Uses angular/native JS for sortable and draggable. no JQueryUI used.
- Provides callbacks for drag/drop events.
- Implementation follows Prototypical scope inheritance.

#### Directives structure:

The directives are structured like below.

    as-sortable                     --> Items list
      as-sortable-item              --> Item to sort/drag
        as-sortable-item-handle     --> Drag Handle

#### Design details:

- ng-model is used to bind the sortable list items with the sortable element.
- as-sortable can be added to the root element.
- as-sortable-item can be added in item element, and follows ng-repeat.
- as-sortable-item-handle can be added to the drag handle in item element.
- All as-sortable, ng-model, as-sortable-item and as-sortable-item-handle are required.
- the 'no-drag' attribute can be added to an element to prevent dragging inside as-sortable-item-handle.
  allows you to perform the element specific event but prevent the element being dragged.
- the drag item handle can listen for custom events as well.
- Added a Jquery like 'containment' option to the sortable to prevent the drag outside specified bounds.
- 'containerPositioning' option may be set to 'relative' to accommodate relative positioning on the container or its ancestry. Use this if the draggable item is offset from the mouse cursor while dragging. A common scenario for this is when using bootstrap columns.
- The 'is-disabled' attribute can be added optionally to as-sortable disable the Drag at runTime.

#### Placeholder:
- By default a placeholder element is created using the same tag as the as-sortable-item element
- CSS styling may be applied via the `as-sortable-placeholder` class
- Additional classes may be applied via the `additionalPlaceholderClass` option provided to the as-sortable item. e.g.
    in JS:
```$scope.sortableOptions = { additionalPlaceholderClass: 'some-class' };```
    in HTML:
```<div as-sortable="sortableOptions">
   ...
</div>```
- A customized placeholder can be provided by specifying the `placeholder` option provided to the as-sortable item. `placeholder` can be both a template string or a function returning a template string.

#### Dragging element CSS
- CSS styling may be applied via the "as-sortable-dragging" class
- When the "as-sortable-item" is being dragged, the CSS class "as-sortable-dragging" is added to all elements.
e.g.
    in HTML
```<!--not dragging-->````
```<div class="as-sortable-item"></div>````
```<!--when dragging as-sortable-item-->```
```<div class="as-sortable-item as-sortable-dragging"></div>````

    in CSS
    .as-sortable-dragging{
       border: 1px dotted #000 !important;
    }
          

#### Callbacks:

Following callbacks are defined, and should be overridden to perform custom logic.

- callbacks.accept = function (sourceItemHandleScope, destSortableScope, destItemScope) {}; //used to determine drag zone is allowed are not.

###### Parameters:
     sourceItemScope - the scope of the item being dragged.
     destScope - the sortable destination scope, the list.
     destItemScope - the destination item scope, this is an optional Param.(Must check for undefined).

- callbacks.orderChanged = function({type: Object}) // triggered when item order is changed with in the same column.
- callbacks.itemMoved = function({type: Object}) // triggered when an item is moved accross columns.
- callbacks.dragStart = function({type: Object}) // triggered on drag start.
- callbacks.dragEnd = function({type: Object}) // triggered on drag end.

##### Parameters:
    Object (event) - structure         
             source:
                  index: original index before move.
                  itemScope: original item scope before move.
                  sortableScope: original sortable list scope.
             dest: index
                  index: index after move.
                  sortableScope: destination sortable scope.  
                  
##### Some Notable Fixes:

- Touch is allowed on only one Item at a time. Tap is prevented on draggable item.
- Pressing 'Esc' key will Cancel the Drag Event, and moves back the Item to it's Original location.
- Right Click on mouse is prevented on draggable Item.
- A child element inside a draggable Item can be made as non draggable.

##### Usage:

Get the binaries of ng-sortable with any of the following ways.

```sh
bower install ng-sortable
```
Or for yeoman with bower automatic include:
```
bower install ng-sortable -save
```
Or bower.json
```
{
  "dependencies": [..., "ng-sortable: "latest_version eg - "1.1.0" ", ...],
}
```
npm install ng-sortable
```
Make sure to load the scripts in your html.
```html
<script type="text/javascript" src="dist/ng-sortable.min.js"></script>
<link rel="stylesheet" type="text/css" href="dist/ng-sortable.min.css">

<!-- OPTIONAL: default style -->
<link rel="stylesheet" type="text/css" href="dist/ng-sortable.style.min.css">
```

And Inject the sortable module as dependency.

```
angular.module('xyzApp', ['as.sortable', '....']);
```

##### Html Structure:

Invoke the Directives using below html structure.

    <ul data-as-sortable="board.dragControlListeners" data-ng-model="items">
       <li data-ng-repeat="item in items" data-as-sortable-item>
          <div data-as-sortable-item-handle></div>
       </li>
    </ul>

Define your callbacks in the invoking controller.

    $scope.dragControlListeners = {
        accept: function (sourceItemHandleScope, destSortableScope) {return boolean}//override to determine drag is allowed or not. default is true.
        itemMoved: function (event) {//Do what you want},
        orderChanged: function(event) {//Do what you want},
        containment: '#board'//optional param.
        clone: true //optional param for clone feature.
    };
    
That's what all you have to do.

##### Restrict Moving between Columns:

Define the accept callback. and the implementation is your choice.
The itemHandleScope(dragged Item) and sortableScope(destination list) is exposed. 
Compare the scope Objects there like below., If you have to exactly restrict moving between columns.

    accept: function (sourceItemHandleScope, destSortableScope) {
      return sourceItemHandleScope.itemScope.sortableScope.$id === destSortableScope.$id;
    }

If you want to restrict only to certain columns say you have 5 columns and you want 
the drag to be allowed in only 3 columns, then you need to implement your custom logic there.,
and that too becomes straight forward as you have your scope Objects in hand.

And reversing the condition, allows you to Drag across Columns but not within same Column.

##### How To Revert Move After Validation Failure:

In case you want the item to be reverted back to its original location after a validation failure
You can just do the below.
In your itemMoved call back define a 'moveSuccess' and 'moveFailure' callbacks.
The move failure Impl here just reverts the moved item to its original location.

    itemMoved: function (eventObj) {

    var moveSuccess, moveFailure;
          /**
           * Action to perform after move success.
           */
          moveSuccess = function() {};

          /**
           * Action to perform on move failure.
           * remove the item from destination Column.
           * insert the item again in original Column.
           */
          moveFailure = function() {   
               eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
               eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.task);
          };
    }


##### Horizontal Sorting:

Horizontal Drag and Drop can be achieved using the same Library. The Column display can be tweaked to have horizontal items and the same can be achieved via some CSS tweaks (like making the column display style to "inline-block"). Added a sample in the demo source (refer plunker.css/js/html).

Plunkr example link: http://plnkr.co/edit/OcaMzBV3c0K3CL1nw9L4?p=preview

##### Scroll page after reaching end of visible area.

Implement dragMove callback and follow https://github.com/a5hik/ng-sortable/issues/13#issuecomment-120388981

##### Enable/Disable Drag at Runtime:

The Drag can be controlled at runtime and you can enable/disable it by setting the "is-disabled" property to true or false.

    <div as-sortable is-disabled="true">..</div>

##### Testing:

- Tested on FireFox, IE 9 and Greater, Chrome and Safari.
- Ipad, Iphone and Android devices.

##### Development Environment setup:

Clone the master 
    $ git clone https://github.com/a5hik/ng-sortable.git

or Download from [Source Master](https://github.com/a5hik/ng-sortable/archive/master.zip)

##### Installation:

##### Development Dependencies (Grunt and Bower):

Install Grunt and Bower.
    $ sudo npm install -g grunt-cli bower

##### Install Project dependencies:
Run the following commands from the project root directory.

    $ npm install
    $ bower install

##### Commands to run:
    $ grunt build - builds the source and generates the min files in dist.
    $ grunt server - runs a local web server on node.js
    $ grunt test - runs the tests (WIP).
    $ grunt test:continuous - end to end test (WIP).

To access the local server, enter the following URL into your web browser:

    http://localhost:9009/demo/
    
##### NG Modules Link:

If you use this module you can give it a thumbs up at [http://ngmodules.org/modules/ng-sortable](http://ngmodules.org/modules/ng-sortable).

Let [me](https://github.com/a5hik) know if you have any questions.

For Bug report, and feature request File an Issue here: [issue](https://github.com/a5hik/ng-sortable/issues).

##### License

MIT, see [LICENSE](./LICENSE).

/* globals: beforeEach, describe, it, module, inject, expect */
describe('RecursionHelper', function(){
	var $compile, parent, scope, link;

	angular.module('Tree', []).directive("tree", function(RecursionHelper){
		return {
			restrict: "E",
			scope: {
				family: '='
			},
			replace: true,
			template: '' +
				'<div class="tree">' +
				'	<p>{{ family.name }}</p>' +
				'	<ul>' +
				'		<li ng-repeat="child in family.children">' +
				'			<tree family="child"></tree>' +
				'		</li>' +
				'	</ul>' +
				'</div>',
			compile: function(element){
				return RecursionHelper.compile(element, link);
			}
		};
	});

    beforeEach(module('RecursionHelper', 'Tree'));
    beforeEach(inject(function(_$compile_, $rootScope){
    	$compile = _$compile_;
		scope = $rootScope.$new();
		scope.treeFamily = {
			name: "Parent",
			children: [
				{
					name: "Child1",
					children: [
						{
							name: "Grandchild1",
							children: []
						},
						{
							name: "Grandchild2",
							children: []
						},
						{
							name: "Grandchild3",
							children: []
						}
					]
				},
				{
					name: "Child2",
					children: []
				}
			]
		};
    }));

	function compileTree(){
		parent = $compile('<tree family="treeFamily"></tree>')(scope);
		scope.$apply();
	}


   	it('should render the whole tree', function(){
		compileTree();
		var children = parent.children('ul').children();
		var grandChildren = children.find(':first-child').children('ul').children();
		expect(children.length).toBe(2);
		expect(grandChildren.length).toBe(3);
	});

	it('should call the pre and post linking functions, when passed as object in the link parameter', function(){
		link = {
			pre: jasmine.createSpy('pre'),
			post: jasmine.createSpy('post')
		};

		compileTree();
		expect(link.pre).toHaveBeenCalled();
		expect(link.post).toHaveBeenCalled();

	});

	it('should call the post linking function, when passed as function in the link parameter', function(){
		link = jasmine.createSpy('post');

		compileTree();
		expect(link).toHaveBeenCalled();
	});
});
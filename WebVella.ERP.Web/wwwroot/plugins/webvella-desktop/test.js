var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Person = (function () {
    function Person(name, age) {
        this.name = name;
        this.age = age;
    }
    return Person;
})();
var Man = (function (_super) {
    __extends(Man, _super);
    function Man(name, age) {
        _super.call(this, name, age);
    }
    Man.prototype.getDescription = function () {
        return this.name + " is" + this.age + " years old";
    };
    return Man;
})(Person);
var lee = new Man("Lee", 34);

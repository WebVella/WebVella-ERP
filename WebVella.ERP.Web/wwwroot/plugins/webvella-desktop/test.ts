class Person{
	constructor(public name:string, public age:number){}
}

class Man extends Person{
	constructor(name:string, age:number){
		super(name,age);
	}

	getDescription():string{
		return this.name + " is" + this.age + " years old";
	}
}

var lee:Man = new Man("Lee",34);
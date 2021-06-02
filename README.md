# WebAPITodoList
RESTful API for a TODO-list with the basic CRUD operations.

The tool used to development is Visual Studio 2019.

The solution is divided into six projects:
	
	- .Net Core Web API.
		- CRUD methods for ToDoList and AppUser entities.
		
		- Using a JSON file or LiteDB NoSQL as database.
		
		- In 'UseDB' parameter on appsettings.json, you can configure which database to use, possible values ("JsonFile" or "LiteDB").
		
	- Core class library
		- Business entities and interfaces.
	
	- Dto class library
		- Entities that are exposed by the web API, using the pattern data transfer object.
	
	- LiteDB class library
		- Implements the connection to LiteDB noSQL database.
	
	- JsonFile class library
		- Implements the connection to json file as database.
		
	- Tests class library
		- Unit tests for all web API methods.

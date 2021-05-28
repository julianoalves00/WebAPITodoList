# WebAPITodoList
RESTful API for a TODO-list with the basic CRUD operations.

Visual Studio 2019 was used as a development tool.

The solution is divided into six projects:
	
	- .Net Core Web API.
		- Using a JSON file or LiteDB NoSQL as database.
		
		- In appsettings.json configure UseDB param to select the database to use, possibles values ("JsonFile" or "LiteDB")
		
	- Core class library
		- Bussines entities and interfaces
	
	- Dto class library
		- Entities that are exposed by the web API
	
	- LiteDB class library
		- Implements the connection to LiteDB 
	
	- JsonFile class library
		- Implements the connection to json file as database
		
	- Tests class library
		- Unit tests for all web API methods.

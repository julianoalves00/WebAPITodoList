# WebAPITodoList
RESTful API for a TODO-list with the basic CRUD operations.

Visual Studio 2019 was used to develop this, the solution is divided into six projects
	- .Net Core Web API.
		- Using a JSON file or LiteDB NoSQL as database.
		- In appsettings.json configure UseDB param to select the database to use
			- "UseDB" : "JsonFile"
			or
			- "UseDB" : "LiteDB"
		
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

# IEIndexAPI

Perform the following tasks before running the application for the first time:

## Database tasks

	1. Create empty database called "IEIndex"
	2. Create a login called "ieindex", password "ieindex"
	3. Map "IEIndex" database to "ieindex" login
	4. Assign the following database-level roles to "ieindexusr" on "IEIndex"
		* db_datareader
		* db_datawriter
		* db_ddladmin
		* public

## Visual Studio tasks

	1. Set the DataLayer project as the startup project
	2. Open the Package Manager Console and make sure "DataLayer" is selected in the "Default Project" dropdown
	3. Run the following commands:
		1. enable-migrations (skip this if you get a warning that "Migrations have already been enabled...")
		2. add-migration Initial
		3. update-database
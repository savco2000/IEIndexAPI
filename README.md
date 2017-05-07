# IEIndexAPI

Perform the following tasks before running the application for the first time:

## Database tasks

Run the SQL script below to create the needed logins, database, users and mapping of roles to the created user.
		
```sql
USE master;  
GO  

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'IEIndex')
CREATE DATABASE IEIndex  
ON   
( NAME = IEIndex_dat,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\ieindex.mdf',  
    SIZE = 10,  
    MAXSIZE = 50,  
    FILEGROWTH = 5 )  
LOG ON  
( NAME = IEIndex_log,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\ieindex_log.ldf',  
    SIZE = 5MB,  
    MAXSIZE = 25MB,  
    FILEGROWTH = 5MB ) ;  
GO 

IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'ieindex')
BEGIN
    CREATE LOGIN ieindex WITH PASSWORD = 'ieindex'
END

GO

USE [IEIndex];

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ieindex')
BEGIN
    CREATE USER [ieindex] FOR LOGIN [ieindex]
    EXEC sp_addrolemember N'db_owner', N'ieindex'	
	EXEC sp_addrolemember N'db_datareader', N'ieindex'
	EXEC sp_addrolemember N'db_datawriter', N'ieindex'
	EXEC sp_addrolemember N'db_ddladmin', N'ieindex'
END;
GO
```

## Visual Studio tasks

	1. Set the DataLayer project as the startup project
	2. Open the Package Manager Console and make sure "DataLayer" is selected in the "Default Project" dropdown
	3. Run the following commands:
		a. enable-migrations (skip this if you get a warning that "Migrations have already been enabled...")
		b. add-migration Initial
		c. update-database
IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'db_Cars')
    CREATE DATABASE db_Cars

GO
-- Creates the login 
CREATE LOGIN db_Cars_backend_user
    WITH PASSWORD = '1983Cars!!';  
GO  

-- Creates a database user for the login created above.  
CREATE USER db_Cars_backend_user FOR LOGIN db_Cars_backend_user;  
GO  


ALTER ROLE db_owner ADD MEMBER [db_Cars_backend_user];


GO

/*
 DROP TABLE dbo.Orders
 DROP TABLE dbo.users
*/

use db_Cars
GO

CREATE TABLE dbo.Users (
 userID INT IDENTITY(1,1) PRIMARY KEY,
  UserName VARCHAR(50),
  Email VARCHAR(100),
  Password VARCHAR(50),
  Created_AT DATETIME DEFAULT GETDATE(),
  IS_Deleted DATE   DEFAULT NULL
);

insert into dbo.Users values ( 'admin', 'ljscodex@gmail.com', '', null, null)

GO

Create TABLE dbo.Cars (
    carID     INT IDENTITY(1,1) PRIMARY KEY,
    carBrand         VARCHAR(50),
    carModel         VARCHAR(50),
    carIsWorking     BIT DEFAULT 'TRUE',
    userID           int,
    FOREIGN KEY (userID) REFERENCES dbo.Users(userID)
)



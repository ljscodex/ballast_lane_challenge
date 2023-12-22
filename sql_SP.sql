use db_Cars

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_Create')
    DROP PROCEDURE sp_Cars_Create

GO

CREATE PROCEDURE sp_Cars_Create
    @CarBrand    varchar(50),
    @CarModel      varchar(50),
    @CarIsWorking  BIT,
    @UserID          int
AS

BEGIN
  SET NOCOUNT ON;

    begin TRY
        INSERT INTO dbo.Cars values ( @CarBrand, @CarModel, @CarIsWorking, @UserID)
        SELECT SCOPE_IDENTITY() as Result
    end TRY
    begin CATCH 
        select -1 as Result
    end catch
END

GO

GRANT EXECUTE ON sp_Cars_Create 
    TO db_Cars_backend_user;  
GO 

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_Update')
    DROP PROCEDURE sp_Cars_Update

GO

CREATE PROCEDURE sp_Cars_Update
    @CarID      INT,
    @CarBrand    varchar(50),
    @CarModel      varchar(50),
    @CarIsWorking  BIT,
    @UserID          int
AS

BEGIN
  SET NOCOUNT ON;

    begin TRY
        UPDATE dbo.Cars 
            SET 
            carBrand= @CarBrand, 
            carModel = @CarModel,
            carIsWorking = @CarIsWorking,
            userID = @UserID
            Where carID = @CarID
    end TRY
    begin CATCH 
        select -1 as Result
    end catch
END

GO

GRANT EXECUTE ON sp_Cars_Update
    TO db_Cars_backend_user;  
GO 

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_CheckifExists')
    DROP PROCEDURE sp_Cars_CheckifExists

GO


CREATE PROCEDURE sp_Cars_CheckifExists
    @CarID   int    
AS

BEGIN
  SET NOCOUNT ON;

    begin try
        if exists ( select 1 from dbo.Cars where carID = @CarID)
          begin  
            select 1 as Result
            END
        else 
            BEGIN   
                select 0 as Result
            END
    end TRY
    begin CATCH 
        select -1 as Result
    end catch

END

GO

GRANT EXECUTE ON sp_Cars_CheckifExists
    TO db_Cars_backend_user;  
GO 


GO 

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_Delete')
    DROP PROCEDURE sp_Cars_Delete

GO


CREATE PROCEDURE sp_Cars_Delete
    @CarID   int    
AS

BEGIN
    
    DELETE FROM dbo.Cars where carID = @CarID

END

GO

GRANT EXECUTE ON sp_Cars_Delete
    TO db_Cars_backend_user;  
GO 

/*
// Replaced by DRY Support on the Backend API ( CarsAPI )
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_List')
    DROP PROCEDURE sp_Cars_List

GO

CREATE PROCEDURE sp_Cars_List
AS

BEGIN
  SET NOCOUNT ON;

  SELECT c.* , u.UserName from dbo.Cars c
        inner join dbo.users u on u.userid = c.userid
END

GO

GRANT EXECUTE ON sp_Cars_List
    TO db_Cars_backend_user;  
GO 
*/

GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Cars_Search')
    DROP PROCEDURE sp_Cars_Search

GO

CREATE PROCEDURE sp_Cars_Search
    @CarID    INT  = null
AS

BEGIN
  SET NOCOUNT ON;

    begin try
        if @CarID is not null
            BEGIN
                SELECT c.* , u.UserName from dbo.Cars c
                    inner join dbo.users u on u.userid = c.userid
                 where c.CarID = @CarID

            END
        ELSE    
            BEGIN
                SELECT c.* , u.UserName from dbo.Cars c
                    inner join dbo.users u on u.userid = c.userid
            END
    end TRY
    begin CATCH 
        select -1 as Result
    end catch

END

GO

GRANT EXECUTE ON sp_Cars_Search
    TO db_Cars_backend_user;  
GO 
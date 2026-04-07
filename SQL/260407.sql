CREATE OR ALTER PROC SP_Packages_GetByOrderId
@OrderId BIGINT 
AS
BEGIN
	SELECT P.*
	  FROM [dbo].[Packages] P
	WHERE P.OrderId = @OrderId
	AND IsDeleted = 0
END

GO


 
CREATE OR ALTER   PROCEDURE [dbo].[SP_Orders_GetDetailById]
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    
    SELECT 
        o.*,
		cw.Name CNWarehouseName,
		vw.Name VNWarehouseName

    FROM Orders o
	LEFT JOIN Warehouses cw ON o.CNWarehouseId = cw.Id
	LEFT JOIN Warehouses vw ON o.VNWarehouseId = vw.Id
    WHERE 
	o.Id = @Id AND
	o.IsDeleted = 0
   
END
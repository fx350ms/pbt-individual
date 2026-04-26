 

CREATE OR ALTER   PROC [dbo].[SP_Packages_GetForCreateNewDeliveryRequest]
@Status INT = -1,
@CustomerId INT = -1,
@WarehouseId INT = -1
AS
BEGIN
	SELECT p.PackageNumber,
	p.BagNumber,
	p.WaybillNumber,
	w.Name WarehouseName
	FROM Packages p
	LEFT JOIN Warehouses w ON p.WarehouseDestinationId = w.Id
	WHERE p.IsDeleted = 0
	AND (p.ShippingStatus = @Status )
	AND (P.CustomerId = @CustomerId)
	AND (@WarehouseId = -1 OR P.WarehouseId = @WarehouseId)

	
END
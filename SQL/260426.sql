 

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

GO

CREATE OR ALTER   PROCEDURE SP_Packages_GetPagedByCustomerId
(
    @CustomerId INT = -1,
    @SkipCount INT = 0,
	@MaxResultCount INT = 50,
    @TotalCount INT OUTPUT,
    @TotalWeight DECIMAL(18,2) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE  @FilteredPackages TABLE
    (
        Id BIGINT PRIMARY KEY,
      
        Weight DECIMAL(18,2)
    );

    -------------------------------------------------------------------
    -- ĐỔ TOÀN BỘ DỮ LIỆU FILTER VÀO #FilteredPackages
    -------------------------------------------------------------------
    INSERT INTO @FilteredPackages
	    SELECT
		p.Id,
		ISNULL(p.Weight, 0) AS Weight
		FROM Packages p
	 
		WHERE 1 = 1
		AND p.IsDeleted = 0
        AND (p.CustomerId = @CustomerId)
         
    -------------------------------------------------------------------
    -- TÍNH TOTAL COUNT + WEIGHT
    -------------------------------------------------------------------
    SELECT 
        @TotalCount = COUNT(*),
        @TotalWeight = ISNULL(SUM(ISNULL(Weight,0)),0)
    FROM @FilteredPackages;
	 
        SELECT 
            p.Id,
			p.PackageNumber,
			p.WaybillNumber,
			p.ShippingStatus,
            w.Name AS WarehouseName
        FROM Packages p
        JOIN @FilteredPackages f on p.Id = f.Id
        LEFT JOIN Warehouses w ON w.Id = p.WarehouseId
        ORDER BY f.Id DESC
        OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
   

END


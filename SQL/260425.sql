 
 
CREATE OR ALTER     PROCEDURE [dbo].[SP_DeliveryRequests_GetPaged]
    @PermissionCase INT = 1,
    @CustomerIds NVARCHAR(MAX) = N'',
    @CustomerId BIGINT = -1,
    @WarehouseId INT = -1,
    @FromCreationTime DATETIME2(7) = NULL,
    @ToCreationTime DATETIME2(7) = NULL,
    @Keyword NVARCHAR(200) = N'',
    @Status INT = -1,
    @SkipCount INT = 0,
    @MaxResultCount INT = 20,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(@SkipCount, 0) < 0
        SET @SkipCount = 0;

    IF ISNULL(@MaxResultCount, 0) <= 0
        SET @MaxResultCount = 20;

    CREATE TABLE #TblCustomerIds
    (
        Id BIGINT NOT NULL PRIMARY KEY
    );

    IF ISNULL(@CustomerIds, N'') <> N''
    BEGIN
        INSERT INTO #TblCustomerIds(Id)
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(value)) AS BIGINT)
        FROM STRING_SPLIT(@CustomerIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(value)) AS BIGINT) IS NOT NULL;
    END

    CREATE TABLE #FilteredDeliveryRequestIds
    (
        Id BIGINT NOT NULL PRIMARY KEY
    );

    INSERT INTO #FilteredDeliveryRequestIds(Id)
    SELECT p.Id
    FROM dbo.DeliveryRequests p
    WHERE (p.IsDeleted = 0 OR p.IsDeleted IS NULL)

        /* Permission */
        AND
        (
            @PermissionCase IN (1,5,6)
            OR
            (
                @PermissionCase IN (2,3,4,7)
                AND p.CustomerId IS NOT NULL
                AND EXISTS
                (
                    SELECT 1
                    FROM #TblCustomerIds t
                    WHERE t.Id = p.CustomerId
                )
            )
        )

        /* Filter theo CustomerId */
        AND (@CustomerId = -1 OR p.CustomerId = @CustomerId)

        /* Filter theo WarehouseId */
        AND (@WarehouseId = -1 OR p.WarehouseId = @WarehouseId)

        /* Filter theo Status */
        AND (@Status = -1 OR p.Status = @Status)

        /* Filter theo CreationTime */
        AND (@FromCreationTime IS NULL OR p.CreationTime >= @FromCreationTime)
        AND (@ToCreationTime IS NULL OR p.CreationTime <= @ToCreationTime)

        /* Filter theo Keyword */
        AND
        (
            @Keyword = N''
            OR p.RequestCode LIKE N'%' + @Keyword + N'%'
        
        );

    SELECT @TotalCount = COUNT(1)
    FROM #FilteredDeliveryRequestIds;

    SELECT
        d.*,
		w.Name AS WarehouseName,
		c.Username CustomerName
    FROM #FilteredDeliveryRequestIds f
	
    INNER JOIN dbo.DeliveryRequests d ON d.Id = f.Id
	LEFT JOIN Warehouses w ON w.Id = d.WarehouseId
	LEFT JOIN Customers c ON d.CustomerId  = c.Id 
    ORDER BY d.Id DESC
    OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
END

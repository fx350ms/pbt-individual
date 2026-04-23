CREATE PROCEDURE [dbo].[SP_Orders_CreateQuicklyOrder]
    -- Input parameters
    @WaybillNumber NVARCHAR(MAX),
    @CreatorUserId BIGINT,
    -- Output parameter
    @NewOrderId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
BEGIN TRY
INSERT INTO [dbo].[Orders] (
            WaybillNumber,
            ShippingLine,
            CreationTime,
            CreatorUserId,
            IsDeleted
        )
        VALUES (
            @WaybillNumber,
            0,
            GETDATE(),
            @CreatorUserId,
            0 
        );
        SET @NewOrderId = SCOPE_IDENTITY();
SELECT * FROM Orders where id = @NewOrderId

END TRY
BEGIN CATCH
SET @NewOrderId = -1; 
        THROW;
END CATCH
END
go


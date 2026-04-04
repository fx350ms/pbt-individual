SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
    Purpose:
    - Create customer record from registered user information
    - Return created/existing customer id via @CustomerId OUTPUT
*/ 
CREATE OR ALTER PROCEDURE [dbo].[SP_Customers_CreateByUser]
    @UserId BIGINT,
    @Username NVARCHAR(256) = NULL,
    @CustomerCode NVARCHAR(256) = NULL,
    @FullName NVARCHAR(256) = NULL,
    @Email NVARCHAR(256) = NULL,
    @PhoneNumber NVARCHAR(50) = NULL,
    @WarehouseId INT = NULL,
    @CustomerId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF (@UserId IS NULL OR @UserId <= 0)
    BEGIN
        THROW 50001, 'UserId is required.', 1;
    END;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
        DECLARE @ResolvedCustomerCode NVARCHAR(256) = NULLIF(LTRIM(RTRIM(@CustomerCode)), '');

        IF (@ResolvedCustomerCode IS NULL)
            SET @ResolvedCustomerCode = NULLIF(LTRIM(RTRIM(@Username)), '');

        IF (@ResolvedCustomerCode IS NULL)
            SET @ResolvedCustomerCode = CONCAT('CUS', @UserId);

        SELECT TOP (1)
            @CustomerId = [Id]
        FROM [dbo].[Customers]
        WHERE [UserId] = @UserId
          AND ([IsDeleted] = 0 OR [IsDeleted] IS NULL)
        ORDER BY [Id] DESC;

        IF (@CustomerId IS NOT NULL AND @CustomerId > 0)
        BEGIN
            UPDATE [dbo].[Customers]
            SET [Username] = COALESCE(NULLIF(LTRIM(RTRIM(@Username)), ''), [Username]),
                [CustomerCode] = COALESCE(NULLIF(LTRIM(RTRIM(@ResolvedCustomerCode)), ''), [CustomerCode]),
                [FullName] = COALESCE(NULLIF(LTRIM(RTRIM(@FullName)), ''), [FullName]),
                [Email] = COALESCE(NULLIF(LTRIM(RTRIM(@Email)), ''), [Email]),
                [PhoneNumber] = COALESCE(NULLIF(LTRIM(RTRIM(@PhoneNumber)), ''), [PhoneNumber]),
                [WarehouseId] = COALESCE(@WarehouseId, [WarehouseId]),
                [LastModificationTime] = @Now
            WHERE [Id] = @CustomerId;
        END
        ELSE
        BEGIN
            INSERT INTO [dbo].[Customers]
            (
                [CustomerCode],
                [FullName],
                [Email],
                [PhoneNumber],
                [Address],
                [AddressReceipt],
                [DateOfBirth],
                [Gender],
                [RegistrationDate],
                [Status],
                [Notes],
                [RefCode],
                [PiorityStorageId],
                [UserId],
                [Username],
                [AddressId],
                [AddressReceiptId],
                [CurrentAmount],
                [CurrentDebt],
                [MaxDebt],
                [VipLevel],
                [LineId],
                [PotentialLevel],
                [IsAgent],
                [AgentLevel],
                [ParentId],
                [SaleId],
                [WarehouseId],
                [InsurancePercentage],
                [BagPrefix],
                [CreationTime],
                [IsDeleted]
            )
            VALUES
            (
                @ResolvedCustomerCode,
                COALESCE(NULLIF(LTRIM(RTRIM(@FullName)), ''), NULLIF(LTRIM(RTRIM(@Username)), ''), @ResolvedCustomerCode),
                NULLIF(LTRIM(RTRIM(@Email)), ''),
                NULLIF(LTRIM(RTRIM(@PhoneNumber)), ''),
                N'',
                N'',
                NULL,
                N'',
                @Now,
                1,
                N'',
                N'',
                NULL,
                @UserId,
                COALESCE(NULLIF(LTRIM(RTRIM(@Username)), ''), @ResolvedCustomerCode),
                NULL,
                NULL,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                @WarehouseId,
                0,
                NULL,
                @Now,
                0
            );

            SET @CustomerId = CAST(SCOPE_IDENTITY() AS BIGINT);
        END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_AbpUsers_Register]
    @TenantId INT,
    @Name NVARCHAR(64),
    @PhoneNumber NVARCHAR(32),
    @EmailAddress NVARCHAR(256),
    @UserName NVARCHAR(256),
    @Password NVARCHAR(128),
    @WarehouseId INT = NULL,
    -- Output params
    @UserId BIGINT OUTPUT,
    @CustomerId BIGINT OUTPUT,
    @Status INT OUTPUT,
    @Msg NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CurrentDate DATETIME2(7) = GETDATE();

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. TẠO TÀI KHOẢN (AbpUsers)
        INSERT INTO [dbo].[AbpUsers] (
            [TenantId], [Name], [Surname], [PhoneNumber], [EmailAddress], 
            [UserName], [Password], [IsEmailConfirmed], [IsActive], [IsDeleted], 
            [CreationTime], [NormalizedEmailAddress], [NormalizedUserName], 
            [SecurityStamp], [ConcurrencyStamp], [AccessFailedCount], 
            [IsLockoutEnabled], [IsPhoneNumberConfirmed], [IsTwoFactorEnabled]
        )
        VALUES (
            @TenantId, @Name, N'', @PhoneNumber, @EmailAddress, 
            @UserName, @Password, 1, 1, 0, 
            @CurrentDate, UPPER(@EmailAddress), UPPER(@UserName),      
            REPLACE(CAST(NEWID() AS NVARCHAR(36)), '-', ''), 
            CAST(NEWID() AS NVARCHAR(36)), 0, 1, 0, 0
        );

        SET @UserId = SCOPE_IDENTITY();

        -- 2. GÁN ROLE MẶC ĐỊNH (AbpUserRoles)
        INSERT INTO [dbo].[AbpUserRoles] ([TenantId], [UserId], [RoleId], [CreationTime])
        SELECT @TenantId, @UserId, [Id], @CurrentDate
        FROM [dbo].[AbpRoles]
        WHERE [IsDefault] = 1 AND [IsDeleted] = 0
          AND ([TenantId] = @TenantId OR (@TenantId IS NULL AND [TenantId] IS NULL));

        -- 3. TẠO KHÁCH HÀNG (Customers)
        INSERT INTO [dbo].[Customers] (
            [CustomerCode], [FullName], [Email], [PhoneNumber], [Address], 
            [AddressReceipt], [RegistrationDate], [Status], [UserId], [Username], 
            [WarehouseId], [CreationTime], [IsDeleted], [CurrentAmount], [CurrentDebt], 
            [MaxDebt], [VipLevel], [LineId], [PotentialLevel], [IsAgent], [AgentLevel], 
            [ParentId], [SaleId], [InsurancePercentage]
        )
        VALUES (
            @UserName, @Name, NULLIF(LTRIM(RTRIM(@EmailAddress)), ''), 
            NULLIF(LTRIM(RTRIM(@PhoneNumber)), ''), N'', N'', @CurrentDate, 1, 
            @UserId, @UserName, @WarehouseId, @CurrentDate, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        );

        SET @CustomerId = SCOPE_IDENTITY();

        -- 4. GỌI SP LIÊN KẾT (SP_Customers_LinkToAccount)
        -- Lưu ý: Gọi trong Transaction để đảm bảo tính nhất quán
        IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'SP_Customers_LinkToAccount' AND type = 'P')
        BEGIN
            EXEC [dbo].[SP_Customers_LinkToAccount] @CustomerId = @CustomerId, @Username = @UserName;
        END

        COMMIT TRANSACTION;

        SET @Status = 1;
        SET @Msg = N'Đăng ký tài khoản và tạo hồ sơ khách hàng thành công';
        
        -- Trả về thông tin ID vừa tạo
        SELECT @UserId AS UserId, @CustomerId AS CustomerId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        SET @Status = 0;
        SET @Msg = ERROR_MESSAGE();
        
        SET @UserId = 0;
        SET @CustomerId = 0;

        SELECT 0 AS UserId, 0 AS CustomerId;
    END CATCH
END
GO
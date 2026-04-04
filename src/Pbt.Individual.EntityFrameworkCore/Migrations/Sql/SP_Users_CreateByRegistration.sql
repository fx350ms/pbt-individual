SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
    Purpose:
    - Create an Abp user record for self-registration
    - Return created user id via @UserId OUTPUT
*/
CREATE OR ALTER PROCEDURE [dbo].[SP_Users_CreateByRegistration]
    @TenantId INT = NULL,
    @UserName NVARCHAR(32),
    @NormalizedUserName NVARCHAR(32),
    @Name NVARCHAR(32),
    @Surname NVARCHAR(32),
    @EmailAddress NVARCHAR(256),
    @NormalizedEmailAddress NVARCHAR(256),
    @PhoneNumber NVARCHAR(MAX) = NULL,
    @Password NVARCHAR(128),
    @SecurityStamp NVARCHAR(MAX) = NULL,
    @ConcurrencyStamp NVARCHAR(MAX) = NULL,
    @IsActive BIT,
    @IsEmailConfirmed BIT,
    @IsLockoutEnabled BIT = 0,
    @IsPhoneNumberConfirmed BIT = 0,
    @IsTwoFactorEnabled BIT = 0,
    @CreationTime DATETIME2(7),
    @IsDeleted BIT = 0,
    @AccessFailedCount INT = 0,
    @UserId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF (@UserName IS NULL OR LTRIM(RTRIM(@UserName)) = '')
        THROW 50001, 'UserName is required.', 1;

    IF (@EmailAddress IS NULL OR LTRIM(RTRIM(@EmailAddress)) = '')
        THROW 50002, 'EmailAddress is required.', 1;

    IF (@Password IS NULL OR LTRIM(RTRIM(@Password)) = '')
        THROW 50003, 'Password is required.', 1;

    IF (@NormalizedUserName IS NULL OR LTRIM(RTRIM(@NormalizedUserName)) = '')
        THROW 50004, 'NormalizedUserName is required.', 1;

    IF (@NormalizedEmailAddress IS NULL OR LTRIM(RTRIM(@NormalizedEmailAddress)) = '')
        THROW 50005, 'NormalizedEmailAddress is required.', 1;

    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (
            SELECT 1
            FROM [dbo].[AbpUsers]
            WHERE ISNULL([TenantId], -1) = ISNULL(@TenantId, -1)
              AND (
                    [NormalizedUserName] = @NormalizedUserName
                    OR [NormalizedEmailAddress] = @NormalizedEmailAddress
                  )
        )
        BEGIN
            THROW 50006, 'User already exists for this tenant.', 1;
        END;

        INSERT INTO [dbo].[AbpUsers]
        (
            [AccessFailedCount],
            [AuthenticationSource],
            [ConcurrencyStamp],
            [CreationTime],
            [CreatorUserId],
            [DeleterUserId],
            [DeletionTime],
            [EmailAddress],
            [EmailConfirmationCode],
            [IsActive],
            [IsDeleted],
            [IsEmailConfirmed],
            [IsLockoutEnabled],
            [IsPhoneNumberConfirmed],
            [IsTwoFactorEnabled],
            [LastLoginTime],
            [LastModificationTime],
            [LastModifierUserId],
            [LockoutEndDateUtc],
            [Name],
            [NormalizedEmailAddress],
            [NormalizedUserName],
            [Password],
            [PasswordResetCode],
            [PhoneNumber],
            [SecurityStamp],
            [Surname],
            [TenantId],
            [UserName]
        )
        VALUES
        (
            @AccessFailedCount,
            NULL,
            COALESCE(NULLIF(LTRIM(RTRIM(@ConcurrencyStamp)), ''), CONVERT(NVARCHAR(36), NEWID())),
            @CreationTime,
            NULL,
            NULL,
            NULL,
            @EmailAddress,
            NULL,
            @IsActive,
            @IsDeleted,
            @IsEmailConfirmed,
            @IsLockoutEnabled,
            @IsPhoneNumberConfirmed,
            @IsTwoFactorEnabled,
            NULL,
            NULL,
            NULL,
            NULL,
            @Name,
            @NormalizedEmailAddress,
            @NormalizedUserName,
            @Password,
            NULL,
            @PhoneNumber,
            COALESCE(NULLIF(LTRIM(RTRIM(@SecurityStamp)), ''), CONVERT(NVARCHAR(36), NEWID())),
            @Surname,
            @TenantId,
            @UserName
        );

        SET @UserId = CAST(SCOPE_IDENTITY() AS BIGINT);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO

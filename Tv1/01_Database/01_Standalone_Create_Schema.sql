IF DB_ID(N'WholesaleManagementDB') IS NULL
BEGIN
    CREATE DATABASE WholesaleManagementDB;
END
GO
USE WholesaleManagementDB;
GO
IF OBJECT_ID(N'dbo.Item',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Item (
        ItemID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Item PRIMARY KEY,
        ItemName NVARCHAR(120) NOT NULL,
        Size NVARCHAR(40) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        StockQuantity INT NOT NULL,
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT CK_Item_Price CHECK (Price >= 0),
        CONSTRAINT CK_Item_Stock CHECK (StockQuantity >= 0),
        CONSTRAINT CK_Item_Name CHECK (LEN(LTRIM(RTRIM(ItemName))) > 0)
    );
END
GO
IF OBJECT_ID(N'dbo.Agent',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Agent (
        AgentID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Agent PRIMARY KEY,
        AgentName NVARCHAR(120) NOT NULL,
        Address NVARCHAR(250) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        CONSTRAINT CK_Agent_Name CHECK (LEN(LTRIM(RTRIM(AgentName))) > 0)
    );
END
GO
IF OBJECT_ID(N'dbo.[Order]',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Order] (
        OrderID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Order PRIMARY KEY,
        OrderDate DATETIME2(0) NOT NULL CONSTRAINT DF_Order_Date DEFAULT SYSUTCDATETIME(),
        AgentID INT NOT NULL,
        OrderStatus NVARCHAR(20) NOT NULL CONSTRAINT DF_Order_Status DEFAULT N'Pending',
        CONSTRAINT FK_Order_Agent FOREIGN KEY(AgentID) REFERENCES dbo.Agent(AgentID) ON DELETE NO ACTION,
        CONSTRAINT CK_Order_Status CHECK (OrderStatus IN (N'Pending',N'Shipped'))
    );
END
GO
IF OBJECT_ID(N'dbo.OrderDetail',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderDetail (
        ID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrderDetail PRIMARY KEY,
        OrderID INT NOT NULL,
        ItemID INT NOT NULL,
        Quantity INT NOT NULL,
        UnitAmount DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_Detail_Order FOREIGN KEY(OrderID) REFERENCES dbo.[Order](OrderID) ON DELETE NO ACTION,
        CONSTRAINT FK_Detail_Item FOREIGN KEY(ItemID) REFERENCES dbo.Item(ItemID) ON DELETE NO ACTION,
        CONSTRAINT UQ_Detail_Order_Item UNIQUE(OrderID,ItemID),
        CONSTRAINT CK_Detail_Qty CHECK (Quantity > 0),
        CONSTRAINT CK_Detail_UnitAmount CHECK (UnitAmount >= 0)
    );
END
GO
IF OBJECT_ID(N'dbo.Users',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        UserID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        UserName NVARCHAR(80) NOT NULL,
        Email NVARCHAR(160) NOT NULL,
        [Password] NVARCHAR(255) NOT NULL,
        [Lock] BIT NOT NULL CONSTRAINT DF_Users_Lock DEFAULT(0),
        [Role] NVARCHAR(12) NOT NULL,
        AgentID INT NULL,
        FailedLoginCount INT NOT NULL CONSTRAINT DF_Users_FailCount DEFAULT(0),
        LastLoginAtUtc DATETIME2(0) NULL,
        CONSTRAINT UQ_Users_UserName UNIQUE(UserName),
        CONSTRAINT UQ_Users_Email UNIQUE(Email),
        CONSTRAINT FK_Users_Agent FOREIGN KEY(AgentID) REFERENCES dbo.Agent(AgentID) ON DELETE NO ACTION,
        CONSTRAINT CK_Users_Role CHECK ([Role] IN (N'Admin',N'Agent')),
        CONSTRAINT CK_Users_RoleAgent CHECK (([Role]=N'Admin' AND AgentID IS NULL) OR ([Role]=N'Agent' AND AgentID IS NOT NULL)),
        CONSTRAINT CK_Users_FailCount CHECK (FailedLoginCount >= 0)
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'UX_Users_AgentID_Active' AND object_id=OBJECT_ID(N'dbo.Users'))
    CREATE UNIQUE INDEX UX_Users_AgentID_Active ON dbo.Users(AgentID) WHERE AgentID IS NOT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Order_Agent_Date' AND object_id=OBJECT_ID(N'dbo.[Order]'))
    CREATE INDEX IX_Order_Agent_Date ON dbo.[Order](AgentID,OrderDate) INCLUDE(OrderStatus);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Order_Status_Date' AND object_id=OBJECT_ID(N'dbo.[Order]'))
    CREATE INDEX IX_Order_Status_Date ON dbo.[Order](OrderStatus,OrderDate);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Detail_Item' AND object_id=OBJECT_ID(N'dbo.OrderDetail'))
    CREATE INDEX IX_Detail_Item ON dbo.OrderDetail(ItemID);
GO

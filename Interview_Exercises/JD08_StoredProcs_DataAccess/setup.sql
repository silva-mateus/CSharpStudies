-- ============================================================
-- JD08 - Stored Procedures and Data Access
-- Setup: Schema + Seed Data for SQL Server LocalDB
-- ============================================================
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'JD08_InventoryDB')
BEGIN
    ALTER DATABASE JD08_InventoryDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE JD08_InventoryDB;
END
GO

CREATE DATABASE JD08_InventoryDB;
GO

USE JD08_InventoryDB;
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    StockQuantity INT NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE INDEX IX_Products_Category ON Products(Category);

-- Seed data
INSERT INTO Products (Name, Category, Price, StockQuantity) VALUES
('Wireless Mouse',       'Electronics', 29.99,  150),
('Mechanical Keyboard',  'Electronics', 89.99,   75),
('USB-C Hub',            'Electronics', 45.00,  200),
('27" Monitor',          'Electronics', 349.99,  40),
('Standing Desk',        'Furniture',  499.99,   20),
('Monitor Arm',          'Furniture',   79.99,   60),
('Ergonomic Chair',      'Furniture',  349.99,   30),
('Notebook Pack',        'Stationery',  12.99,  500),
('Whiteboard Markers',   'Stationery',   8.49,  300),
('Sticky Notes',         'Stationery',   5.99,  800);

PRINT 'JD08_InventoryDB created and seeded successfully.';
GO

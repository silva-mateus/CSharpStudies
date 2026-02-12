-- ============================================================
-- JD02 - SQL Querying Fundamentals
-- Schema and Seed Data for SQL Server LocalDB
-- ============================================================
-- Run this script against (localdb)\MSSQLLocalDB to set up the exercise database.

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'JD02_QueryExercise')
BEGIN
    ALTER DATABASE JD02_QueryExercise SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE JD02_QueryExercise;
END
GO

CREATE DATABASE JD02_QueryExercise;
GO

USE JD02_QueryExercise;
GO

-- ============================================================
-- TABLES
-- ============================================================

CREATE TABLE Departments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Budget DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Salary DECIMAL(18,2) NOT NULL CHECK (Salary > 0),
    HireDate DATE NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id),
    ManagerId INT NULL FOREIGN KEY REFERENCES Employees(Id)
);

CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    City NVARCHAR(100) NULL,
    Country NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    StockQuantity INT NOT NULL DEFAULT 0
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers(Id),
    OrderDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'Shipped', 'Delivered', 'Cancelled'))
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0)
);

-- Indexes for common query patterns
CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);

-- ============================================================
-- SEED DATA
-- ============================================================

-- Departments
INSERT INTO Departments (Name, Budget) VALUES
('Engineering', 500000),
('Marketing', 200000),
('Sales', 300000),
('HR', 150000),
('Finance', 250000);

-- Employees (ManagerId set after initial inserts)
SET IDENTITY_INSERT Employees ON;
INSERT INTO Employees (Id, FirstName, LastName, Email, Salary, HireDate, DepartmentId, ManagerId) VALUES
(1,  'Alice',   'Johnson',  'alice.johnson@company.com',   130000, '2016-06-18', 1, NULL),
(2,  'Bob',     'Smith',    'bob.smith@company.com',       110000, '2020-07-01', 1, 1),
(3,  'Carol',   'White',    'carol.white@company.com',      95000, '2023-01-10', 1, 1),
(4,  'David',   'Brown',    'david.brown@company.com',      85000, '2018-11-20', 2, NULL),
(5,  'Eve',     'Davis',    'eve.davis@company.com',        92000, '2021-05-05', 2, 4),
(6,  'Frank',   'Miller',   'frank.miller@company.com',     78000, '2022-08-14', 3, NULL),
(7,  'Grace',   'Wilson',   'grace.wilson@company.com',    105000, '2017-02-28', 3, 6),
(8,  'Henry',   'Taylor',   'henry.taylor@company.com',     72000, '2020-09-03', 4, NULL),
(9,  'Ivy',     'Anderson', 'ivy.anderson@company.com',     68000, '2024-04-22', 4, 8),
(10, 'Jack',    'Thomas',   'jack.thomas@company.com',     115000, '2019-08-30', 1, 1),
(11, 'Karen',   'Lee',      'karen.lee@company.com',        88000, '2023-06-12', 2, 4),
(12, 'Leo',     'Martinez', 'leo.martinez@company.com',     95000, '2020-12-01', 3, 6),
(13, 'Mia',     'Garcia',   'mia.garcia@company.com',       75000, '2022-03-17', 4, 8),
(14, 'Nathan',  'Clark',    'nathan.clark@company.com',    102000, '2021-09-25', 5, NULL),
(15, 'Olivia',  'Moore',    'olivia.moore@company.com',     97000, '2019-04-10', 5, 14);
SET IDENTITY_INSERT Employees OFF;

-- Customers
INSERT INTO Customers (Name, Email, City, Country, CreatedAt) VALUES
('Acme Corp',       'orders@acme.com',       'New York',   'USA',     '2023-01-15'),
('GlobalTech',      'buy@globaltech.com',    'London',     'UK',      '2023-03-20'),
('StartupXYZ',      'info@startupxyz.com',   'Berlin',     'Germany', '2023-06-01'),
('MegaRetail',      'purchase@megaretail.com','Tokyo',      'Japan',   '2024-01-10'),
('LocalShop',       'orders@localshop.com',  'Chicago',    'USA',     '2024-03-15'),
('SilentBuyer Co',  'contact@silentbuyer.com','Paris',      'France',  '2024-06-01'),
('NeverOrders Ltd', 'hello@neverorders.com', 'Sydney',     'Australia','2024-08-20');

-- Products
INSERT INTO Products (Name, Category, Price, StockQuantity) VALUES
('Wireless Mouse',       'Electronics', 29.99,  150),
('Mechanical Keyboard',  'Electronics', 89.99,   75),
('USB-C Hub',            'Electronics', 45.00,  200),
('Standing Desk',        'Furniture',  499.99,   20),
('Monitor Arm',          'Furniture',   79.99,   60),
('C# in Depth',          'Books',       39.99,  100),
('Design Patterns',      'Books',       44.99,   80),
('Notebook Pack',        'Stationery',  12.99,  500),
('Whiteboard Markers',   'Stationery',   8.49,  300),
('Ergonomic Chair',      'Furniture',  349.99,   30);

-- Orders (mix of statuses and dates)
INSERT INTO Orders (CustomerId, OrderDate, Status) VALUES
(1, '2025-01-10', 'Delivered'),
(1, '2025-03-15', 'Shipped'),
(2, '2025-02-20', 'Delivered'),
(2, '2025-06-01', 'Pending'),
(3, '2025-04-10', 'Delivered'),
(3, '2025-07-22', 'Shipped'),
(4, '2025-05-05', 'Pending'),
(4, '2025-08-15', 'Delivered'),
(5, '2025-01-20', 'Cancelled'),
(5, '2025-09-01', 'Pending'),
(1, '2025-11-10', 'Pending'),
(2, '2024-12-01', 'Delivered'),
(3, '2024-11-15', 'Pending');

-- OrderItems
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
-- Order 1 (Acme, Delivered)
(1, 1, 10, 29.99),
(1, 2,  5, 89.99),
-- Order 2 (Acme, Shipped)
(2, 3,  8, 45.00),
(2, 6,  3, 39.99),
-- Order 3 (GlobalTech, Delivered)
(3, 4,  2, 499.99),
(3, 5,  4, 79.99),
-- Order 4 (GlobalTech, Pending)
(4, 7,  6, 44.99),
(4, 8, 20, 12.99),
-- Order 5 (StartupXYZ, Delivered)
(5, 1, 15, 29.99),
(5, 10, 1, 349.99),
-- Order 6 (StartupXYZ, Shipped)
(6, 2,  3, 89.99),
(6, 9, 10,  8.49),
-- Order 7 (MegaRetail, Pending)
(7, 4,  5, 499.99),
(7, 10, 3, 349.99),
-- Order 8 (MegaRetail, Delivered)
(8, 1, 50, 29.99),
(8, 3, 20, 45.00),
-- Order 9 (LocalShop, Cancelled)
(9, 6,  2, 39.99),
(9, 7,  2, 44.99),
-- Order 10 (LocalShop, Pending)
(10, 8, 50, 12.99),
(10, 9, 30,  8.49),
-- Order 11 (Acme, Pending)
(11, 2, 10, 89.99),
(11, 5,  6, 79.99),
-- Order 12 (GlobalTech, Delivered, from 2024)
(12, 1, 20, 29.99),
(12, 6, 10, 39.99),
-- Order 13 (StartupXYZ, Pending, from 2024)
(13, 4,  1, 499.99),
(13, 3,  5, 45.00);

PRINT 'Database JD02_QueryExercise created and seeded successfully.';
GO

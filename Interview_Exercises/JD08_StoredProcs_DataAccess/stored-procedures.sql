-- ============================================================
-- JD08 - Stored Procedures
-- Write your stored procedures here. Run AFTER setup.sql.
-- ============================================================
USE JD08_InventoryDB;
GO

-- ============================================================
-- 1. usp_GetProductById
-- Parameters: @ProductId INT
-- Returns: Single product row or empty result set.
-- ============================================================

-- TODO: Write your stored procedure here


-- ============================================================
-- 2. usp_GetProductsByCategory
-- Parameters: @Category NVARCHAR(100)
-- Returns: All products in the category, ordered by Name.
-- ============================================================

-- TODO: Write your stored procedure here


-- ============================================================
-- 3. usp_CreateProduct
-- Parameters: @Name, @Category, @Price, @StockQuantity
-- Returns: The newly created product row (including generated Id).
-- ============================================================

-- TODO: Write your stored procedure here


-- ============================================================
-- 4. usp_UpdateProductStock
-- Parameters: @ProductId INT, @QuantityChange INT
-- Updates StockQuantity by adding @QuantityChange (can be negative).
-- Must use a TRANSACTION.
-- Must THROW an error if new stock would be < 0.
-- Returns: Updated product row.
-- ============================================================

-- TODO: Write your stored procedure here


-- ============================================================
-- 5. usp_GetInventoryReport
-- No parameters.
-- Returns: Category, ProductCount, TotalStockValue, AvgPrice
-- Use a CTE or temp table for the aggregation.
-- ============================================================

-- TODO: Write your stored procedure here

-- ============================================================
-- JD02 - SQL Querying Fundamentals
-- Write your queries below each numbered section.
-- ============================================================
USE JD02_QueryExercise;
GO

-- ============================================================
-- QUERY 1: Basic SELECT with WHERE and ORDER BY
-- Return all employees in the 'Engineering' department,
-- ordered by salary descending.
-- Expected columns: FirstName, LastName, Salary, HireDate
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 2: INNER JOIN
-- Return all orders with the customer name and total item count
-- per order.
-- Expected columns: OrderId, CustomerName, OrderDate, Status, TotalItems
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 3: LEFT JOIN to find orphaned records
-- Return all customers who have NEVER placed an order.
-- Expected columns: Id, Name, Email, City
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 4: GROUP BY with HAVING
-- Return departments where the average employee salary
-- exceeds $80,000.
-- Expected columns: DepartmentName, EmployeeCount, AvgSalary
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 5: Subquery in WHERE
-- Return employees who earn more than the average salary
-- of their own department.
-- Expected columns: FirstName, LastName, Salary, DepartmentName, DeptAvgSalary
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 6: Correlated subquery
-- For each product, show the product name and the total
-- quantity ever ordered (across all orders). Include products
-- with zero orders.
-- Expected columns: ProductName, Category, TotalQuantityOrdered
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 7: Common Table Expression (CTE)
-- Using a CTE, find the top 3 customers by total spending
-- (sum of Quantity * UnitPrice across all their orders).
-- Expected columns: CustomerName, TotalSpending
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 8: Window function - ROW_NUMBER
-- Rank employees within each department by salary (highest first).
-- Show rank, name, department, and salary.
-- Expected columns: SalaryRank, FirstName, LastName, DepartmentName, Salary
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 9: Running total with SUM OVER
-- Show all orders chronologically with a running total of
-- order amounts (each order's total = sum of its items'
-- Quantity * UnitPrice).
-- Expected columns: OrderId, OrderDate, CustomerName, OrderTotal, RunningTotal
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 10: PIVOT
-- Show total sales (sum of Quantity * UnitPrice) per product
-- category, pivoted by order status.
-- Columns: Category, Pending, Shipped, Delivered, Cancelled
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 11: Multi-table JOIN with aggregation (Report)
-- Management report showing for each department:
-- - Department name
-- - Number of employees
-- - Average salary
-- - Total budget
-- - Highest paid employee name
-- Order by employee count descending.
-- Expected columns: DepartmentName, EmployeeCount, AvgSalary, Budget, HighestPaidEmployee
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 12a: UPDATE with condition
-- Update all 'Pending' orders that are older than 30 days
-- (from GETUTCDATE()) to 'Cancelled'.
-- ============================================================

-- TODO: Write your UPDATE statement here


-- ============================================================
-- QUERY 12b: DELETE with subquery
-- Delete all OrderItems that belong to 'Cancelled' orders.
-- ============================================================

-- TODO: Write your DELETE statement here

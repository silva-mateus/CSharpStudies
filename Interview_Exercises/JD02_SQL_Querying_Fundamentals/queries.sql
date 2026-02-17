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
SELECT
	FirstName,
	LastName,
	Salary,
	HireDate
FROM dbo.Employees e
JOIN dbo.Departments d on e.DepartmentId = d.Id
WHERE d.Name = 'Engineering'
ORDER BY Salary DESC;

-- ============================================================
-- QUERY 2: INNER JOIN
-- Return all orders with the customer name and total item count
-- per order.
-- Expected columns: OrderId, CustomerName, OrderDate, Status, TotalItems
-- ============================================================

SELECT 
	o.Id AS OrderId,
	c.Name AS CustomerName,
	o.OrderDate AS OrderDate,
	o.Status AS Status,
	SUM(oi.Quantity) AS TotalItems
FROM dbo.Orders o
JOIN dbo.Customers c
	ON o.CustomerId = c.Id
JOIN dbo.OrderItems oi
	ON o.Id = oi.OrderId
GROUP BY
	o.Id,
	c.Name,
	o.OrderDate,
	o.Status;

-- ============================================================
-- QUERY 3: LEFT JOIN to find orphaned records
-- Return all customers who have NEVER placed an order.
-- Expected columns: Id, Name, Email, City
-- ============================================================

SELECT
	c.Id,
	c.Name,
	c.Email,
	c.City
FROM dbo.Customers c
LEFT JOIN dbo.Orders o
	on c.Id = o.CustomerId
WHERE
	o.Id IS NULL;


-- ============================================================
-- QUERY 4: GROUP BY with HAVING
-- Return departments where the average employee salary
-- exceeds $80,000.
-- Expected columns: DepartmentName, EmployeeCount, AvgSalary
-- ============================================================

SELECT
	d.Name as DepartmentName,
	Count(e.Id) as EmployeeCount,
	AVG(e.Salary) as AvgSalary
FROM dbo.Employees e
join dbo.Departments d
	on e.DepartmentId = d.Id
group by
	d.Name
having AVG(e.Salary) > 80000;

-- ============================================================
-- QUERY 5: Subquery in WHERE
-- Return employees who earn more than the average salary
-- of their own department.
-- Expected columns: FirstName, LastName, Salary, DepartmentName, DeptAvgSalary
-- ============================================================
SELECT
	e.FirstName,
	e.LastName,
	e.Salary,
	d.Name as DepartmentName,
	(
		SELECT AVG(Salary)
		from Employees
		WHERE DepartmentId = e.DepartmentId
	) as DeptAvgSalary
FROM dbo.Employees e
join dbo.Departments d
	on e.DepartmentId = d.Id
WHERE e.Salary > (
	SELECT AVG(Salary)
	from Employees
	WHERE DepartmentId = e.DepartmentId
);


-- ============================================================
-- QUERY 6: Correlated subquery
-- For each product, show the product name and the total
-- quantity ever ordered (across all orders). Include products
-- with zero orders.
-- Expected columns: ProductName, Category, TotalQuantityOrdered
-- ============================================================

SELECT 
	p.Name as ProductName,
	p.Category,
	Coalesce(
		(
			SELECT SUM(Quantity)
			from dbo.OrderItems oi
			where oi.ProductId = p.Id
		),0
	) as TotalQuantityOrdered
FROM dbo.Products p;

-- ============================================================
-- QUERY 7: Common Table Expression (CTE)
-- Using a CTE, find the top 3 customers by total spending
-- (sum of Quantity * UnitPrice across all their orders).
-- Expected columns: CustomerName, TotalSpending
-- ============================================================

WITH customerRanks as (
	SELECT 
		o.CustomerId,
		SUM(oi.Quantity * oi.UnitPrice) as TotalSpending
	FROM dbo.Orders o
	join dbo.OrderItems oi
		on o.Id = oi.OrderId
	group by
		o.CustomerId
)
SELECT TOP 3
	c.Name,
	cr.TotalSpending
FROM dbo.Customers c
join customerRanks cr
	on cr.CustomerId = c.Id
order by cr.TotalSpending DESC;

-- ============================================================
-- QUERY 8: Window function - ROW_NUMBER
-- Rank employees within each department by salary (highest first).
-- Show rank, name, department, and salary.
-- Expected columns: SalaryRank, FirstName, LastName, DepartmentName, Salary
-- ============================================================

SELECT
	ROW_NUMBER() OVER (
		PARTITION BY e.DepartmentId
		ORDER BY e.Salary DESC
	) as SalaryRank,
	e.FirstName,
	e.LastName,
	d.Name as DepartmentName,
	e.Salary
FROM dbo.Employees e
join dbo.Departments d on e.DepartmentId = d.Id
ORDER BY d.Name, SalaryRank;

-- ============================================================
-- QUERY 9: Running total with SUM OVER
-- Show all orders chronologically with a running total of
-- order amounts (each order's total = sum of its items'
-- Quantity * UnitPrice).
-- Expected columns: OrderId, OrderDate, CustomerName, OrderTotal, RunningTotal
-- ============================================================
WITH OrderTotals AS (
SELECT 
	o.Id AS OrderId,
	o.OrderDate,
	c.Name AS CustomerName,
	SUM(oi.Quantity * oi.UnitPrice) AS OrderTotal
FROM dbo.Orders o
JOIN dbo.Customers c
	ON o.CustomerId = c.Id
JOIN dbo.OrderItems oi
	ON o.Id = oi.OrderId
GROUP BY
	o.Id,
	o.OrderDate,
	c.Name
)
SELECT
	OrderId,
	OrderDate,
	CustomerName,
	OrderTotal,
	SUM(OrderTotal) OVER (
		ORDER BY OrderDate
		ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
	) AS RunningTotal
FROM OrderTotals
ORDER BY OrderDate;

-- ============================================================
-- QUERY 10: PIVOT
-- Show total sales (sum of Quantity * UnitPrice) per product
-- category, pivoted by order status.
-- Columns: Category, Pending, Shipped, Delivered, Cancelled
-- ============================================================
SELECT
	Category,
	ISNULL([Pending],0) as Pending,
	ISNULL([Shipped],0) as Shipped,
	ISNULL([Delivered],0) as Delivered,
	ISNULL([Cancelled],0) as Cancelled
FROM (
	SELECT
		p.Category,
		o.Status,
		oi.Quantity * oi.UnitPrice as SalesAmount
		FROM dbo.OrderItems oi
	JOIN dbo.Orders o
		on oi.OrderId = o.Id
	JOIN dbo.Products p
		on oi.ProductId = p.Id
) src
PIVOT (
	SUM(SalesAmount)
	FOR Status IN ([Pending], [Shipped], [Delivered], [Cancelled])
) pvt
order by Category;

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

WITH DeptStats AS (
	SELECT
		DepartmentId,
		COUNT(*) AS EmployeeCount,
		AVG(Salary) AS AvgSalary
	FROM dbo.Employees
	GROUP BY DepartmentId
),
HighestPaid as (
	SELECT
		DepartmentId,
		CONCAT(FirstName, ' ', LastName) AS HighestPaidEmployee,
		ROW_NUMBER() OVER (
			PARTITION BY DepartmentId
			ORDER BY Salary DESC
		) AS rn
	FROM dbo.Employees
)
SELECT
	d.Name as DepartmentName,
	ds.EmployeeCount,
	ds.AvgSalary,
	d.Budget,
	hp.HighestPaidEmployee
FROM dbo.Departments d
LEFT JOIN DeptStats ds
	on d.Id = ds.DepartmentId
LEFT JOIN HighestPaid hp
	on d.Id = hp.DepartmentId
	AND hp.rn = 1
ORDER BY ds.EmployeeCount DESC;

-- ============================================================
-- QUERY 12a: UPDATE with condition
-- Update all 'Pending' orders that are older than 30 days
-- (from GETUTCDATE()) to 'Cancelled'.
-- ============================================================

BEGIN TRANSACTION

UPDATE dbo.Orders
SET Status = 'Cancelled'
WHERE 
	Status = 'Pending'
AND
	OrderDate < DATEADD(DAY, -30, GETUTCDATE());

SELECT @@ROWCOUNT;

ROLLBACK

-- Select to confirm
SELECT  
    *,
    DATEDIFF(DAY, OrderDate, GETUTCDATE()) as DateDifference
FROM dbo.Orders
WHERE 
    Status = 'Pending'
AND
    OrderDate < DATEADD(DAY, -30, GETUTCDATE());


-- ============================================================
-- QUERY 12b: DELETE with subquery
-- Delete all OrderItems that belong to 'Cancelled' orders.
-- ============================================================

BEGIN TRANSACTION

DELETE FROM dbo.OrderItems
WHERE OrderId IN (
	SELECT Id
	FROM dbo.Orders
	WHERE Status = 'Cancelled'
);

SELECT @@ROWCOUNT as RowsDeleted;

ROLLBACK

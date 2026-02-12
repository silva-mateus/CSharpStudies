-- ============================================================
-- JD03 - Database Schema Design
-- Library Management System - Sample Application Queries
-- ============================================================

USE JD03_LibraryDB;
GO

-- ============================================================
-- QUERY 1: Available Copies
-- For a given book (by ISBN), find all copies not currently on loan.
-- Expected columns: CopyId, Barcode, Condition, BookTitle
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 2: Overdue Loans
-- List all overdue loans (DueDate < GETUTCDATE() and ReturnDate IS NULL).
-- Expected columns: LoanId, MemberName, BookTitle, Barcode, DueDate, DaysOverdue
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 3: Member Loan History
-- For a given member (by Id), show all their loans with book details.
-- Expected columns: LoanDate, DueDate, ReturnDate, BookTitle, ISBN, Barcode
-- Order by LoanDate descending.
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 4: Popular Books This Month
-- Top 10 books by number of loans started in the current month.
-- Expected columns: BookTitle, ISBN, LoanCount
-- ============================================================

-- TODO: Write your query here


-- ============================================================
-- QUERY 5: Members with High Unpaid Fines
-- Members whose total unpaid fines exceed $10.00.
-- Expected columns: MemberId, MemberName, Email, TotalUnpaidFines
-- ============================================================

-- TODO: Write your query here

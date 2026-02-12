-- ============================================================
-- JD03 - Database Schema Design
-- Library Management System - Schema DDL
-- ============================================================
-- TODO: Write your CREATE TABLE statements here.
-- Target: SQL Server T-SQL syntax.
-- Requirements:
--   - All tables must have a PRIMARY KEY
--   - Foreign keys with ON DELETE behavior specified
--   - UNIQUE constraints where specified in requirements
--   - CHECK constraints for valid status values
--   - DEFAULT values where appropriate
--   - Indexes for the common query patterns
--   - Minimum 3NF normalization
--
-- Tables needed:
--   1. Categories
--   2. Books
--   3. BookCategories (many-to-many)
--   4. Authors
--   5. BookAuthors (many-to-many)
--   6. Members
--   7. Copies
--   8. Loans
--   9. Reservations
--  10. Fines
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'JD03_LibraryDB')
BEGIN
    ALTER DATABASE JD03_LibraryDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE JD03_LibraryDB;
END
GO

CREATE DATABASE JD03_LibraryDB;
GO

USE JD03_LibraryDB;
GO

-- TODO: Write your CREATE TABLE statements below

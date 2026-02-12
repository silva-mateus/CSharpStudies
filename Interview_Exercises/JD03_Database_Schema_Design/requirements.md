# JD03 - Database Schema Design

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Design exercise (DDL + documentation)

## Overview

Design a normalized relational database schema for a **Library Management System** based on the business requirements below. Write the DDL, seed data, sample queries, and document your design decisions. This exercise tests your ability to translate business requirements into a well-structured database.

## Business Requirements

### Entities

1. **Books**: The library has a catalog of books. Each book has a title, ISBN (unique, 13 chars), publication year, and page count. A book belongs to one or more categories (e.g., "Fiction", "Science", "History").

2. **Authors**: Books are written by authors. An author has a first name, last name, and optional biography. A book can have multiple authors, and an author can write multiple books.

3. **Members**: Library members have a first name, last name, email (unique), phone, and membership start date. Members can be "Active", "Suspended", or "Expired".

4. **Copies**: The library can own multiple physical copies of the same book. Each copy has a unique barcode and a condition status ("New", "Good", "Worn", "Damaged").

5. **Loans**: When a member borrows a copy, a loan record is created with a loan date, due date (14 days from loan date by default), and optional return date. A copy can only be on one active loan at a time.

6. **Reservations**: Members can reserve a book (not a specific copy). When any copy becomes available, the oldest reservation for that book should be fulfilled. A reservation has a status ("Pending", "Fulfilled", "Cancelled") and a reservation date.

7. **Fines**: When a loan is returned late, a fine is generated. Fines have an amount ($0.50 per day late), a status ("Unpaid", "Paid", "Waived"), and are linked to the loan.

### Business Rules

- A member cannot have more than 5 active loans at a time.
- A member cannot borrow if they have any unpaid fines over $10.00.
- ISBN must be unique across all books.
- A copy's barcode must be unique.
- When a loan is returned late, the fine amount should be calculable from the due date and return date.

### Common Query Patterns (design indexes for these)

- Find all available copies of a specific book (copies not currently on loan).
- Find all overdue loans (due date < today, return date is NULL).
- Find a member's loan history with book titles.
- Find the most popular books this month (most loans).
- Find members with unpaid fines totaling over $10.

## Deliverables

Write the following files:

### 1. `schema.sql`
- CREATE TABLE statements for all tables
- Appropriate data types, primary keys, foreign keys
- UNIQUE constraints, CHECK constraints, DEFAULT values
- Indexes for the common query patterns listed above

### 2. `seed-data.sql`
- INSERT statements with realistic sample data:
  - At least 5 categories, 10 books, 8 authors, 15 book-author relationships
  - At least 10 members
  - At least 20 copies across the books
  - At least 15 loans (mix of active, returned, overdue)
  - At least 5 reservations
  - At least 5 fines

### 3. `sample-queries.sql`
Write these 5 queries that the application would need:
1. **Available copies**: For a given book (by ISBN), find all copies not currently on loan.
2. **Overdue loans**: List all overdue loans with member name, book title, days overdue.
3. **Member loan history**: For a given member, show all loans with book title, loan/due/return dates, sorted by loan date descending.
4. **Popular books this month**: Top 10 books by number of loans started in the current month.
5. **Members with high fines**: Members whose total unpaid fines exceed $10, with the total amount.

### 4. `design-decisions.md`
Document your design decisions:
- Why you chose certain data types (e.g., DECIMAL vs MONEY for fines)
- Normalization level and any intentional denormalization
- Index strategy and why each index was chosen
- How you handle the many-to-many relationships
- Any constraints you added and why

## Constraints

- Target SQL Server T-SQL syntax.
- Schema must be in at least 3rd Normal Form (3NF).
- Use appropriate data types (don't use NVARCHAR(MAX) everywhere).
- All foreign keys must have ON DELETE behavior specified.

## Topics Covered

- Database normalization (1NF, 2NF, 3NF)
- DDL: CREATE TABLE, constraints (PK, FK, UNIQUE, CHECK, DEFAULT)
- Data type selection
- Index design for query performance
- Many-to-many relationship modeling
- Self-documenting schema design

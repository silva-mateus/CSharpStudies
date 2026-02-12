# JD09 - Comprehensive Integration & Acceptance Testing

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Write tests for a provided API

## Overview

A complete, working ASP.NET Core Library Management API is provided. Your task is to write a comprehensive test suite covering all layers of the test pyramid: unit tests, integration tests, and acceptance tests. This exercise focuses on the "support system and solution integration testing, user acceptance testing" aspect of the role.

## Provided Application (do NOT modify)

The Library API has the following endpoints:

### Books
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/books` | List books (filter by `?author=`, `?available=true`) |
| GET | `/books/{id}` | Get book details with availability |
| POST | `/books` | Add a new book |

### Members
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/members` | List all members |
| GET | `/members/{id}` | Get member with active loans |
| POST | `/members` | Register a new member |

### Loans
| Method | Route | Description |
|--------|-------|-------------|
| POST | `/loans` | Borrow a book `{ "memberId": 1, "bookId": 1 }` |
| POST | `/loans/{id}/return` | Return a book |
| GET | `/loans/overdue` | List overdue loans |

### Business Rules (implemented in the service layer)
- A member can have max 5 active loans.
- A book can only be loaned if at least 1 copy is available.
- Loan period is 14 days from loan date.
- Returning a book after the due date generates a fine of $0.50/day.
- A member with unpaid fines > $10 cannot borrow.

## Tests to Write

### Unit Tests (15+ tests) - Test the service/business logic layer

Test the `LoanService` and `FineCalculator` with fakes:

1. Borrowing reduces available copies by 1.
2. Borrowing when no copies available returns error.
3. Member with 5 active loans cannot borrow.
4. Member with > $10 unpaid fines cannot borrow.
5. Returning a book increases available copies.
6. Returning on time generates no fine.
7. Returning 1 day late generates $0.50 fine.
8. Returning 10 days late generates $5.00 fine.
9. Loan due date is 14 days from loan date.
10. Returning already-returned loan returns error.
11. Invalid book ID returns error.
12. Invalid member ID returns error.
13. Fine calculation with zero days late returns 0.
14. Overdue loans query returns only loans past due date.
15. Available copies calculation is correct.

### Integration Tests (10+ tests) - Test API endpoints via WebApplicationFactory

1. GET /books returns 200 with list.
2. GET /books?available=true returns only books with available copies.
3. POST /books creates book, returns 201.
4. POST /books with invalid data returns 400.
5. GET /members/{id} includes active loans.
6. POST /loans borrows a book successfully (201).
7. POST /loans for unavailable book returns 400/409.
8. POST /loans/{id}/return returns the book (200).
9. GET /loans/overdue returns only overdue loans.
10. POST /members with duplicate email returns 409.

### Acceptance Tests (5+ tests) - End-to-end Given/When/Then scenarios

Write as descriptive test methods following Given/When/Then naming:

1. **Given** a member with no loans, **When** they borrow a book, **Then** the loan is created and the book's available count decreases.
2. **Given** a member borrows a book, **When** they return it on time, **Then** no fine is created and the book becomes available again.
3. **Given** a member borrows a book, **When** they return it 5 days late, **Then** a fine of $2.50 is created.
4. **Given** a member has 5 active loans, **When** they try to borrow another, **Then** the request is rejected.
5. **Given** a member has > $10 unpaid fines, **When** they try to borrow, **Then** the request is rejected.

## Constraints

- Do NOT modify the provided API code.
- Use xUnit and FluentAssertions.
- Create hand-rolled fakes for unit tests (no mocking library).
- Create test data builders or helper methods for readable test setup.
- All tests must pass when run together (no test-order dependencies).

## Topics Covered

- Test pyramid (unit, integration, acceptance)
- xUnit testing patterns
- WebApplicationFactory for integration tests
- Hand-rolled fakes for unit tests
- Test data builders
- Given/When/Then acceptance test style
- Testing business rules
- FluentAssertions

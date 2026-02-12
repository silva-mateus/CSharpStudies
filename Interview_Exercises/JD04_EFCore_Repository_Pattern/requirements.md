# JD04 - EF Core Repository with SQL Server

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch (with integration tests)

## Overview

Build a data access layer for a Blog platform using Entity Framework Core with SQL Server LocalDB. This exercise tests your ability to define EF Core models, configure relationships with Fluent API, implement the repository pattern, and write integration tests against a real database.

## Requirements

### Domain Model

| Entity | Properties |
|--------|-----------|
| Author | Id (int), FirstName, LastName, Email (unique), Bio (optional), CreatedAt |
| Post | Id (int), Title, Content, PublishedAt (nullable), IsPublished, AuthorId (FK), CreatedAt, UpdatedAt |
| Comment | Id (int), Content, AuthorName, AuthorEmail, PostId (FK), CreatedAt |
| Tag | Id (int), Name (unique) |
| PostTag | PostId (FK), TagId (FK) -- many-to-many join |

### Relationships

- Author -> Posts: one-to-many (an author has many posts)
- Post -> Comments: one-to-many (a post has many comments, cascade delete)
- Post <-> Tag: many-to-many (via PostTag join table)

### EF Core Configuration

- Use **Fluent API** in `OnModelCreating` (NOT data annotations)
- Configure all relationships, constraints, indexes
- Configure `Author.Email` as unique
- Configure `Tag.Name` as unique, max length 50
- Configure `Post.Title` max length 200
- Configure cascade delete for Post -> Comments
- Configure the many-to-many join table explicitly

### Repository Interfaces

#### `IRepository<T>` (generic)
| Method | Description |
|--------|-------------|
| `Task<T?> GetByIdAsync(int id)` | Get entity by ID |
| `Task<IReadOnlyList<T>> GetAllAsync()` | Get all entities |
| `Task<T> AddAsync(T entity)` | Add and return entity |
| `Task UpdateAsync(T entity)` | Update entity |
| `Task DeleteAsync(int id)` | Delete by ID |

#### `IBlogRepository` (domain-specific)
| Method | Description |
|--------|-------------|
| `Task<IReadOnlyList<Post>> GetPostsByTagAsync(string tagName)` | Posts with a specific tag, include Author |
| `Task<IReadOnlyList<Post>> GetRecentPostsAsync(int count)` | Most recent published posts with Author and comment count |
| `Task<AuthorStatistics> GetAuthorStatisticsAsync(int authorId)` | Author name, post count, total comments, avg comments per post |
| `Task<Post?> GetPostWithDetailsAsync(int postId)` | Post with Author, Comments, and Tags loaded |

#### `AuthorStatistics` record
```csharp
public record AuthorStatistics(string AuthorName, int PostCount, int TotalComments, double AvgCommentsPerPost);
```

### Connection String

Use SQL Server LocalDB:
```
Server=(localdb)\\MSSQLLocalDB;Database=JD04_BlogDB;Trusted_Connection=True;
```

## Integration Tests to Write

Tests should create a fresh database, seed test data, run queries, then drop the database.

1. AddAsync creates an entity and assigns an Id.
2. GetByIdAsync returns the correct entity.
3. GetByIdAsync returns null for non-existent Id.
4. UpdateAsync modifies entity fields.
5. DeleteAsync removes the entity.
6. GetPostsByTagAsync returns only posts with the specified tag.
7. GetRecentPostsAsync returns published posts ordered by date.
8. GetAuthorStatisticsAsync returns correct counts.
9. GetPostWithDetailsAsync includes Author, Comments, and Tags.
10. Deleting a Post cascades to delete its Comments.

## Constraints

- Use Fluent API only (no `[Key]`, `[Required]`, etc. data annotations).
- Use `async/await` for all repository methods.
- Tests must use a real SQL Server LocalDB database (not InMemory provider).
- Each test class should create/drop its own test database.

## Topics Covered

- Entity Framework Core
- Fluent API configuration
- Repository pattern
- One-to-many and many-to-many relationships
- SQL Server LocalDB
- Integration testing with EF Core
- Async data access

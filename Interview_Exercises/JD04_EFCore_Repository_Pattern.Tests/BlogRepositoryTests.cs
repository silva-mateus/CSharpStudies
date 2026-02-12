using JD04_EFCore_Repository_Pattern.Data;
using JD04_EFCore_Repository_Pattern.Entities;
using JD04_EFCore_Repository_Pattern.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JD04_EFCore_Repository_Pattern.Tests;

/// <summary>
/// TODO: Write integration tests for BlogRepository against SQL Server LocalDB.
///
/// SETUP PATTERN:
/// - Each test class should use a unique database name (e.g., "JD04_BlogDB_Test_{Guid}").
/// - In the constructor, create the database with EnsureCreated().
/// - In Dispose(), drop the database with EnsureDeleted().
/// - Implement IDisposable for cleanup.
///
/// Example:
///   private readonly BlogDbContext _context;
///   public BlogRepositoryTests()
///   {
///       var options = new DbContextOptionsBuilder&lt;BlogDbContext&gt;()
///           .UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=JD04_Test_{Guid.NewGuid():N};Trusted_Connection=True;")
///           .Options;
///       _context = new BlogDbContext(options);
///       _context.Database.EnsureCreated();
///       SeedTestData();
///   }
///
/// TESTS TO WRITE:
///  1. AddAsync creates entity and assigns Id.
///  2. GetByIdAsync returns correct entity.
///  3. GetByIdAsync returns null for non-existent Id.
///  4. UpdateAsync modifies entity fields.
///  5. DeleteAsync removes entity.
///  6. GetPostsByTagAsync returns only posts with specified tag.
///  7. GetRecentPostsAsync returns published posts ordered by date, limited to count.
///  8. GetAuthorStatisticsAsync returns correct counts.
///  9. GetPostWithDetailsAsync includes Author, Comments, and Tags.
/// 10. Deleting a Post cascades to delete its Comments.
/// </summary>
public class BlogRepositoryTests : IDisposable
{
    // TODO: Set up context, seed data, write tests, clean up

    public void Dispose()
    {
        // TODO: _context.Database.EnsureDeleted(); _context.Dispose();
    }
}

using JD04_EFCore_Repository_Pattern.Entities;
using Microsoft.EntityFrameworkCore;

namespace JD04_EFCore_Repository_Pattern.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: Configure all entities using Fluent API (NOT data annotations).
        //
        // Author:
        //   - Email is required, unique, max length 200
        //   - FirstName and LastName required, max length 100
        //
        // Post:
        //   - Title required, max length 200
        //   - Content required
        //   - Has one Author with many Posts
        //   - Has many Comments (cascade delete)
        //   - Has many Tags via many-to-many (configure join table "PostTag")
        //
        // Comment:
        //   - Content, AuthorName, AuthorEmail required
        //   - Has one Post (cascade delete when post is deleted)
        //
        // Tag:
        //   - Name required, unique, max length 50
        //   - Many-to-many with Post

        throw new NotImplementedException("Configure Fluent API here");
    }
}

using JD04_EFCore_Repository_Pattern.Data;
using JD04_EFCore_Repository_Pattern.Entities;

namespace JD04_EFCore_Repository_Pattern.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }

    // TODO: Implement all IBlogRepository methods.
    // Use Include/ThenInclude for eager loading.
    // Use LINQ for filtering and aggregation.

    public Task<IReadOnlyList<Post>> GetPostsByTagAsync(string tagName)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Post>> GetRecentPostsAsync(int count)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorStatistics> GetAuthorStatisticsAsync(int authorId)
    {
        throw new NotImplementedException();
    }

    public Task<Post?> GetPostWithDetailsAsync(int postId)
    {
        throw new NotImplementedException();
    }
}

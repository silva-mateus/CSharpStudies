using JD04_EFCore_Repository_Pattern.Entities;

namespace JD04_EFCore_Repository_Pattern.Repositories;

public record AuthorStatistics(string AuthorName, int PostCount, int TotalComments, double AvgCommentsPerPost);

public interface IBlogRepository
{
    /// <summary>
    /// Returns all published posts that have the specified tag, including the Author.
    /// </summary>
    Task<IReadOnlyList<Post>> GetPostsByTagAsync(string tagName);

    /// <summary>
    /// Returns the most recent published posts (by PublishedAt), including Author
    /// and the comment count. Limited to <paramref name="count"/> results.
    /// </summary>
    Task<IReadOnlyList<Post>> GetRecentPostsAsync(int count);

    /// <summary>
    /// Returns statistics for the given author: name, post count,
    /// total comments across all posts, average comments per post.
    /// </summary>
    Task<AuthorStatistics> GetAuthorStatisticsAsync(int authorId);

    /// <summary>
    /// Returns a post with its Author, all Comments, and all Tags eagerly loaded.
    /// Returns null if not found.
    /// </summary>
    Task<Post?> GetPostWithDetailsAsync(int postId);
}

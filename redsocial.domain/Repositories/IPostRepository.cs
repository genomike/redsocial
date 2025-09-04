using RedSocial.Domain.Entities;

namespace RedSocial.Domain.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(string id);
        Task<Post> SaveAsync(Post post);
        Task<List<Post>> GetAllAsync();
        Task<List<Post>> GetByAuthorIdAsync(string authorId);
        Task UpdateAsync(Post post);
    }
}

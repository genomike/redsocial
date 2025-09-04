using Microsoft.EntityFrameworkCore;
using RedSocial.Domain.Entities;
using RedSocial.Domain.Repositories;
using RedSocial.Infrastructure.Data;

namespace RedSocial.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly RedSocialDbContext _context;

        public PostRepository(RedSocialDbContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetByIdAsync(string id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> SaveAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<List<Post>> GetByAuthorIdAsync(string authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}

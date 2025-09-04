using RedSocial.Domain.Entities;
using RedSocial.Domain.Repositories;

namespace RedSocial.Application.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // Constructor para tests (sin DI)
        public PostService()
        {
            _postRepository = new InMemoryPostRepository();
        }

        public async Task<Post> CreatePost(string employeeId, string content)
        {
            var id = Guid.NewGuid().ToString();
            var post = new Post(id, employeeId, content);
            
            return await _postRepository.SaveAsync(post);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Post?> GetPostById(string id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task AddLikeToPost(string postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            post.AddLike();
            await _postRepository.UpdateAsync(post);
        }
    }

    // Implementación en memoria para tests
    internal class InMemoryPostRepository : IPostRepository
    {
        private readonly List<Post> _posts = new();

        public Task<Post?> GetByIdAsync(string id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(post);
        }

        public Task<Post> SaveAsync(Post post)
        {
            _posts.Add(post);
            return Task.FromResult(post);
        }

        public Task<List<Post>> GetAllAsync()
        {
            return Task.FromResult(_posts.ToList());
        }

        public Task<List<Post>> GetByAuthorIdAsync(string authorId)
        {
            var posts = _posts.Where(p => p.AuthorId == authorId).ToList();
            return Task.FromResult(posts);
        }

        public Task UpdateAsync(Post post)
        {
            var existingPost = _posts.FirstOrDefault(p => p.Id == post.Id);
            if (existingPost != null)
            {
                var index = _posts.IndexOf(existingPost);
                _posts[index] = post;
            }
            return Task.CompletedTask;
        }
    }
}

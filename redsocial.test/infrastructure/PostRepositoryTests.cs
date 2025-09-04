using Microsoft.EntityFrameworkCore;
using RedSocial.Domain.Entities;
using RedSocial.Infrastructure.Data;
using RedSocial.Infrastructure.Repositories;

namespace redsocial.test.infrastructure
{
    public class PostRepositoryTests : IDisposable
    {
        private readonly RedSocialDbContext _context;
        private readonly PostRepository _repository;

        public PostRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RedSocialDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RedSocialDbContext(options);
            _repository = new PostRepository(_context);
        }

        [Fact]
        public async Task SaveAsync_WithValidPost_ShouldSaveToDatabase()
        {
            // GIVEN: Un post válido
            var post = new Post("post-001", "author-001", "This is a test post content");

            // WHEN: Se guarda en el repositorio
            var savedPost = await _repository.SaveAsync(post);

            // THEN: El post debe estar guardado correctamente
            Assert.NotNull(savedPost);
            Assert.Equal(post.AuthorId, savedPost.AuthorId);
            Assert.Equal(post.Content, savedPost.Content);
            Assert.Equal(0, savedPost.Likes);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnPost()
        {
            // GIVEN: Un post existente en la base de datos
            var post = new Post("post-002", "author-002", "Another test post");
            await _repository.SaveAsync(post);

            // WHEN: Se busca por ID
            var foundPost = await _repository.GetByIdAsync(post.Id);

            // THEN: Se debe retornar el post correcto
            Assert.NotNull(foundPost);
            Assert.Equal(post.Id, foundPost.Id);
            Assert.Equal(post.AuthorId, foundPost.AuthorId);
            Assert.Equal(post.Content, foundPost.Content);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // GIVEN: Un ID inexistente
            var invalidId = "non-existent-post-id";

            // WHEN: Se busca por ID inexistente
            var foundPost = await _repository.GetByIdAsync(invalidId);

            // THEN: Se debe retornar null
            Assert.Null(foundPost);
        }

        [Fact]
        public async Task GetAllAsync_WhenPostsExist_ShouldReturnAllPosts()
        {
            // GIVEN: Varios posts en la base de datos
            var post1 = new Post("post-003", "author-003", "First post content");
            var post2 = new Post("post-004", "author-004", "Second post content");
            
            await _repository.SaveAsync(post1);
            await _repository.SaveAsync(post2);

            // WHEN: Se solicitan todos los posts
            var allPosts = await _repository.GetAllAsync();

            // THEN: Se deben retornar todos los posts
            Assert.NotNull(allPosts);
            Assert.True(allPosts.Count >= 2);
            Assert.Contains(allPosts, p => p.Content == post1.Content);
            Assert.Contains(allPosts, p => p.Content == post2.Content);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoPostsExist_ShouldReturnEmptyList()
        {
            // GIVEN: Base de datos vacía
            // (No se agregan posts)

            // WHEN: Se solicitan todos los posts
            var allPosts = await _repository.GetAllAsync();

            // THEN: Se debe retornar una lista vacía
            Assert.NotNull(allPosts);
            Assert.Empty(allPosts);
        }

        [Fact]
        public async Task GetByAuthorIdAsync_WithValidAuthorId_ShouldReturnAuthorPosts()
        {
            // GIVEN: Posts de diferentes autores en la base de datos
            var authorId = "specific-author";
            var post1 = new Post("post-005", authorId, "First post by author");
            var post2 = new Post("post-006", "other-author", "Post by different author");
            var post3 = new Post("post-007", authorId, "Second post by author");
            
            await _repository.SaveAsync(post1);
            await _repository.SaveAsync(post2);
            await _repository.SaveAsync(post3);

            // WHEN: Se buscan posts por autor específico
            var authorPosts = await _repository.GetByAuthorIdAsync(authorId);

            // THEN: Se deben retornar solo los posts del autor especificado
            Assert.NotNull(authorPosts);
            Assert.Equal(2, authorPosts.Count);
            Assert.Contains(authorPosts, p => p.Content == post1.Content);
            Assert.Contains(authorPosts, p => p.Content == post3.Content);
            Assert.DoesNotContain(authorPosts, p => p.Content == post2.Content);
        }

        [Fact]
        public async Task GetByAuthorIdAsync_WithInvalidAuthorId_ShouldReturnEmptyList()
        {
            // GIVEN: Posts en la base de datos de otros autores
            var post = new Post("post-008", "existing-author", "Some post content");
            await _repository.SaveAsync(post);

            // WHEN: Se buscan posts por un autor inexistente
            var authorPosts = await _repository.GetByAuthorIdAsync("non-existent-author");

            // THEN: Se debe retornar una lista vacía
            Assert.NotNull(authorPosts);
            Assert.Empty(authorPosts);
        }

        [Fact]
        public async Task UpdateAsync_WithValidPost_ShouldUpdateInDatabase()
        {
            // GIVEN: Un post existente en la base de datos
            var post = new Post("post-009", "author-009", "Original content");
            await _repository.SaveAsync(post);

            // Agregar algunos likes
            post.AddLike();
            post.AddLike();

            // WHEN: Se actualiza el post
            await _repository.UpdateAsync(post);

            // THEN: Los cambios deben estar reflejados
            var updatedPost = await _repository.GetByIdAsync(post.Id);
            Assert.NotNull(updatedPost);
            Assert.Equal(2, updatedPost.Likes);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

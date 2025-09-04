using RedSocial.Application.Services;

namespace redsocial.test.application
{
    public class PostServiceTests
    {
        [Fact]
        public async Task CreatePost_WithValidEmployeeAndContent_ShouldReturnNewPost()
        {
            // GIVEN: Un servicio de posts, un empleado válido y contenido válido
            var postService = new PostService();
            var employeeId = "emp-001";
            var content = "This is a test post from the service layer";

            // WHEN: Se crea un post a través del servicio
            var post = await postService.CreatePost(employeeId, content);

            // THEN: Se debe retornar un post válido con los datos correctos
            Assert.NotNull(post);
            Assert.Equal(employeeId, post.AuthorId);
            Assert.Equal(content, post.Content);
            Assert.NotNull(post.Id);
            Assert.Equal(0, post.Likes);
        }

        [Fact]
        public async Task GetAllPosts_WhenPostsExist_ShouldReturnAllPosts()
        {
            // GIVEN: Un servicio de posts con posts existentes
            var postService = new PostService();
            var post1 = await postService.CreatePost("emp-001", "First post");
            var post2 = await postService.CreatePost("emp-002", "Second post");

            // WHEN: Se solicitan todos los posts
            var allPosts = await postService.GetAllPosts();

            // THEN: Se deben retornar todos los posts creados
            Assert.NotNull(allPosts);
            Assert.Contains(post1, allPosts);
            Assert.Contains(post2, allPosts);
            Assert.True(allPosts.Count >= 2);
        }

        [Fact]
        public async Task GetAllPosts_WhenNoPostsExist_ShouldReturnEmptyList()
        {
            // GIVEN: Un servicio de posts sin posts creados
            var postService = new PostService();

            // WHEN: Se solicitan todos los posts
            var allPosts = await postService.GetAllPosts();

            // THEN: Se debe retornar una lista vacía
            Assert.NotNull(allPosts);
            Assert.Empty(allPosts);
        }

        [Fact]
        public async Task AddLikeToPost_WithValidPostId_ShouldIncrementLikes()
        {
            // GIVEN: Un servicio de posts con un post existente
            var postService = new PostService();
            var post = await postService.CreatePost("emp-001", "Post to like");
            var initialLikes = post.Likes;

            // WHEN: Se agrega un like al post a través del servicio
            await postService.AddLikeToPost(post.Id);

            // THEN: El número de likes del post se debe incrementar
            var updatedPost = await postService.GetPostById(post.Id);
            Assert.Equal(initialLikes + 1, updatedPost!.Likes);
        }

        [Fact]
        public async Task AddLikeToPost_WithInvalidPostId_ShouldThrowException()
        {
            // GIVEN: Un servicio de posts y un ID de post inexistente
            var postService = new PostService();
            var invalidPostId = "non-existent-post";

            // WHEN: Se intenta agregar un like a un post inexistente
            // THEN: Se debe lanzar una excepción
            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await postService.AddLikeToPost(invalidPostId));
        }

        [Fact]
        public async Task GetPostById_WithValidId_ShouldReturnCorrectPost()
        {
            // GIVEN: Un servicio de posts con un post existente
            var postService = new PostService();
            var expectedContent = "Test post content";
            var post = await postService.CreatePost("emp-001", expectedContent);

            // WHEN: Se busca el post por su ID
            var foundPost = await postService.GetPostById(post.Id);

            // THEN: Se debe retornar el post correcto
            Assert.NotNull(foundPost);
            Assert.Equal(post.Id, foundPost.Id);
            Assert.Equal(expectedContent, foundPost.Content);
        }

        [Fact]
        public async Task GetPostById_WithInvalidId_ShouldReturnNull()
        {
            // GIVEN: Un servicio de posts y un ID inexistente
            var postService = new PostService();
            var invalidId = "non-existent-id";

            // WHEN: Se busca un post con un ID inexistente
            var foundPost = await postService.GetPostById(invalidId);

            // THEN: Se debe retornar null
            Assert.Null(foundPost);
        }
    }
}

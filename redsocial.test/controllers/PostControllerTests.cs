using Microsoft.AspNetCore.Mvc;
using RedSocial.Api.Controllers;
using RedSocial.Api.DTOs;
using RedSocial.Application.Services;

namespace redsocial.test.controllers
{
    public class PostControllerTests
    {
        private PostController CreateController()
        {
            var postService = new PostService();
            return new PostController(postService);
        }

        [Fact]
        public async Task CreatePost_WithValidData_ShouldReturn201Created()
        {
            // GIVEN: Un controlador y datos válidos de post
            var controller = CreateController();
            var request = new CreatePostRequest
            {
                AuthorId = "test-author-id",
                Content = "This is a test post"
            };

            // WHEN: Se envía una petición POST para crear un post
            var result = await controller.CreatePost(request);

            // THEN: Se debe retornar 201 Created con los datos del post
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<PostResponse>(actionResult.Value);
            
            Assert.Equal(request.AuthorId, response.AuthorId);
            Assert.Equal(request.Content, response.Content);
            Assert.NotNull(response.Id);
            Assert.Equal(0, response.Likes);
        }

        [Fact]
        public async Task CreatePost_WithEmptyContent_ShouldReturn400BadRequest()
        {
            // GIVEN: Un controlador y datos inválidos (contenido vacío)
            var controller = CreateController();
            var request = new CreatePostRequest
            {
                AuthorId = "test-author-id",
                Content = ""
            };

            // WHEN: Se envía una petición POST con contenido vacío
            var result = await controller.CreatePost(request);

            // THEN: Se debe retornar 400 Bad Request
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreatePostsBatch_WithValidData_ShouldReturnListOfPosts()
        {
            // GIVEN: Un controlador y una lista de posts válidos
            var controller = CreateController();
            var requests = new List<CreatePostRequest>
            {
                new CreatePostRequest { AuthorId = "author1", Content = "First post content" },
                new CreatePostRequest { AuthorId = "author2", Content = "Second post content" },
                new CreatePostRequest { AuthorId = "author1", Content = "Third post content" }
            };

            // WHEN: Se envía una petición POST batch
            var result = await controller.CreatePostsBatch(requests);

            // THEN: Se deben crear todos los posts exitosamente
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var responses = Assert.IsType<List<PostResponse>>(actionResult.Value);
            
            Assert.Equal(3, responses.Count);
            Assert.All(responses, r => Assert.NotNull(r.Id));
            Assert.All(responses, r => Assert.Equal(0, r.Likes));
        }

        [Fact]
        public async Task CreatePostsBatch_WithSomeInvalidData_ShouldReturnPartialSuccess()
        {
            // GIVEN: Un controlador y una lista mixta (válidos e inválidos)
            var controller = CreateController();
            var requests = new List<CreatePostRequest>
            {
                new CreatePostRequest { AuthorId = "author1", Content = "Valid post content" },
                new CreatePostRequest { AuthorId = "author2", Content = "" }, // Contenido vacío
                new CreatePostRequest { AuthorId = "author3", Content = "Another valid post" }
            };

            // WHEN: Se envía una petición POST batch
            var result = await controller.CreatePostsBatch(requests);

            // THEN: Se deben crear solo los posts válidos y retornar info de errores
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task CreatePostsBatch_WithEmptyList_ShouldReturnEmptyList()
        {
            // GIVEN: Un controlador y una lista vacía
            var controller = CreateController();
            var requests = new List<CreatePostRequest>();

            // WHEN: Se envía una petición POST batch vacía
            var result = await controller.CreatePostsBatch(requests);

            // THEN: Se debe retornar una lista vacía
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var responses = Assert.IsType<List<PostResponse>>(actionResult.Value);
            
            Assert.Empty(responses);
        }

        [Fact]
        public async Task GetAllPosts_WhenPostsExist_ShouldReturnListOfPosts()
        {
            // GIVEN: Un controlador con posts existentes
            var controller = CreateController();
            
            // Crear algunos posts
            var requests = new List<CreatePostRequest>
            {
                new CreatePostRequest { AuthorId = "author1", Content = "Post 1 content" },
                new CreatePostRequest { AuthorId = "author2", Content = "Post 2 content" }
            };

            await controller.CreatePostsBatch(requests);

            // WHEN: Se solicitan todos los posts
            var result = await controller.GetAllPosts();

            // THEN: Se debe retornar la lista de posts
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var responses = Assert.IsType<List<PostResponse>>(actionResult.Value);
            
            Assert.True(responses.Count >= 2);
            Assert.All(responses, r => Assert.NotNull(r.Id));
        }

        [Fact]
        public async Task GetAllPosts_WhenNoPostsExist_ShouldReturnEmptyList()
        {
            // GIVEN: Un controlador sin posts
            var controller = CreateController();

            // WHEN: Se solicitan todos los posts
            var result = await controller.GetAllPosts();

            // THEN: Se debe retornar una lista vacía
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var responses = Assert.IsType<List<PostResponse>>(actionResult.Value);
            
            Assert.Empty(responses);
        }

        [Fact]
        public async Task GetPostById_WithValidId_ShouldReturnPost()
        {
            // GIVEN: Un controlador y un post existente
            var controller = CreateController();
            var createRequest = new CreatePostRequest
            {
                AuthorId = "test-author",
                Content = "Test post content"
            };

            var createResult = await controller.CreatePost(createRequest);
            var createdPost = ((CreatedAtActionResult)createResult.Result!).Value as PostResponse;

            // WHEN: Se busca el post por ID
            var result = await controller.GetPostById(createdPost!.Id);

            // THEN: Se debe retornar el post correcto
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PostResponse>(actionResult.Value);
            
            Assert.Equal(createdPost.Id, response.Id);
            Assert.Equal(createdPost.AuthorId, response.AuthorId);
            Assert.Equal(createdPost.Content, response.Content);
        }

        [Fact]
        public async Task GetPostById_WithInvalidId_ShouldReturn404NotFound()
        {
            // GIVEN: Un controlador y un ID inexistente
            var controller = CreateController();
            var invalidId = "non-existent-id";

            // WHEN: Se busca un post inexistente
            var result = await controller.GetPostById(invalidId);

            // THEN: Se debe retornar 404 Not Found
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddLikeToPost_WithValidId_ShouldReturn200Ok()
        {
            // GIVEN: Un controlador y un post existente
            var controller = CreateController();
            var createRequest = new CreatePostRequest
            {
                AuthorId = "test-author",
                Content = "Test post for likes"
            };

            var createResult = await controller.CreatePost(createRequest);
            var createdPost = ((CreatedAtActionResult)createResult.Result!).Value as PostResponse;

            // WHEN: Se agrega un like al post
            var result = await controller.AddLikeToPost(createdPost!.Id);

            // THEN: Se debe retornar 200 OK
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task AddLikeToPost_WithInvalidId_ShouldReturn404NotFound()
        {
            // GIVEN: Un controlador y un ID inexistente
            var controller = CreateController();
            var invalidId = "non-existent-post-id";

            // WHEN: Se intenta agregar un like a un post inexistente
            var result = await controller.AddLikeToPost(invalidId);

            // THEN: Se debe retornar 404 Not Found
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

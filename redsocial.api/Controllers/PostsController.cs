using Microsoft.AspNetCore.Mvc;
using RedSocial.Api.DTOs;
using RedSocial.Application.Services;

namespace RedSocial.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Crear un nuevo post
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PostResponse>> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                var post = await _postService.CreatePost(request.AuthorId, request.Content);
                
                var response = new PostResponse
                {
                    Id = post.Id,
                    AuthorId = post.AuthorId,
                    Content = post.Content,
                    Likes = post.Likes
                };

                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtener todos los posts
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PostResponse>>> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            
            var response = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                Content = p.Content,
                Likes = p.Likes
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Obtener post por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse>> GetPostById(string id)
        {
            var post = await _postService.GetPostById(id);
            
            if (post == null)
            {
                return NotFound(new { message = $"Post with ID {id} not found" });
            }

            var response = new PostResponse
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Content = post.Content,
                Likes = post.Likes
            };

            return Ok(response);
        }

        /// <summary>
        /// Dar like a un post
        /// </summary>
        [HttpPost("{id}/like")]
        public async Task<ActionResult> AddLikeToPost(string id)
        {
            try
            {
                await _postService.AddLikeToPost(id);
                return Ok(new { message = "Like added successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
